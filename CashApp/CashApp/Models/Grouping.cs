using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CashApp.Models
{
    [ImplementPropertyChanged]
    public class Grouping : ObservableCollection<Transaction>
    {
        public string Key { get; private set; }
        public string Period { get; private set; }
        public ObservableCollection<GroupingAmount> Amounts { get; set; }
        public ObservableCollection<Transaction> Transactions
        {
            get
            {
                return new ObservableCollection<Transaction>(Items);
            }
        }

        public Grouping(string key, string period, IEnumerable<Transaction> items)
        {
            Key = key;
            Period = period;
            foreach (var item in items)
            {
                Items.Add(item);
            }
            Amounts = new ObservableCollection<GroupingAmount>(Items.GroupBy(c => c.Currency).Select(x => new GroupingAmount
            {
                Currency = x.Key,
                Amount = x.Sum(a => a.Amount)
            }));
        }
    }
}
