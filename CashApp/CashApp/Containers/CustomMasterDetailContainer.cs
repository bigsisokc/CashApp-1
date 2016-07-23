using FreshMvvm;
using PropertyChanged;
using System.ComponentModel;
using Xamarin.Forms;

namespace CashApp.Containers
{
    [ImplementPropertyChanged]
    public class CustomMasterDetailContainer : FreshMasterDetailNavigationContainer, INotifyPropertyChanged
    {
        Image userImage;
        Label userLabel;
        Label userEmail;

        public CustomMasterDetailContainer()
            : base()
        {
            //MessagingCenter.Subscribe<App>(this, "Authenticated", (sender) =>
            //{
            //    userImage.Source = sender.GoogleInfo.picture;
            //    userLabel.Text = sender.GoogleInfo.name;
            //    userEmail.Text = sender.GoogleInfo.email;
            //});
        }

        protected override void CreateMenuPage(string menuPageTitle, string menuIcon = null)
        {
            var menuPage = new ContentPage();
            menuPage.Title = menuPageTitle;
            menuPage.BackgroundColor = Color.FromHex("404040");
            menuPage.Padding = new Thickness(5, 20, 0, 0);
            //menuPage.BindingContext = Info;

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var listView = new ListView();

            listView.ItemsSource = PageNames;

            listView.ItemSelected += (sender, args) => {
                if (Pages.ContainsKey((string)args.SelectedItem))
                {
                    Detail = Pages[(string)args.SelectedItem];
                }

                IsPresented = false;
            };

            var userLayout = new StackLayout();
            userLayout.Orientation = StackOrientation.Horizontal;

            userImage = new Image();
            userImage.WidthRequest = 48;
            userImage.HeightRequest = 48;
            //userImage.SetBinding(Image.SourceProperty, new Binding("picture"));

            var userNameLayout = new StackLayout();
            userNameLayout.Orientation = StackOrientation.Vertical;

            userLabel = new Label();
            userLabel.TextColor = Color.White;
            //userLabel.Text = "Name";
            //userLabel.SetBinding(Label.TextProperty, new Binding("name"));

            userEmail = new Label();
            userEmail.TextColor = Color.White;
            userEmail.SetBinding(Label.TextProperty, new Binding("email"));

            userNameLayout.Children.Add(userLabel);
            userNameLayout.Children.Add(userEmail);

            userLayout.Children.Add(userImage);
            userLayout.Children.Add(userNameLayout);


            Grid.SetRow(userLayout, 0);
            Grid.SetColumn(userLayout, 0);
            Grid.SetColumnSpan(userLayout, 2);
            Grid.SetRow(listView, 2);
            Grid.SetColumn(listView, 1);
            grid.Children.Add(userLayout);
            grid.Children.Add(listView);

            menuPage.Content = grid;

            var navPage = new NavigationPage(menuPage) { Title = "Menu" };

            if (!string.IsNullOrEmpty(menuIcon))
                navPage.Icon = menuIcon;

            Master = navPage;
        }
    }
}
