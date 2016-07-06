using CashApp.PageModels;
using CashApp.Services;
using FreshMvvm;
using Xamarin.Forms;

namespace CashApp
{
    public class App : Application
    {
        public App()
        {
            FreshIOC.Container.Register<IRestService, RestService>();
            
            var masterDetailNav = new FreshMasterDetailNavigationContainer();
            masterDetailNav.Init("Menu", "Menu.png");
            masterDetailNav.AddPage<PeriodPageModel>("Transactions", null);
            masterDetailNav.AddPage<AboutPageModel>("About", null);
            MainPage = masterDetailNav;
        }
        
    }
}
