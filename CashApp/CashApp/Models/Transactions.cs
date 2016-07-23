using Newtonsoft.Json;
using System;

namespace CashApp.Models
{
    public class Transactions
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public DateTime TransDate { get; set; }
        [JsonIgnore]
        public string transdateDisplay { get { return TransDate.ToLocalTime().ToString("d"); } }
        [JsonIgnore]
        public string Period { get { return string.Format("{0:MMM yyyy}", TransDate); } }
        [JsonIgnore]
        public string PeriodSort { get { return string.Format("{0:yyyyMM}", TransDate); } }
        [JsonIgnore]
        public int Year { get { return TransDate.Year; } }
        [JsonIgnore]
        public int Month { get { return TransDate.Month; } }
        public decimal Amount { get; set; }
        public string Username { get; set; }
        public string Category { get; set; }
        [Microsoft.WindowsAzure.MobileServices.Version]
        public string Version { get; set; }
    }
}
