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

            //MainPage = new NavigationPage(new TransactionPage());
            var page = FreshPageModelResolver.ResolvePageModel<TransactionPageModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;
        }
        
    }
}
