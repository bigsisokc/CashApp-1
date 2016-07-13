using FreshMvvm;
using Xamarin.Forms;

namespace CashApp.Containers
{
    public class CustomMasterDetailContainer : FreshMasterDetailNavigationContainer
    {
        protected override void CreateMenuPage(string menuPageTitle, string menuIcon = null)
        {
            var menuPage = new ContentPage();
            menuPage.Title = menuPageTitle;
            menuPage.BackgroundColor = Color.FromHex("404040");
            menuPage.Padding = new Thickness(5, 20, 0, 0);

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var userLayout = new StackLayout();
            userLayout.Orientation = StackOrientation.Horizontal;
            Grid.SetRow(userLayout, 0);
            Grid.SetColumn(userLayout, 0);
            Grid.SetColumnSpan(userLayout, 2);

            var userImage = new Image();
            userImage.Source = App.Instance.Picture;
            userImage.WidthRequest = 48;
            userImage.HeightRequest = 48;

            var userNameLayout = new StackLayout();
            userNameLayout.Orientation = StackOrientation.Vertical;

            var userLabel = new Label();
            userLabel.Text = App.Instance.Name;
            userLabel.TextColor = Color.White;

            var userEmail = new Label();
            userEmail.Text = App.Instance.Email;
            userEmail.TextColor = Color.White;

            userNameLayout.Children.Add(userLabel);
            userNameLayout.Children.Add(userEmail);

            userLayout.Children.Add(userImage);
            userLayout.Children.Add(userNameLayout);

            var listView = new ListView();
            listView.ItemsSource = PageNames;
            listView.ItemSelected += (sender, args) =>
            {
                if (Pages.ContainsKey((string)args.SelectedItem))
                {
                    Detail = Pages[(string)args.SelectedItem];
                }

                IsPresented = false;
            };
            Grid.SetRow(listView, 2);
            Grid.SetColumn(listView, 1);

            grid.Children.Add(userLayout);
            grid.Children.Add(listView);

            menuPage.Content = grid;

            Master = menuPage;
        }
    }
}
