using PropertyChanged;

namespace CashApp.Models
{
    [ImplementPropertyChanged]
    public class CurrencyModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
    }
}
