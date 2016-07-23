using Acr.UserDialogs;
using CashApp.Interfaces;
using CashApp.Mixins;
using CashApp.Models;
using FreshMvvm;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CashApp.PageModels
{
    [ImplementPropertyChanged]
    public class ItemPageModel : FreshBasePageModel
    {
        public Transactions Item { get; set; }
        private readonly IDataService service;
        private readonly IUserDialogs userDialog;

        public ItemPageModel(IDataService service, IUserDialogs userDialog)
        {
            this.service = service;
            this.userDialog = userDialog;
            CurrencyList = new List<CurrencyModel>
            {
                new CurrencyModel { Id = 1, Code = "IDR" },
                new CurrencyModel { Id = 2, Code = "SGD" },
            };
            CurrencyIndex = 0;
        }
        
        public override void Init(object initData)
        {
            if (initData != null && initData.ToString() != string.Empty)
            {
                InitializeData(initData.ToString()).RunForget();
            }
            else
            {
                Description = string.Empty;
                Amount = 0;
                TransDate = DateTime.Today;
                Currency = "IDR";
            }
        }

        private async Task InitializeData(string id)
        {
            var loading = userDialog.Loading("Loading transaction", show: false);
            loading.Show();
            IsBusy = true;
            Item = await service.GetTransaction(id);
            if (Item != null)
            {
                id = Item.Id;
                Description = Item.Description;
                TransDate = Item.TransDate;
                Amount = Item.Amount;
                Currency = Item.Currency;
                Username = Item.Username;
                Category = Item.Category;
            }
            IsBusy = false;
            loading.Hide();
        }

        private IList<CurrencyModel> currencyList;
        public IList<CurrencyModel> CurrencyList
        {
            get { return currencyList; }
            set
            {
                currencyList = value;
            }
        }

        private string category;
        public string Category
        {
            get { return category; }
            set
            {
                category = value;
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
            }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
            }
        }

        private int currencyIndex;
        public int CurrencyIndex
        {
            get { return currencyIndex; }
            set
            {
                currencyIndex = value;
            }
        }

        public string Currency
        {
            get
            {
                return CurrencyIndex == 1 ? "SGD" : "IDR";
            }
            set
            {
                if (value == "SGD")
                {
                    CurrencyIndex = 1;
                }
                else
                {
                    CurrencyIndex = 0;
                }
            }
        }

        private DateTime transDate;
        public DateTime TransDate
        {
            get { return transDate; }
            set
            {
                transDate = value;
            }
        }

        private decimal amount;
        public decimal Amount
        {
            get { return amount; }
            set
            {
                if (amount == value)
                {
                    return;
                }
                amount = value;
            }
        }

        public bool CanSaveItem()
        {
            return !string.IsNullOrEmpty(Description) && Amount > 0;
        }

        public async Task SaveItem()
        {
            var loading = userDialog.Loading("Saving transaction");

            loading.Show();
            IsBusy = true;
            if (Item == null)
            {
                await service.AddTransaction(new Transactions
                {
                    Amount = Amount,
                    Currency = Currency,
                    Description = Description,
                    Id = Guid.NewGuid().ToString(),
                    TransDate = TransDate,
                    Category = Category
                });
            }
            else
            {
                await service.UpdateTransaction(Item);
            }
            IsBusy = false;
            loading.Hide();

            await CoreMethods.PopPageModel();
        }

        public async Task DeleteItem()
        {
            if (Item != null)
            {
                var confirmDelete = await userDialog.ConfirmAsync("Are you sure you want to delete this transaction?");

                if (confirmDelete)
                {
                    var loading = userDialog.Loading("Deleting transaction");
                    loading.Show();
                    await service.DeleteTransaction(Item);
                    loading.Hide();
                }
                else
                {
                    return;
                }
            }

            await CoreMethods.PopPageModel(data: true);
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; }
        }

        public ICommand SaveItemCommand
        {
            get
            {
                return new Command(async () => await SaveItem(), CanSaveItem);
            }
        }
        public ICommand DeleteItemCommand
        {
            get
            {
                return new Command(async () => await DeleteItem(), () => Item != null);
            }
        }

    }
}
