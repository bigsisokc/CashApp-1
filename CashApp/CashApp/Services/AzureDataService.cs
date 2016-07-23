using Acr.UserDialogs;
using CashApp.Interfaces;
using CashApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CashApp.Services
{
    public class AzureDataService : IDataService
    {
        public MobileServiceClient MobileService { get; set; }
        IMobileServiceSyncTable<Transactions> transactionTable;
        IUserDialogs userDialog;

        public AzureDataService(IUserDialogs userDialog)
        {
            this.userDialog = userDialog;
        }

        public async Task Initialize()
        {
            var handler = new AuthHandler(userDialog);
            //Create our client
            MobileService = new MobileServiceClient("https://mycashapi.azurewebsites.net", handler);
            handler.Client = MobileService;

            if (!string.IsNullOrWhiteSpace(CashApp.Helpers.Settings.AuthToken) && !string.IsNullOrWhiteSpace(CashApp.Helpers.Settings.UserId))
            {
                MobileService.CurrentUser = new MobileServiceUser(CashApp.Helpers.Settings.UserId);
                MobileService.CurrentUser.MobileServiceAuthenticationToken = CashApp.Helpers.Settings.AuthToken;
            }

            await GetUserData();

            const string path = "syncstore.db";
            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Transactions>();
            await MobileService.SyncContext.InitializeAsync(store, new SyncHandler());

            //Get our sync table that will call out to azure
            transactionTable = MobileService.GetSyncTable<Transactions>();
        }

        public async Task Purge()
        {
            await transactionTable.PurgeAsync(true);
        }

        public async Task Logout()
        {
            await MobileService.LogoutAsync();
            CashApp.Helpers.Settings.UserId = string.Empty;
            CashApp.Helpers.Settings.AuthToken = string.Empty;
        }

        public async Task<IEnumerable<Transactions>> GetTransactions()
        {
            await Initialize();
            await SyncTransaction();
            return await transactionTable.OrderBy(c => c.TransDate).ToEnumerableAsync();
        }

        public async Task<Transactions> GetTransaction(string id)
        {
            var list = await transactionTable.Where(x => x.Id == id).ToEnumerableAsync();
            return list.FirstOrDefault();
        }

        public async Task AddTransaction(Transactions trans)
        {
            await Initialize();
            
            await transactionTable.InsertAsync(trans);

            await SyncTransaction();
        }

        public async Task UpdateTransaction(Transactions trans)
        {
            await Initialize();

            await transactionTable.UpdateAsync(trans);

            await SyncTransaction();
        }

        public async Task DeleteTransaction(Transactions trans)
        {
            await Initialize();
            await transactionTable.DeleteAsync(trans);
            await SyncTransaction();
        }

        public async Task SyncTransaction()
        {
            try
            {
                await transactionTable.PullAsync("allTransactions", transactionTable.CreateQuery());
                await MobileService.SyncContext.PushAsync();
            }
            catch (Exception ex)
            {
                await userDialog.AlertAsync(string.Format("Failed synchronising. Error : {0}", ex.Message));
            }
        }

        private async Task GetUserData()
        {
            try
            {
                var result = await MobileService.InvokeApiAsync("identities", HttpMethod.Get, null);
            }
            catch (Exception ex)
            {
                await userDialog.AlertAsync(string.Format("Failed retrieving user info. Error : {0}", ex.Message));
            }
        }
    }
}
