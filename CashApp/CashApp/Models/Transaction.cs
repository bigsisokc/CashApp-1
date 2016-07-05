using PropertyChanged;
using System;

namespace CashApp.Models
{
    [ImplementPropertyChanged]
    public class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }

        DateTime transDate;
        public DateTime TransDate
        {
            get
            {
                return transDate;
            }
            set
            {
                transDate = new DateTime(value.Ticks, DateTimeKind.Utc);
                Period = string.Format("{0:MMM yyyy}", value);
                PeriodSort = string.Format("{0:yyyyMM}", value);
            }
        }
        public string Period { get; set; }
        public string PeriodSort { get; set; }
        public decimal Amount { get; set; }
    }
}
