using CashApp.Message;
using CashApp.Models;
using CashApp.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CashApp.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        private readonly IRestService service;
        private readonly MvxSubscriptionToken token;

        public MainViewModel(IRestService service, IMvxMessenger messenger)
        {
            this.service = service;
            token = messenger.Subscribe<SaveMessage>(async (message) => await OnSavedMessage(message));
        }

        private async Task OnSavedMessage(SaveMessage saveMessage)
        {
            if (saveMessage.Saved)
            {
                IsBusy = true;
                await RefreshData();
                IsBusy = false;
            }            
        }
        
        public override async void Start()
        {
            base.Start();

            IsBusy = true;
            await RefreshData();
            IsBusy = false;
        }

        private async Task RefreshData()
        {
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
        }

        private ObservableCollection<Transaction> items;
        public ObservableCollection<Transaction> Items
        {
            get { return items; }
            set
            {
                items = value;
                RaisePropertyChanged(() => Items);
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        private Transaction selectedItem;
        public Transaction SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);

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
                RaisePropertyChanged(() => ItemGrouped);
            }
        }

        public ICommand EditItemCommand
        {
            get
            {
                return new MvxCommand(Edit, () => SelectedItem != null);
            }
        }

        public ICommand AddItemCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<ItemViewModel>());
            }
        }

        public MvxCommand LoadItemCommand
        {
            get
            {
                return new MvxCommand(async () => await Load());
            }
        }

        public async Task Load()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            LoadItemCommand.RaiseCanExecuteChanged();

            await RefreshData();

            IsBusy = false;
            LoadItemCommand.RaiseCanExecuteChanged();
        }

        public void Edit()
        {
            ShowViewModel<ItemViewModel>(new { id = SelectedItem.Id });
            SelectedItem = null;
        }
    }
}
