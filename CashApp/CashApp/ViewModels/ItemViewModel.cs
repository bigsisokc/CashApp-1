using CashApp.Message;
using CashApp.Models;
using CashApp.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashApp.ViewModels
{
    public class ItemViewModel : MvxViewModel
    {
        int id;
        Transaction item;
        private readonly IRestService service;
        private readonly IMvxMessenger messenger;

        public ItemViewModel(IRestService service, IMvxMessenger messenger)
        {
            this.service = service;
            this.messenger = messenger;
            CurrencyList = new List<CurrencyModel>
            {
                new CurrencyModel { Id = 1, Code = "IDR" },
                new CurrencyModel { Id = 2, Code = "SGD" },
            };
            CurrencyIndex = 0;
        }

        public void Init(int id)
        {
            this.id = id;
        }

        public override async void Start()
        {
            base.Start();

            if (item != null) return;

            IsBusy = true;
            item = await service.GetData(id);
            if (item != null)
            {
                Description = item.Description;
                TransDate = item.TransDate;
                Amount = item.Amount;
                Currency = item.Currency;
            }
            else
            {
                TransDate = DateTime.Today;
                Currency = "IDR";
            }
            IsBusy = false;
        }

        private IList<CurrencyModel> currencyList;
        public IList<CurrencyModel> CurrencyList
        {
            get { return currencyList; }
            set
            {
                currencyList = value;
                RaisePropertyChanged(() => CurrencyList);
            }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                RaisePropertyChanged(() => Description);
            }
        }

        private int currencyIndex;
        public int CurrencyIndex
        {
            get { return currencyIndex; }
            set
            {
                currencyIndex = value;
                RaisePropertyChanged(() => CurrencyIndex);
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
                RaisePropertyChanged(() => TransDate);
            }
        }

        private decimal amount;
        public decimal Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                RaisePropertyChanged(() => Amount);
            }
        }

        public bool CanSaveItem()
        {
            return !string.IsNullOrEmpty(Description) && Amount > 0;
        }

        public async Task SaveItem()
        {
            Close(this);

            bool result = await service.SaveItem(new Transaction
            {
                Amount = Amount,
                Currency = Currency,
                Description = Description,
                Id = id,
                TransDate = TransDate
            });

            var savedMessage = new SaveMessage(this, result);
            messenger.Publish(savedMessage);
        }

        public async Task DeleteItem()
        {
            Close(this);

            if (id > 0)
            {
                await service.DeleteItem(id);
                var savedMessage = new SaveMessage(this, true);
                messenger.Publish(savedMessage);
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; RaisePropertyChanged(() => IsBusy); }
        }

        public MvxCommand SaveItemCommand
        {
            get
            {
                return new MvxCommand(async () => await SaveItem());
            }
        }
        public MvxCommand DeleteItemCommand
        {
            get
            {
                return new MvxCommand(async () => await DeleteItem(), () => id > 0);
            }
        }

    }
}
