using CashApp.Models;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CashApp.Services
{
    public class RestService : IRestService
    {
        Stopwatch sw = new Stopwatch();

        public RestService()
        {

        }

        private HttpClient client;
        public HttpClient Client
        {
            get
            {
                if (client == null)
                {
                    var httpClient = new HttpClient(new NativeMessageHandler());
                    httpClient.Timeout = TimeSpan.FromSeconds(10);
                    client = httpClient;
                }
                return client;
            }
        }

        private void StartDiagnostic()
        {
            sw.Reset();
            sw.Start();
        }

        private void StopDiagnostic(string method)
        {
            sw.Stop();
            Debug.WriteLine("Run {1} for {0}ms", sw.ElapsedMilliseconds, method);
        }

        public async Task<List<Transaction>> GetPeriodData(int year, int month)
        {
            StartDiagnostic();
            var uri = new Uri(string.Format(Constants.TransactionRestUrl, string.Format("period/{0}/{1}", year, month)));
            List<Transaction> result = await GetData<List<Transaction>>(uri);
            StopDiagnostic(nameof(GetPeriodData));
            return result;
        }

        public async Task<List<TransactionWithPeriod>> GetPeriodData()
        {
            StartDiagnostic();
            var uri = new Uri(string.Format(Constants.TransactionRestUrl, "period"));
            List<TransactionWithPeriod> result = await GetData<List<TransactionWithPeriod>>(uri);
            foreach (var res in result)
            {
                res.TransDate = new DateTime(res.Year, res.Month, 1);
            }
            StopDiagnostic(nameof(GetPeriodData));
            return result;
        }

        public async Task<List<Transaction>> GetAllData()
        {
            StartDiagnostic();
            var uri = new Uri(string.Format(Constants.TransactionRestUrl, string.Empty));
            List<Transaction> result = await GetData<List<Transaction>>(uri);
            StopDiagnostic(nameof(GetAllData));
            return result;
        }

        private async Task<TData> GetData<TData>(Uri uri)
        {
            TData result = default(TData);

            try
            {
                var response = await Client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(json))
                    {
                        result = JsonConvert.DeserializeObject<TData>(json);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ooops! Something went wrong fetching information for: GetAllData. Exception: {0}", ex);
            }
            return result;
        }

        public async Task<bool> SaveItem(Transaction item)
        {
            StartDiagnostic();
            bool result = false;
            Uri uri = new Uri(string.Format(Constants.TransactionRestUrl, string.Empty));
            if (item.Id > 0)
            {
                uri = new Uri(string.Format(Constants.TransactionRestUrl, item.Id));
            }

            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;

                if (item.Id == 0)
                {
                    response = await Client.PostAsync(uri, content);
                }
                else
                {
                    response = await Client.PutAsync(uri, content);
                }
                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully save the information");
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ooops! Something went wrong saving information for: {0}. Exception: {1}", item.Id, ex);
            }
            StopDiagnostic(nameof(SaveItem));
            return result;
        }

        public async Task DeleteItem(int id)
        {
            StartDiagnostic();
            var uri = new Uri(string.Format(Constants.TransactionRestUrl, id));

            try
            {
                var response = await Client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Successfully delete the information");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ooops! Something went wrong deleting information for: {0}. Exception: {1}", id, ex);
            }
            StopDiagnostic(nameof(DeleteItem));
        }

        public async Task<Transaction> GetData(int id)
        {
            StartDiagnostic();
            Transaction result = null;
            if (id > 0)
            {

                var uri = new Uri(string.Format(Constants.TransactionRestUrl, id));

                try
                {
                    var response = await Client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(json))
                        {
                            result = null;
                        }

                        result = JsonConvert.DeserializeObject<Transaction>(json);
                    }
                    else
                    {
                        result = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ooops! Something went wrong fetching information for: GetData {0}. Exception: {1}", id, ex);
                    result = null;
                }
            }
            StopDiagnostic(nameof(GetData));
            return result;
        }
    }
}
