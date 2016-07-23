using Xamarin.Forms;

namespace CashApp.Pages
{
    public class BaseContentPage : ContentPage
    {
        protected override void OnAppearing()
        {
            base.OnAppearing();

            //if (!App.Instance.IsAuthenticated)
            //{
            //    Navigation.PushModalAsync(new LoginPage());
            //}
        }
    }
}
