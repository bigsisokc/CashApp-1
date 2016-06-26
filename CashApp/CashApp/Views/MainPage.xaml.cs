using Xamarin.Forms;

namespace CashApp.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    ListView1.ItemAppearing += List_ItemAppearing;
        //    ListView1.ItemDisappearing += List_ItemDisappearing;
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();
        //    ListView1.ItemAppearing -= List_ItemAppearing;
        //    ListView1.ItemDisappearing -= List_ItemDisappearing;
        //}

        //async void List_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    await Task.Run(() =>
        //    {
        //        var items = ListView1.ItemsSource as IList;
        //        if (items != null)
        //        {
        //            var index = items.IndexOf(e.Item);
        //            if (index < appearingListItemIndex)
        //            {
        //                Device.BeginInvokeOnMainThread(() => fab.Hide());
        //            }
        //            appearingListItemIndex = index;
        //        }
        //    });
        //}

        //async void List_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    await Task.Run(() =>
        //    {
        //        var items = list.ItemsSource as IList;
        //        if (items != null)
        //        {
        //            var index = items.IndexOf(e.Item);
        //            if (index < appearingListItemIndex)
        //            {
        //                Device.BeginInvokeOnMainThread(() => fab.Show());
        //            }
        //            appearingListItemIndex = index;
        //        }
        //    });
        //}
    }
}
