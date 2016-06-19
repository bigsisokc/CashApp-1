using CashApp.Models;
using ModernHttpClient;
using MvvmCross.Platform;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CashApp.Services
{
    public class RestService : IRestService
    {
        public RestService()
        {

        }

        HttpClient baseClient;
        private HttpClient BaseClient
        {
            get
            {
                return baseClient ?? (baseClient = new HttpClient(new NativeMessageHandler()));
            }
        }

        public async Task<List<Transaction>> GetAllData()
        {
            var uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

            try
            {
                var response = await BaseClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json)) return null;

                return JsonConvert.DeserializeObject<List<Transaction>>(json);
            }
            catch (Exception ex)
            {
                Mvx.TaggedTrace(typeof(RestService).Name,
                    "Ooops! Something went wrong fetching information for: GetAllData. Exception: {0}", ex);
                return null;
            }
        }

        public async Task<bool> SaveItem(Transaction item)
        {
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
                if (item.Id == 0)
                {
                    response = await BaseClient.PostAsync(uri, content);
                }
                else
                {
                    response = await BaseClient.PutAsync(uri, content);
                }
                if (response.IsSuccessStatusCode)
                {
                    Mvx.TaggedTrace(typeof(RestService).Name,
                        "Successfully save the information");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Mvx.TaggedTrace(typeof(RestService).Name,
                    "Ooops! Something went wrong saving information for: {0}. Exception: {1}", item.Id, ex);
            }
            return false;
        }

        public async Task DeleteItem(int id)
        {
            var uri = new Uri(string.Format(Constants.RestUrl, id));

            try
            {
                var response = await BaseClient.DeleteAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    Mvx.TaggedTrace(typeof(RestService).Name,
                        "Successfully delete the information");
                }

            }
            catch (Exception ex)
            {
                Mvx.TaggedTrace(typeof(RestService).Name,
                   "Ooops! Something went wrong deleting information for: {0}. Exception: {1}", id, ex);
            }
        }

        public async Task<Transaction> GetData(int id)
        {
            if (id == 0)
            {
                return null;
            }
            var uri = new Uri(string.Format(Constants.RestUrl, id));

            try
            {
                var response = await BaseClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(json)) return null;

                return JsonConvert.DeserializeObject<Transaction>(json);
            }
            catch (Exception ex)
            {
                Mvx.TaggedTrace(typeof(RestService).Name,
                    "Ooops! Something went wrong fetching information for: GetData {0}. Exception: {1}", id, ex);
                return null;
            }
        }
    }
}
