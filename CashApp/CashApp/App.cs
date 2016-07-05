using Acr.UserDialogs;
using CashApp.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;
using MvvmCross.Plugins.Messenger;

namespace CashApp
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            Mvx.RegisterSingleton<IMvxMessenger>(new MvxMessengerHub());
            Mvx.RegisterSingleton(() => UserDialogs.Instance);
            Mvx.RegisterSingleton<IRestService>(new RestService());
            RegisterAppStart<ViewModels.TransactionViewModel>();
        }
    }
}
