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

        public TransactionPageModel(IRestService service)
        {
            this.service = service;
        }
        
        public override void Init(object initData)
        {
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

        private async Task RefreshData()
        {
            var loading = UserDialogs.Instance.Loading("Loading data");

            loading.Show();
            IsBusy = true;
            var result = await service.GetAllData();

            if (result != null)
            {
                Items = new ObservableCollection<Transaction>(result);
                var sorted = from record in result
                             orderby record.PeriodSort descending, record.TransDate descending, record.Id descending
                             group record by new { record.PeriodSort, record.Period } into gr
                             select new Grouping(gr.Key.PeriodSort, gr.Key.Period, gr);
                ItemGrouped = new ObservableCollection<Grouping>(sorted);
            }
            else
            {
                Items = new ObservableCollection<Transaction>();
                ItemGrouped = new ObservableCollection<Grouping>();
            }
            IsBusy = false;
            loading.Hide();
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

        private ObservableCollection<Grouping> itemGrouped;
        public ObservableCollection<Grouping> ItemGrouped
        {
            get { return itemGrouped; }
            set
            {
                itemGrouped = value;
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

        public async Task Load()
        {
            await RefreshData();
        }

        public void Add()
        {
            CoreMethods.PushPageModel<ItemPageModel>(0);
        }

        public void Edit()
        {
            CoreMethods.PushPageModel<ItemPageModel>(SelectedItem.Id);
            SelectedItem = null;
        }
    }
}
