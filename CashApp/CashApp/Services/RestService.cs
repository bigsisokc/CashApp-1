using CashApp.Models;
using ModernHttpClient;
using MvvmCross.Platform;
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

        private HttpClient GetClient()
        {
            var client = new HttpClient(new NativeMessageHandler());
            client.Timeout = TimeSpan.FromSeconds(10);
            return client;
        }

        private void StartDiagnostic()
        {
            sw.Reset();
            sw.Start();
        }

        private void StopDiagnostic(string method)
        {
            sw.Stop();
            Mvx.TaggedTrace(typeof(RestService).Name, "Run {1} for {0}ms", sw.ElapsedMilliseconds, method);
        }
        
        public async Task<List<Transaction>> GetAllData()
        {
            StartDiagnostic();
            List<Transaction> result = null;
            var uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

            try
            {
                using (var client = GetClient())
                {
                    var response = await client.GetAsync(uri);
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(json))
                        {
                            result = null;
                        }

                        result = JsonConvert.DeserializeObject<List<Transaction>>(json);
                    }
                    else
                    {
                        result = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Mvx.TaggedTrace(typeof(RestService).Name,
                    "Ooops! Something went wrong fetching information for: GetAllData. Exception: {0}", ex);
                result = null;
            }
            StopDiagnostic(nameof(GetAllData));
            return result;
        }

        public async Task<bool> SaveItem(Transaction item)
        {
            StartDiagnostic();
            bool result = false;
            Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));
            if (item.Id > 0)
            {
                uri = new Uri(string.Format(Constants.RestUrl, item.Id));
            }

            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = null;
                using (var client = GetClient())
                {
                    if (item.Id == 0)
                    {
                        response = await client.PostAsync(uri, content);
                    }
                    else
                    {
                        response = await client.PutAsync(uri, content);
                    }
                    if (response.IsSuccessStatusCode)
                    {
                        Mvx.TaggedTrace(typeof(RestService).Name,
                            "Successfully save the information");
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Mvx.TaggedTrace(typeof(RestService).Name,
                    "Ooops! Something went wrong saving information for: {0}. Exception: {1}", item.Id, ex);
            }
            StopDiagnostic(nameof(SaveItem));
            return result;
        }

        public async Task DeleteItem(int id)
        {
            StartDiagnostic();
            var uri = new Uri(string.Format(Constants.RestUrl, id));

            try
            {
                using (var client = GetClient())
                {
                    var response = await client.DeleteAsync(uri);

                    if (response.IsSuccessStatusCode)
                    {
                        Mvx.TaggedTrace(typeof(RestService).Name,
                            "Successfully delete the information");
                    }
                }
            }
            catch (Exception ex)
            {
                Mvx.TaggedTrace(typeof(RestService).Name,
                   "Ooops! Something went wrong deleting information for: {0}. Exception: {1}", id, ex);
            }
            StopDiagnostic(nameof(DeleteItem));
        }

        public async Task<Transaction> GetData(int id)
        {
            StartDiagnostic();
            Transaction result = null;
            if (id > 0)
            {

                var uri = new Uri(string.Format(Constants.RestUrl, id));

                try
                {
                    using (var client = GetClient())
                    {
                        var response = await client.GetAsync(uri);
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
                }
                catch (Exception ex)
                {
                    Mvx.TaggedTrace(typeof(RestService).Name,
                        "Ooops! Something went wrong fetching information for: GetData {0}. Exception: {1}", id, ex);
                    result = null;
                }
            }
            StopDiagnostic(nameof(GetData));
            return result;
        }
    }
}
