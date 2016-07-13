﻿using Acr.UserDialogs;
using CashApp.Mixins;
using CashApp.Models;
using CashApp.Services;
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
        int id;
        Transaction item;
        private readonly IRestService service;
        private readonly IUserDialogs userDialog;

        public ItemPageModel(IRestService service, IUserDialogs userDialog)
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
            if (initData != null)
            {
                int.TryParse(initData.ToString(), out id);
            }
            if (item != null) return;

            Description = string.Empty;
            Amount = 0;
            TransDate = DateTime.Today;
            Currency = "IDR";

            if (id > 0)
            {
                InitializeData(id).RunForget();
            }
        }

        private async Task InitializeData(int id)
        {
            var loading = userDialog.Loading("Loading transaction", show: false);
            loading.Show();
            IsBusy = true;
            item = await service.GetData(id, null);
            if (item != null)
            {
                Description = item.Description;
                TransDate = item.TransDate;
                Amount = item.Amount;
                Currency = item.Currency;
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

        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
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
            bool result = await service.SaveItem(new Transaction
            {
                Amount = Amount,
                Currency = Currency,
                Description = Description,
                Id = id,
                TransDate = TransDate
            }, null);
            IsBusy = false;
            loading.Hide();

            await CoreMethods.PopPageModel(data: result);
        }

        public async Task DeleteItem()
        {
            bool canDelete = id > 0;

            if (canDelete)
            {
                var confirmDelete = await userDialog.ConfirmAsync("Are you sure you want to delete this transaction?");

                if (confirmDelete)
                {
                    var loading = userDialog.Loading("Deleting transaction");
                    loading.Show();
                    await service.DeleteItem(id, null);
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
                return new Command(async () => await DeleteItem(), () => id > 0);
            }
        }

    }
}
