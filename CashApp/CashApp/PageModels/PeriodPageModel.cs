using Acr.UserDialogs;
using CashApp.Mixins;
using CashApp.Models;
using CashApp.Services;
using FreshMvvm;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CashApp.PageModels
{
    [ImplementPropertyChanged]
    public class PeriodPageModel : FreshBasePageModel
    {
        private readonly IRestService service;

        public PeriodPageModel(IRestService service)
        {
            this.service = service;
        }
        
        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            RefreshData().RunForget();
        }

        private async Task RefreshData()
        {
            var loading = UserDialogs.Instance.Loading("Loading data");

            loading.Show();
            IsBusy = true;
            var result = await service.GetPeriodData();

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
            loading.Hide();
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
                return new Command(Add, () => !IsBusy);
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
            CoreMethods.PushPageModel<ItemPageModel>(data: 0);
        }

        public void Select()
        {
            CoreMethods.PushPageModel<TransactionPageModel>(data: SelectedItem);
            SelectedItem = null;
        }
    }
}
