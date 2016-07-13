using Acr.UserDialogs;
using CashApp.Containers;
using CashApp.Models;
using CashApp.PageModels;
using CashApp.Services;
using FreshMvvm;
using ModernHttpClient;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CashApp
{
    public class App : Application
    {
        private App()
        {
            FreshIOC.Container.Register<IRestService, RestService>();
            FreshIOC.Container.Register<IUserDialogs>(UserDialogs.Instance);
            
            MainPage = GetMainPage();
        }

        // just a singleton pattern so I can have the concept of an app instance
        static volatile App _Instance;
        static object _SyncRoot = new Object();
        public static App Instance
        {
            get
            {
                if (_Instance == null)
                {
                    lock (_SyncRoot)
                    {
                        if (_Instance == null)
                        {
                            _Instance = new App();
                            _Instance.OAuthSettings =
                                new OAuthSettings(
                                    clientId: "1026078904285-ig00qql8v78d38p1l2scs4kb7oqer0oo.apps.googleusercontent.com",       // your OAuth2 client id 
                                    scope: "https://www.googleapis.com/auth/userinfo.email",  		// The scopes for the particular API you're accessing. The format for this will vary by API.
                                    authorizeUrl: "https://accounts.google.com/o/oauth2/auth",  	// the auth URL for the service
                                    redirectUrl: "https://www.googleapis.com/plus/v1/people/me");   // the redirect URL for the service

                            // If you'd like to know more about how to integrate with an OAuth provider, 
                            // I personally like the Instagram API docs: http://instagram.com/developer/authentication/
                        }
                    }
                }

                return _Instance;
            }
        }

        public OAuthSettings OAuthSettings { get; private set; }

        Page _NavPage;

        public Page GetMainPage()
        {
            var masterDetailNav = new FreshMasterDetailNavigationContainer();
            masterDetailNav.Init("Menu", "Menu.png");
            masterDetailNav.AddPage<PeriodPageModel>("Transactions", null);
            masterDetailNav.AddPage<AboutPageModel>("About", null);
            _NavPage = masterDetailNav;
            
            return _NavPage;
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrWhiteSpace(_Token); }
        }

        string _Token;
        public string Token
        {
            get { return _Token; }
        }

        public string Picture
        {
            get
            {
                string result = string.Empty;
                if (GoogleInfo != null)
                {
                    result = GoogleInfo.picture;
                }
                return result;
            }
        }

        public string Name
        {
            get
            {
                string result = string.Empty;
                if (GoogleInfo != null)
                {
                    result = GoogleInfo.name;
                }
                return result;
            }
        }

        public string Email
        {
            get
            {
                string result = string.Empty;
                if (GoogleInfo != null)
                {
                    result = GoogleInfo.email;
                }
                return result;
            }
        }

        GoogleInfo _googleInfo;
        public GoogleInfo GoogleInfo
        {
            get { return _googleInfo; }
        }

        public async Task SaveToken(string token)
        {
            _Token = token;

            await GetProfileInfoFromGoogle(token);

            // broadcast a message that authentication was successful
            MessagingCenter.Send(this, "Authenticated");
        }

        public Action SuccessfulLoginAction
        {
            get
            {
                return new Action(() => _NavPage.Navigation.PopModalAsync());
            }
        }

        const string googUesrInfoAccessleUrl = "https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}";
        async Task<bool> GetProfileInfoFromGoogle(string access_token)
        {
            var progress = UserDialogs.Instance.Loading("Please wait...");
            bool isValid = false;
            //Google API REST request
            string userInfo = await DownloadString(string.Format(googUesrInfoAccessleUrl, access_token));
            if (userInfo != "Exception")
            {
                //step 4: Deserialize the JSON response to get data in class object
                _googleInfo = JsonConvert.DeserializeObject<GoogleInfo>(userInfo);
                isValid = true;
            }
            else
            {
                isValid = false;
                await UserDialogs.Instance.AlertAsync("Failed retrieving user information");
            }
            progress.Hide();
            return isValid;
        }

        async Task<string> DownloadString(string strUri)
        {
            var client = new HttpClient(new NativeMessageHandler());
            string strResultData;
            try
            {
                strResultData = await client.GetStringAsync(new Uri(strUri));
            }
            catch
            {
                strResultData = "Exception";
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                    client = null;
                }
            }
            return strResultData;
        }
    }
}
