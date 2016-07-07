using Acr.UserDialogs;
using CashApp.Mixins;
using CashApp.Models;
using CashApp.Services;
using FreshMvvm;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CashApp.PageModels
{
    [ImplementPropertyChanged]
    public class TransactionPageModel : FreshBasePageModel
    {
        private readonly IRestService service;
        private Grouping GroupingData;

        public TransactionPageModel(IRestService service)
        {
            this.service = service;
        }
        
        public override void Init(object initData)
        {
            GroupingData = initData as Grouping;
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
                    MessagingCenter.Send<FreshBasePageModel>(this.PreviousPageModel, "refresh");
                }
            }
        }

        private async Task RefreshData()
        {
            if (GroupingData != null)
            {
                var loading = UserDialogs.Instance.Loading("Loading data");

                loading.Show();
                IsBusy = true;
                var result = await service.GetPeriodData(GroupingData.Year, GroupingData.Month, null);

                if (result != null)
                {
                    Items = new ObservableCollection<Transaction>(result.OrderByDescending(x => x.TransDate).ThenByDescending(x => x.Id));
                }
                else
                {
                    Items = new ObservableCollection<Transaction>();
                }
                IsBusy = false;
                loading.Hide();
            }
        }

        private ObservableCollection<Transaction> items;
        public ObservableCollection<Transaction> Items
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

        private Transaction selectedItem;
        public Transaction SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;

                if (value != null)
                {
                    EditItemCommand.Execute(null);
                }
            }
        }

        public ICommand EditItemCommand
        {
            get
            {
                return new Command(Edit, () => SelectedItem != null);
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

        public ICommand ClosePageCommand
        {
            get
            {
                return new Command(async () => await CoreMethods.PopPageModel());
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

        public void Edit()
        {
            CoreMethods.PushPageModel<ItemPageModel>(data: SelectedItem.Id);
            SelectedItem = null;
        }
    }
}
