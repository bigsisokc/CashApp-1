using Acr.UserDialogs;
using CashApp.Containers;
using CashApp.Interfaces;
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
            FreshIOC.Container.Register<IDataService, AzureDataService>();
            FreshIOC.Container.Register<IUserDialogs>(UserDialogs.Instance);
            
            MainPage = GetMainPage();
        }
        
        Page _NavPage;

        public Page GetMainPage()
        {
            var masterDetailNav = new CustomMasterDetailContainer();
            masterDetailNav.Init("Menu", "Menu.png");
            masterDetailNav.AddPage<PeriodPageModel>("Transactions", null);
            masterDetailNav.AddPage<AboutPageModel>("About", null);
            _NavPage = masterDetailNav;
            return _NavPage;
        }
        
        //public async Task SaveToken(string token)
        //{
        //    Token = token;

        //    await GetProfileInfoFromGoogle(token);

        //    // broadcast a message that authentication was successful
        //    MessagingCenter.Send(this, "Authenticated");
        //}

        //public Action SuccessfulLoginAction
        //{
        //    get
        //    {
        //        return new Action(() => _NavPage.Navigation.PopModalAsync());
        //    }
        //}

        //const string googUesrInfoAccessleUrl = "https://www.googleapis.com/oauth2/v1/userinfo?access_token={0}";
        //async Task<bool> GetProfileInfoFromGoogle(string access_token)
        //{
        //    //var progress = UserDialogs.Instance.Loading("Please wait...");
        //    bool isValid = false;
        //    //Google API REST request
        //    string userInfo = await DownloadString(string.Format(googUesrInfoAccessleUrl, access_token));
        //    if (userInfo != "Exception")
        //    {
        //        //step 4: Deserialize the JSON response to get data in class object
        //        GoogleInfo = JsonConvert.DeserializeObject<GoogleInfo>(userInfo);
        //        isValid = true;
        //    }
        //    else
        //    {
        //        isValid = false;
        //        await UserDialogs.Instance.AlertAsync("Failed retrieving user information");
        //    }
        //    //progress.Hide();
        //    return isValid;
        //}

        //async Task<string> DownloadString(string strUri)
        //{
        //    var client = new HttpClient(new NativeMessageHandler());
        //    string strResultData;
        //    try
        //    {
        //        strResultData = await client.GetStringAsync(new Uri(strUri));
        //    }
        //    catch
        //    {
        //        strResultData = "Exception";
        //    }
        //    finally
        //    {
        //        if (client != null)
        //        {
        //            client.Dispose();
        //            client = null;
        //        }
        //    }
        //    return strResultData;
        //}
    }
}
