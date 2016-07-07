using CashApp.Converters;
using CashApp.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace CashApp.Pages
{
    public class PeriodPage : ContentPage
    {
        private FloatingActionButtonView fab;
        private ListView list;
        private int appearingListItemIndex = 0;

        public PeriodPage()
        {
            Title = "Period List";

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

            formLayout.Children.Add(GetScrollView());

            fab = GetFloatingButton();
            formLayout.Children.Add(fab);

            this.Content = formLayout;
        }

        private ScrollView GetScrollView()
        {
            var scroll = new ScrollView();
            AbsoluteLayout.SetLayoutBounds(scroll, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(scroll, AbsoluteLayoutFlags.All);

            list = GetListView();
            scroll.Content = list;
            return scroll;
        }

        private ListView GetListView()
        {
            var view = new ListView();
            view.HasUnevenRows = true;
            view.IsPullToRefreshEnabled = true;
            view.SetBinding(ListView.IsRefreshingProperty, new Binding("IsBusy"));
            view.SetBinding(ListView.ItemsSourceProperty, new Binding("Items"));
            view.SetBinding(ListView.RefreshCommandProperty, new Binding("LoadItemCommand"));
            view.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedItem", BindingMode.TwoWay));
            view.ItemTemplate = GetListViewItemTemplate();
            //view.SeparatorColor = Color.White;

            return view;
        }

        private View GetRepeaterView()
        {
            var repeater = new RepeaterView<GroupingAmount>
            {
                Spacing = 10,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid();
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var labelCurrency = new Label();
                    labelCurrency.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    //labelCurrency.TextColor = Color.White;
                    labelCurrency.VerticalOptions = LayoutOptions.Center;
                    labelCurrency.SetBinding(Label.TextProperty, new Binding("Currency"));
                    Grid.SetColumn(labelCurrency, 0);

                    var labelAmount = new Label();
                    labelAmount.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    //labelAmount.TextColor = Color.White;
                    labelAmount.VerticalOptions = LayoutOptions.Center;
                    labelAmount.SetBinding(Label.TextProperty, new Binding("Amount", converter: new CurrencyConverter()));
                    Grid.SetColumn(labelAmount, 1);

                    grid.Children.Add(labelCurrency);
                    grid.Children.Add(labelAmount);

                    return new ViewCell { View = labelAmount };
                })
            };
            repeater.SetBinding(RepeaterView<GroupingAmount>.ItemsSourceProperty, new Binding("Amounts"));

            return repeater;
        }
        
        private DataTemplate GetListViewItemTemplate()
        {
            var template = new DataTemplate(() =>
            {
                var grid = new Grid();
                //grid.BackgroundColor = Color.FromHex("#FF3498DB");
                grid.Padding = new Thickness(10);
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });

                var labelDescription = new Label();
                labelDescription.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                //labelDescription.TextColor = Color.White;
                labelDescription.VerticalOptions = LayoutOptions.Center;
                labelDescription.SetBinding(Label.TextProperty, new Binding("Period"));
                Grid.SetColumn(labelDescription, 0);

                var amountList = GetRepeaterView();
                Grid.SetColumn(amountList, 1);

                grid.Children.Add(labelDescription);
                grid.Children.Add(amountList);

                return new ViewCell { View = grid };
            });

            return template;
        }

        private FloatingActionButtonView GetFloatingButton()
        {
            var btn = new FloatingActionButtonView();
            AbsoluteLayout.SetLayoutBounds(btn, new Rectangle(1, 1, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(btn, AbsoluteLayoutFlags.PositionProportional);
            btn.ColorNormal = Color.FromHex("#FF3498db");
            btn.ColorPressed = Color.Black;
            btn.ColorRipple = Color.FromHex("#FF3498db");
            btn.ImageName = "ic_add.png";
            btn.SetBinding(FloatingActionButtonView.CommandProperty, new Binding("AddItemCommand"));
            return btn;
        }

        protected override void OnAppearing()
        {
            var formLayout = new AbsoluteLayout();
            formLayout.VerticalOptions = LayoutOptions.FillAndExpand;
            formLayout.HorizontalOptions = LayoutOptions.FillAndExpand;

            formLayout.Children.Add(GetScrollView());

            fab = GetFloatingButton();
            formLayout.Children.Add(fab);

            this.Content = formLayout;

            base.OnAppearing();
            list.ItemAppearing += List_ItemAppearing;
            list.ItemDisappearing += List_ItemDisappearing;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            list.ItemAppearing -= List_ItemAppearing;
            list.ItemDisappearing -= List_ItemDisappearing;
        }

        async void List_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
        {
            await Task.Run(() =>
            {
                var items = list.ItemsSource as ObservableCollection<Grouping>;
                if (items != null)
                {
                    var item = e.Item as Grouping;
                    if (item != null)
                    {
                        var index = items.IndexOf(item);
                        if (index < appearingListItemIndex)
                        {
                            Device.BeginInvokeOnMainThread(() => fab.Hide());
                        }
                        appearingListItemIndex = index;
                    }
                }
            });
        }

        async void List_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await Task.Run(() =>
            {
                var items = list.ItemsSource as ObservableCollection<Grouping>;
                if (items != null)
                {
                    var item = e.Item as Grouping;
                    if (item != null)
                    {
                        var index = items.IndexOf(item);
                        if (index < appearingListItemIndex)
                        {
                            Device.BeginInvokeOnMainThread(() => fab.Show());
                        }
                        appearingListItemIndex = index;
                    }
                }
            });
        }
        
    }
}
