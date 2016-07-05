namespace CashApp.Models
{
    public interface IInfoService
    {
        string DeviceId { get; }
        string PackageName { get; }
        string AppVersionName { get; }
        int AppVersionCode { get; }
        double DeviceScreenWidth { get; }
        double DeviceScreenHeight { get; }
    }
}
