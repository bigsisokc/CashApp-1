using CashApp.Droid.Helpers;
using CashApp.Interfaces;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(Authentication))]
namespace CashApp.Droid.Helpers
{
    public class Authentication : IAuthentication
    {
        public async Task<MobileServiceUser> LoginAsync(IMobileServiceClient client, MobileServiceAuthenticationProvider provider)
        {
            try
            {

                CashApp.Helpers.Settings.LoginAttempts++;
                var user = await client.LoginAsync(Forms.Context, provider);
                CashApp.Helpers.Settings.AuthToken = user?.MobileServiceAuthenticationToken ?? string.Empty;
                CashApp.Helpers.Settings.UserId = user?.UserId ?? string.Empty;
                return user;
            }
            catch //(Exception e)
            {
                //e.Data["method"] = "LoginAsync";
                //Xamarin.Insights.Report(e);
            }

            return null;
        }

        public void ClearCookies()
        {
            try
            {
                if ((int)global::Android.OS.Build.VERSION.SdkInt >= 21)
                    global::Android.Webkit.CookieManager.Instance.RemoveAllCookies(null);
            }
            catch //(Exception ex)
            {
            }
        }
    }
}