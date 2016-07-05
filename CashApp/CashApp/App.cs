using CashApp.Services;
using CashApp.Views;
using FreshMvvm;
using Xamarin.Forms;

namespace CashApp
{
    public class App : Application
    {
        public App()
        {
            FreshIOC.Container.Register<IRestService, RestService>();

            MainPage = new NavigationPage(new TransactionPage());
        }
        
    }
}
