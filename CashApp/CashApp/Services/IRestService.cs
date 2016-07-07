using CashApp.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CashApp.Services
{
    public interface IRestService
    {
        Task<List<TransactionWithPeriod>> GetPeriodData(CancellationTokenSource cts);

        Task<List<Transaction>> GetPeriodData(int year, int month, CancellationTokenSource cts);

        Task<List<Transaction>> GetAllData(CancellationTokenSource cts);

        Task<Transaction> GetData(int id, CancellationTokenSource cts);

        Task<bool> SaveItem(Transaction item, CancellationTokenSource cts);

        Task DeleteItem(int id, CancellationTokenSource cts);
    }
}
