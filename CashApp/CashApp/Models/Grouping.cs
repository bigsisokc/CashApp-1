﻿using PropertyChanged;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CashApp.Models
{
    [ImplementPropertyChanged]
    public class Grouping : ObservableCollection<Transactions>
    {
        public int Year { get; private set; }
        public int Month { get; private set; }
        public string Period { get; private set; }
        public ObservableCollection<GroupingAmount> Amounts { get; set; }
        public ObservableCollection<Transactions> Transactions
        {
            get
            {
                return new ObservableCollection<Transactions>(Items);
            }
        }

        public Grouping()
        {
            Amounts = new ObservableCollection<GroupingAmount>();
        }

        public Grouping(int year, int month, string period, IEnumerable<Transactions> items)
        {
            Year = year;
            Month = month;
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
