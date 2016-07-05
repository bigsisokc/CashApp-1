using PropertyChanged;
using System.Collections.Generic;

namespace CashApp.Models
{
    [ImplementPropertyChanged]
    public class GroupingHeader
    {
        public string Period { get; set; }
        public IList<GroupingAmount> Amounts { get; set; }
    }
}
