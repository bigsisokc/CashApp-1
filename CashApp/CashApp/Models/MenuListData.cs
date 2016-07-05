using CashApp.Views;
using System.Collections.Generic;

namespace CashApp.Models
{
    public class MenuListData : List<CashApp.Models.MenuItem>
    {
        public MenuListData()
        {
            this.Add(new CashApp.Models.MenuItem()
            {
                Title = "Transactions",
                IconSource = "transaction.png",
                TargetType = typeof(TransactionPage)
            });

            this.Add(new CashApp.Models.MenuItem()
            {
                Title = "About",
                IconSource = "info.png",
                TargetType = typeof(AboutPage)
            });
        }
    }
}
