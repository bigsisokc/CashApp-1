using CashApp.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(CashApp.Droid.InfoService))]
namespace CashApp.Droid
{
    public class InfoService : IInfoService
    {

        public string DeviceId
        {
            get
            {
                return Android.OS.Build.Serial;
            }
        }

        public string PackageName
        {
            get
            {
                return Forms.Context.PackageName;
            }
        }



        public string AppVersionName
        {
            get
            {
                var context = Forms.Context;
                return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName;
            }
        }

        public int AppVersionCode
        {
            get
            {
                var context = Forms.Context;
                return context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionCode;
            }
        }

        public double DeviceScreenWidth
        {
            get
            {
                var displayMetrics = Forms.Context.Resources.DisplayMetrics;
                return displayMetrics.WidthPixels / displayMetrics.Density;
            }
        }
        public double DeviceScreenHeight
        {
            get
            {
                var displayMetrics = Forms.Context.Resources.DisplayMetrics;
                return displayMetrics.HeightPixels / displayMetrics.Density;
            }
        }
    }
}