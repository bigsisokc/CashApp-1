using CashApp.Droid;
using CashApp.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(LoginPage), typeof(LoginPageRenderer))]
namespace CashApp.Droid
{
    public class LoginPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            // this is a ViewGroup - so should be able to load an AXML file and FindView<>
            //var activity = this.Context as Activity;

            //var auth = new OAuth2Authenticator(
            //    clientId: App.Instance.OAuthSettings.ClientId, // your OAuth2 client id
            //    scope: App.Instance.OAuthSettings.Scope, // The scopes for the particular API you're accessing. The format for this will vary by API.
            //    authorizeUrl: new Uri(App.Instance.OAuthSettings.AuthorizeUrl), // the auth URL for the service
            //    redirectUrl: new Uri(App.Instance.OAuthSettings.RedirectUrl)); // the redirect URL for the service

            //auth.Completed += async (sender, eventArgs) => {
            //    if (eventArgs.IsAuthenticated)
            //    {
            //        string access_token;
            //        eventArgs.Account.Properties.TryGetValue("access_token", out access_token);
            //        await App.Instance.SaveToken(access_token);
            //        AccountStore.Create(this.Context).Save(eventArgs.Account, "Google");
            //        App.Instance.SuccessfulLoginAction.Invoke();
            //    }
            //    else
            //    {
            //        // The user cancelled
            //        Toast.MakeText(this.Context, "Fail to authenticate!", ToastLength.Short).Show();
            //    }
            //};

            //activity.StartActivity(auth.GetUI(activity));
        }

    }
}