using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using CashApp.Droid;
using HockeyApp.Android;
using MvvmCross.Core.ViewModels;
using MvvmCross.Core.Views;
using MvvmCross.Forms.Presenter.Core;
using MvvmCross.Forms.Presenter.Droid;
using MvvmCross.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace CashApp.Droid
{
    [Activity(Label = "MvxFormsApplicationActivity"
        , Theme = "@style/MyTheme"
        , Icon = "@android:color/transparent"
        , ScreenOrientation = ScreenOrientation.Portrait)]
    public class MvxFormsApplicationActivity
        : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            CrashManager.Register(this, "21d237e64379458aa960e50d0bc15cb0");
            UserDialogs.Init(this);

            Forms.Init(this, bundle);
            var mvxFormsApp = new MvxFormsApp();
            LoadApplication(mvxFormsApp);

            var presenter = Mvx.Resolve<IMvxViewPresenter>() as MvxFormsDroidPagePresenter;
            presenter.MvxFormsApp = mvxFormsApp;

            Mvx.Resolve<IMvxAppStart>().Start();
        }
    }
}