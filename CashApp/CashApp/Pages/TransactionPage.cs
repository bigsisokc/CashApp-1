using CashApp.Converters;
using CashApp.Models;
using CashApp.PageModels;
using FreshMvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CashApp.Pages
{
    public class TransactionPage : BaseContentPage
    {
        private FloatingActionButtonView fab;
        private ListView list;
        private int appearingListItemIndex = 0;

        public TransactionPage()
        {
            Title = "Transaction List";

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
            view.SetBinding(ListView.ItemsSourceProperty, new Binding("Items", BindingMode.TwoWay));
            view.SetBinding(ListView.RefreshCommandProperty, new Binding("LoadItemCommand"));
            view.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedItem", BindingMode.TwoWay));
            view.ItemTemplate = GetListViewItemTemplate();

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
                var items = list.ItemsSource as ObservableCollection<Transaction>;
                if (items != null)
                {
                    var item = e.Item as Transaction;
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
                var items = list.ItemsSource as ObservableCollection<Transaction>;
                if (items != null)
                {
                    var item = e.Item as Transaction;
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

        protected override bool OnBackButtonPressed()
        {
            var pageModel = this.GetModel() as TransactionPageModel;
            if (pageModel != null)
            {
                pageModel.ClosePageCommand.Execute(null);
                return false;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
    }
}
