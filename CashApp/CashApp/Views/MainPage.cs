using System;
using Xamarin.Forms;

namespace CashApp.Views
{
    public class MainPage : MasterDetailPage
    {
        MenuPage menuPage;

        public MainPage()
        {
            menuPage = new MenuPage();

            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as CashApp.Models.MenuItem);

            Master = menuPage;
            Detail = new NavigationPage(new TransactionPage());
        }

        void NavigateTo(CashApp.Models.MenuItem menu)
        {
            if (menu == null)
                return;

            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

            Detail = new NavigationPage(displayPage);

            menuPage.Menu.SelectedItem = null;
            IsPresented = false;
        }
    }
}
