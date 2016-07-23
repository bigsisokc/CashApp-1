using Microsoft.WindowsAzure.MobileServices.Sync;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Threading.Tasks;

namespace CashApp.Services
{
    public class SyncHandler : IMobileServiceSyncHandler
    {
        public Task<JObject> ExecuteTableOperationAsync(IMobileServiceTableOperation operation)
        {
            Debug.WriteLine("Executing operation '{0}' for table '{1}'", operation.Kind, operation.Table.TableName);
            return operation.ExecuteAsync();
        }

        public Task OnPushCompleteAsync(MobileServicePushCompletionResult result)
        {
            Debug.WriteLine("Push result: {0}", result.Status);
            foreach (var error in result.Errors)
            {
                Debug.WriteLine("Push error: {0}", error.Status);
            }

            return Task.FromResult(0);
        }
    }
}
