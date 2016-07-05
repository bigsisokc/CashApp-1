using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using CashApp.Droid;
using HockeyApp.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace CashApp.Droid
{
    [Activity(Label = "My Cash"
        , Theme = "@style/MyTheme"
        , Icon = "@android:color/transparent"
        , MainLauncher = true
        , ScreenOrientation = ScreenOrientation.Portrait
        , ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            CrashManager.Register(this, "21d237e64379458aa960e50d0bc15cb0");
            UserDialogs.Init(this);


            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        void CheckForUpdates()
        {
            // Remove this for store builds!
            UpdateManager.Register(this, "21d237e64379458aa960e50d0bc15cb0");
        }

        void UnregisterManagers()
        {
            UpdateManager.Unregister();
        }

        protected override void OnPause()
        {
            base.OnPause();

            UnregisterManagers();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            UnregisterManagers();
        }
    }
}