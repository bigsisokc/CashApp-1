using System.Diagnostics;
using System.Threading.Tasks;

namespace CashApp.Mixins
{
    public static class AsyncMixin
    {
        public static void RunForget(this Task t)
        {
            t.ContinueWith((tResult) =>
            {
                //Console.WriteLine(t.Exception)
                //TODO: Log to Xamarin insights
                Debug.WriteLine(t.Exception);
            },
                TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
