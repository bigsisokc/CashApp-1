using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CashApp.Models
{
    public class Grouping<K, KD, A, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }
        public KD KeyDescription { get; set; }
        public A KeyAmount { get; set; }

        public Grouping(K key, KD keyDescription, A amount, IEnumerable<T> items)
        {
            Key = key;
            KeyDescription = keyDescription;
            KeyAmount = amount;
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
