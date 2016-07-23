using Acr.UserDialogs;
using CashApp.Interfaces;
using CashApp.Mixins;
using CashApp.Models;
using FreshMvvm;
using Microsoft.WindowsAzure.MobileServices;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CashApp.PageModels
{
    [ImplementPropertyChanged]
    public class PeriodPageModel : FreshBasePageModel
    {
        private bool shouldRefresh;
        private readonly IDataService service;
        private readonly IUserDialogs userDialog;

        public PeriodPageModel(IDataService service, IUserDialogs userDialog)
        {
            this.service = service;
            this.userDialog = userDialog;
            MessagingCenter.Subscribe<FreshBasePageModel>(this, "refresh", (sender) =>
            {
                shouldRefresh = true;
            });
        }

        public override void Init(object initData)
        {
            //service.Purge();
            //service.Logout();
            //CashApp.Helpers.Settings.UserId = string.Empty;
            //CashApp.Helpers.Settings.AuthToken = string.Empty;
            RefreshData().RunForget();
        }

        public override void ReverseInit(object returnData)
        {
            if (returnData != null)
            {
                bool valid = false;
                bool.TryParse(returnData.ToString(), out valid);
                if (valid)
                {
                    RefreshData().RunForget();
                }
            }
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            if (shouldRefresh)
            {
                RefreshData().RunForget();
            }
        }

        private async Task RefreshData()
        {
            var isLoggedIn = false;
            try
            {
                isLoggedIn = CashApp.Helpers.Settings.IsLoggedIn;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return;
            }
            if (!CashApp.Helpers.Settings.IsLoggedIn)
            {
                await service.Initialize();
                var user = await DependencyService.Get<IAuthentication>().LoginAsync(service.MobileService, MobileServiceAuthenticationProvider.Google);
                if (user == null)
                    return;
            }

            using (var loading = userDialog.Loading("Loading data"))
            {
                IsBusy = true;

                var result = await service.GetTransactions();

                var grouping = new Grouping();
                var groupingList = new List<Grouping> { grouping };

                Items = new ObservableCollection<Grouping>(groupingList);
                if (result != null)
                {
                    var sorted = from record in result
                                 orderby record.PeriodSort descending
                                 group record by new { record.Year, record.Month, record.Period } into gr
                                 select new Grouping(gr.Key.Year, gr.Key.Month, gr.Key.Period, gr);
                    Items = new ObservableCollection<Grouping>(sorted);
                }
                IsBusy = false;
            }
            shouldRefresh = false;
        }

        private ObservableCollection<Grouping> items;
        public ObservableCollection<Grouping> Items
        {
            get { return items; }
            set
            {
                items = value;
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; }
        }

        private Grouping selectedItem;
        public Grouping SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;

                if (value != null)
                {
                    SelectItemCommand.Execute(null);
                }
            }
        }

        public ICommand SelectItemCommand
        {
            get
            {
                return new Command(Select, () => SelectedItem != null);
            }
        }

        public ICommand AddItemCommand
        {
            get
            {
                return new Command(Add);
            }
        }

        public ICommand LoadItemCommand
        {
            get
            {
                return new Command(async () => await Load());
            }
        }

        public async Task Load()
        {
            await RefreshData();
        }

        public void Add()
        {
            CoreMethods.PushPageModel<ItemPageModel>(data: string.Empty);
        }

        public void Select()
        {
            CoreMethods.PushPageModel<TransactionPageModel>(data: SelectedItem);
            SelectedItem = null;
        }
    }
}
