using Acr.UserDialogs;
using CashApp.Models;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
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
                    httpClient.Timeout = TimeSpan.FromSeconds(20);
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

        public async Task<List<Transaction>> GetPeriodData(int year, int month, CancellationTokenSource cts)
        {
            StartDiagnostic();
            var uri = new Uri(string.Format(Constants.TransactionRestUrl, string.Format("period/{0}/{1}", year, month)));
            List<Transaction> result = await GetData<List<Transaction>>(uri, cts);
            StopDiagnostic(nameof(GetPeriodData));
            return result;
        }

        public async Task<List<TransactionWithPeriod>> GetPeriodData(CancellationTokenSource cts)
        {
            StartDiagnostic();
            var uri = new Uri(string.Format(Constants.TransactionRestUrl, "period"));
            List<TransactionWithPeriod> result = await GetData<List<TransactionWithPeriod>>(uri, cts);
            if (result != null)
            {
                foreach (var res in result)
                {
                    res.TransDate = new DateTime(res.Year, res.Month, 1);
                }
            }
            StopDiagnostic(nameof(GetPeriodData));
            return result;
        }

        public async Task<List<Transaction>> GetAllData(CancellationTokenSource cts)
        {
            StartDiagnostic();
            var uri = new Uri(string.Format(Constants.TransactionRestUrl, string.Empty));
            List<Transaction> result = await GetData<List<Transaction>>(uri, cts);
            StopDiagnostic(nameof(GetAllData));
            return result;
        }

        private async Task<TData> GetData<TData>(Uri uri, CancellationTokenSource cts)
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
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    UserDialogs.Instance.ErrorToast("Cannot reach My Cash Service");
                    Debug.WriteLine("Cannot reach the services");
                }
            }
            catch (WebException ex)
            {
                UserDialogs.Instance.ErrorToast(string.Format("Request Failed : {0}", ex.Message));
                Debug.WriteLine("Request Failed, Ex : {0}", ex);
            }
            catch (TaskCanceledException ex)
            {
                if (cts != null && ex.CancellationToken == cts.Token)
                {
                    Debug.WriteLine("Request canceled");
                }
                else
                {
                    UserDialogs.Instance.ErrorToast("Request timed out");
                    Debug.WriteLine("Request Timeout, Ex : {0}", ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ooops! Something went wrong fetching information for: GetAllData. Exception: {0}", ex);
            }
            return result;
        }

        public async Task<bool> SaveItem(Transaction item, CancellationTokenSource cts)
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
                    UserDialogs.Instance.SuccessToast("Transaction saved");
                    Debug.WriteLine("Successfully save the information");
                    result = true;
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    UserDialogs.Instance.ErrorToast("Cannot reach My Cash Service");
                    Debug.WriteLine("Cannot reach the services");
                }
            }
            catch (WebException ex)
            {
                UserDialogs.Instance.ErrorToast(string.Format("Request Failed : {0}", ex.Message));
                Debug.WriteLine("Request Failed, Ex : {0}", ex);
            }
            catch (TaskCanceledException ex)
            {
                if (cts != null && ex.CancellationToken == cts.Token)
                {
                    Debug.WriteLine("Request canceled");
                }
                else
                {
                    UserDialogs.Instance.ErrorToast("Request timed out");
                    Debug.WriteLine("Request Timeout, Ex : {0}", ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ooops! Something went wrong saving information for: {0}. Exception: {1}", item.Id, ex);
            }
            StopDiagnostic(nameof(SaveItem));
            return result;
        }

        public async Task DeleteItem(int id, CancellationTokenSource cts)
        {
            StartDiagnostic();
            var uri = new Uri(string.Format(Constants.TransactionRestUrl, id));

            try
            {
                var response = await Client.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    UserDialogs.Instance.SuccessToast("Transaction deleted");
                    Debug.WriteLine("Successfully delete the information");
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    UserDialogs.Instance.ErrorToast("Cannot reach My Cash Service");
                    Debug.WriteLine("Cannot reach the services");
                }
            }
            catch (WebException ex)
            {
                UserDialogs.Instance.ErrorToast(string.Format("Request Failed : {0}", ex.Message));
                Debug.WriteLine("Request Failed, Ex : {0}", ex);
            }
            catch (TaskCanceledException ex)
            {
                if (cts != null && ex.CancellationToken == cts.Token)
                {
                    Debug.WriteLine("Request canceled");
                }
                else
                {
                    UserDialogs.Instance.ErrorToast("Request timed out");
                    Debug.WriteLine("Request Timeout, Ex : {0}", ex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Ooops! Something went wrong deleting information for: {0}. Exception: {1}", id, ex);
            }
            StopDiagnostic(nameof(DeleteItem));
        }

        public async Task<Transaction> GetData(int id, CancellationTokenSource cts)
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
                        if (!string.IsNullOrEmpty(json))
                        {
                            result = JsonConvert.DeserializeObject<Transaction>(json);
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        UserDialogs.Instance.ErrorToast("Cannot reach My Cash Service");
                        Debug.WriteLine("Cannot reach the services");
                    }
                }
                catch (WebException ex)
                {
                    UserDialogs.Instance.ErrorToast(string.Format("Request Failed : {0}", ex.Message));
                    Debug.WriteLine("Request Failed, Ex : {0}", ex);
                }
                catch (TaskCanceledException ex)
                {
                    if (cts != null && ex.CancellationToken == cts.Token)
                    {
                        Debug.WriteLine("Request canceled");
                    }
                    else
                    {
                        UserDialogs.Instance.ErrorToast("Request timed out");
                        Debug.WriteLine("Request Timeout, Ex : {0}", ex);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Ooops! Something went wrong fetching information for: GetData {0}. Exception: {1}", id, ex);
                }
            }
            StopDiagnostic(nameof(GetData));
            return result;
        }
    }
}
