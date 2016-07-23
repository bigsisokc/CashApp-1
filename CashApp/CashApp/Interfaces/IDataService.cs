using CashApp.Models;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashApp.Interfaces
{
    public interface IDataService
    {
        MobileServiceClient MobileService { get; set; }
        Task Initialize();
        Task<IEnumerable<Transactions>> GetTransactions();
        Task<Transactions> GetTransaction(string id);
        Task AddTransaction(Transactions trans);
        Task UpdateTransaction(Transactions trans);
        Task DeleteTransaction(Transactions trans);
        Task SyncTransaction();
        Task Purge();
        Task Logout();
    }
}
