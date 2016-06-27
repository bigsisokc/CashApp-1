using System.Collections.Generic;

namespace CashApp.Models
{
    public class GroupingHeader
    {
        public string Period { get; set; }
        public IList<GroupingAmount> Amounts { get; set; }
    }
}
