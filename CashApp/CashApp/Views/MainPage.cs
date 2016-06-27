using CashApp.Converters;
using Xamarin.Forms;

namespace CashApp.Views
{
    public class MainPage : ContentPage
    {
        public MainPage()
        {
            var toolbarAdd = new ToolbarItem
            {
                Text = "Add",
                ClassId = "Add",
                Order = ToolbarItemOrder.Primary
            };
            toolbarAdd.SetBinding(ToolbarItem.CommandProperty, new Binding("AddItemCommand"));

            ToolbarItems.Add(toolbarAdd);

            Padding = new Thickness(5, 0, 5, 95);

            var labelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontSizeProperty, Value = 12 }
                }
            };

            Resources = new ResourceDictionary();
            Resources.Add(labelStyle);

            var formLayout = new AbsoluteLayout();
            formLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            formLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

            formLayout.Children.Add(GetActivityIndicator());
            formLayout.Children.Add(GetScrollView());

            this.Content = formLayout;
        }

        private ActivityIndicator GetActivityIndicator()
        {
            var indicator = new ActivityIndicator();
            indicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));
            indicator.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsBusy"));
            AbsoluteLayout.SetLayoutBounds(indicator, new Rectangle(0.5, 0.5, -1, -1));
            AbsoluteLayout.SetLayoutFlags(indicator, AbsoluteLayoutFlags.PositionProportional);
            return indicator;
        }

        private ScrollView GetScrollView()
        {
            var scroll = new ScrollView();
            AbsoluteLayout.SetLayoutBounds(scroll, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(scroll, AbsoluteLayoutFlags.All);
            scroll.SetBinding(ScrollView.IsVisibleProperty, new Binding("IsBusy", converter: new BooleanNegationConverter()));
            scroll.Content = GetListView();
            return scroll;
        }

        private StackLayout GetStackLayout()
        {
            var layout = new StackLayout();
            layout.Orientation = StackOrientation.Vertical;
            layout.Padding = new Thickness(10);
            layout.Spacing = 10;
            
            layout.Children.Add(GetListView());

            return layout;
        }

        private ListView GetListView()
        {
            var view = new ListView();
            view.HasUnevenRows = true;
            view.IsGroupingEnabled = true;
            view.IsPullToRefreshEnabled = true;
            view.SetBinding(ListView.ItemsSourceProperty, new Binding("ItemGrouped", BindingMode.TwoWay));
            view.SetBinding(ListView.IsRefreshingProperty, new Binding("IsBusy", BindingMode.TwoWay));
            view.SetBinding(ListView.RefreshCommandProperty, new Binding("LoadItemCommand"));
            view.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedItem", BindingMode.TwoWay));
            view.GroupDisplayBinding = new Binding("Key");
            view.GroupShortNameBinding = new Binding("Key");
            view.GroupHeaderTemplate = GetListViewHeaderTemplate();
            view.ItemTemplate = GetListViewItemTemplate();

            return view;
        }

        private DataTemplate GetListViewHeaderTemplate()
        {
            var template = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.BackgroundColor = Color.FromHex("#FF3498DB");
                grid.Padding = new Thickness(5);
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

                var labelDescription = new Label();
                labelDescription.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                labelDescription.TextColor = Color.White;
                labelDescription.VerticalOptions = LayoutOptions.Center;
                labelDescription.SetBinding(Label.TextProperty, new Binding("KeyDescription"));
                Grid.SetColumn(labelDescription, 0);
                Grid.SetColumnSpan(labelDescription, 2);

                var labelAmount = new Label();
                labelAmount.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                labelAmount.TextColor = Color.White;
                labelAmount.VerticalOptions = LayoutOptions.Center;
                labelAmount.SetBinding(Label.TextProperty, new Binding("KeyAmount", converter: new CurrencyConverter()));
                Grid.SetColumn(labelAmount, 2);

                grid.Children.Add(labelDescription);
                grid.Children.Add(labelAmount);

                return new ViewCell { View = grid, Height = 40 };
            });

            return template;
        }

        private DataTemplate GetListViewItemTemplate()
        {
            var template = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.Padding = new Thickness(5);
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

                var labelDate = new Label();
                labelDate.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                labelDate.SetBinding(Label.TextProperty, new Binding("TransDate", converter: new DateConverter()));
                Grid.SetColumn(labelDate, 0);

                var labelCurrency = new Label();
                labelCurrency.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                labelCurrency.SetBinding(Label.TextProperty, new Binding("Currency"));
                Grid.SetColumn(labelCurrency, 1);

                var labelAmount = new Label();
                labelAmount.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                labelAmount.SetBinding(Label.TextProperty, new Binding("Amount", converter: new CurrencyConverter()));
                Grid.SetColumn(labelAmount, 2);

                var labelDescription = new Label();
                labelDescription.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
                labelDescription.SetBinding(Label.TextProperty, new Binding("Description"));
                Grid.SetColumn(labelDescription, 0);
                Grid.SetRow(labelDescription, 1);
                Grid.SetColumnSpan(labelDescription, 3);

                grid.Children.Add(labelDate);
                grid.Children.Add(labelCurrency);
                grid.Children.Add(labelAmount);
                grid.Children.Add(labelDescription);

                return new ViewCell { View = grid, Height = 70 };
            });

            return template;
        }
    }
}
