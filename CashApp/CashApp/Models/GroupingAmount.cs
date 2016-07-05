using PropertyChanged;

namespace CashApp.Models
{
    [ImplementPropertyChanged]
    public class GroupingAmount
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
