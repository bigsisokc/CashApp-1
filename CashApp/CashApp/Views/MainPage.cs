using CashApp.Converters;
using CashApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace CashApp.Views
{
    public class MainPage : ContentPage
    {
        private FloatingActionButtonView fab;
        private ListView list;
        private int appearingListItemIndex = 0;

        public MainPage()
        {
            Title = "My Cash";
            //var toolbarAdd = new ToolbarItem
            //{
            //    Text = "Add",
            //    ClassId = "Add",
            //    Order = ToolbarItemOrder.Primary
            //};
            //toolbarAdd.SetBinding(ToolbarItem.CommandProperty, new Binding("AddItemCommand"));

            //ToolbarItems.Add(toolbarAdd);

            Padding = new Thickness(5, 0, 5, 0);

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

            fab = GetFloatingButton();
            formLayout.Children.Add(fab);

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

            list = GetListView();
            scroll.Content = list;
            return scroll;
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
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150) });

                var labelDescription = new Label();
                labelDescription.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                labelDescription.TextColor = Color.White;
                labelDescription.VerticalOptions = LayoutOptions.Center;
                labelDescription.SetBinding(Label.TextProperty, new Binding("Period"));
                Grid.SetColumn(labelDescription, 0);

                var amountList = GetGroupHeaderRepeaterView();
                Grid.SetColumn(amountList, 1);
                Grid.SetColumnSpan(amountList, 2);

                grid.Children.Add(labelDescription);
                grid.Children.Add(amountList);

                return new ViewCell { View = grid };
            });

            return template;
        }

        private View GetGroupHeaderRepeaterView()
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
                    labelCurrency.TextColor = Color.White;
                    labelCurrency.VerticalOptions = LayoutOptions.Center;
                    labelCurrency.SetBinding(Label.TextProperty, new Binding("Currency"));
                    Grid.SetColumn(labelCurrency, 0);

                    var labelAmount = new Label();
                    labelAmount.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                    labelAmount.TextColor = Color.White;
                    labelAmount.VerticalOptions = LayoutOptions.Center;
                    labelAmount.SetBinding(Label.TextProperty, new Binding("Amount", converter: new CurrencyConverter()));
                    Grid.SetColumn(labelAmount, 1);

                    grid.Children.Add(labelCurrency);
                    grid.Children.Add(labelAmount);

                    return new ViewCell { View = grid };
                })
            };
            repeater.SetBinding(RepeaterView<GroupingAmount>.ItemsSourceProperty, new Binding("Amounts"));

            return repeater;
        }

        private ListView GetGroupHeaderListView()
        {
            var view = new ListView();
            view.VerticalOptions = LayoutOptions.Center;
            view.SetBinding(ListView.ItemsSourceProperty, new Binding("Amounts"));

            var template = new DataTemplate(() =>
            {
                var grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var labelCurrency = new Label();
                labelCurrency.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                labelCurrency.TextColor = Color.White;
                labelCurrency.VerticalOptions = LayoutOptions.Center;
                labelCurrency.SetBinding(Label.TextProperty, new Binding("Currency"));
                Grid.SetColumn(labelCurrency, 0);

                var labelAmount = new Label();
                labelAmount.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                labelAmount.TextColor = Color.White;
                labelAmount.VerticalOptions = LayoutOptions.Center;
                labelAmount.SetBinding(Label.TextProperty, new Binding("Amount", converter: new CurrencyConverter()));
                Grid.SetColumn(labelAmount, 1);

                grid.Children.Add(labelCurrency);
                grid.Children.Add(labelAmount);

                return new ViewCell { View = grid };
            });

            view.ItemTemplate = template;

            return view;
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
                    var item = e.Item as Transaction;
                    if (item != null)
                    {
                        var transactions = items.SelectMany(c => c.Transactions).ToList();
                        var index = transactions.IndexOf(item);
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
                    var item = e.Item as Transaction;
                    if (item != null)
                    {
                        var transactions = items.SelectMany(c => c.Transactions).ToList();
                        var index = transactions.IndexOf(item);
                        if (index < appearingListItemIndex)
                        {
                            Device.BeginInvokeOnMainThread(() => fab.Show());
                        }
                        appearingListItemIndex = index;
                    }
                }
            });
        }

        public int FindIndex<T>(IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }
    }
}
