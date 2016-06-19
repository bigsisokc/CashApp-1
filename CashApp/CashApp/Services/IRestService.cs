using CashApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CashApp.Services
{
    public interface IRestService
    {
        Task<List<Transaction>> GetAllData();

        Task<Transaction> GetData(int id);

        Task<bool> SaveItem(Transaction item);

        Task DeleteItem(int id);
    }
}
