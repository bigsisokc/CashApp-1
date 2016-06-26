using CashApp.Converters;
using Xamarin.Forms;

namespace CashApp
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
            scroll.Content = GetStackLayout();
            return scroll;
        }

        private StackLayout GetStackLayout()
        {
            var layout = new StackLayout();
            layout.VerticalOptions = LayoutOptions.FillAndExpand;
            layout.HorizontalOptions = LayoutOptions.FillAndExpand;
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
            view.HorizontalOptions = LayoutOptions.FillAndExpand;
            view.VerticalOptions = LayoutOptions.FillAndExpand;
            view.IsGroupingEnabled = true;
            view.IsPullToRefreshEnabled = true;
            view.SetBinding(ListView.ItemsSourceProperty, new Binding("ItemGrouped", BindingMode.TwoWay));
            view.SetBinding(ListView.IsRefreshingProperty, new Binding("IsBusy", BindingMode.TwoWay));
            view.SetBinding(ListView.RefreshCommandProperty, new Binding("LoadItemCommand"));
            view.SetBinding(ListView.SelectedItemProperty, new Binding("SelectedItem", BindingMode.TwoWay));

            return view;
        }

//            < AbsoluteLayout HorizontalOptions = "FillAndExpand" VerticalOptions = "FillAndExpand" >
   
//           < ActivityIndicator AbsoluteLayout.LayoutBounds = "0.5,0.5,-1,-1"
//                           AbsoluteLayout.LayoutFlags = "PositionProportional"
//                           IsRunning = "{Binding IsBusy}"
//                           IsVisible = "{Binding IsBusy}" />
//        < ScrollView AbsoluteLayout.LayoutBounds = "0,0,1,1"
//                    AbsoluteLayout.LayoutFlags = "All"
//                    IsVisible = "{Binding IsBusy,
//                                        Converter ={ StaticResource not}
//        }">
//            <StackLayout HorizontalOptions = "FillAndExpand"
//                         Orientation="Vertical"
//                         Padding="10"
//                         Spacing="10"
//                         VerticalOptions="FillAndExpand">
//                <ListView Name = "ListView1"
//                          GroupDisplayBinding="{Binding Key}"
//                          GroupShortNameBinding="{Binding Key}"
//                          HasUnevenRows="True"
//                          HorizontalOptions="FillAndExpand"
//                          IsGroupingEnabled="true"
//                          IsPullToRefreshEnabled="True"
//                          IsRefreshing="{Binding IsBusy,
//                                                 Mode=OneWay
//    }"
//                          ItemsSource="{Binding ItemGrouped,
//                                                Mode=TwoWay
//}"
//                          RefreshCommand="{Binding LoadItemCommand}"
//                          SelectedItem="{Binding SelectedItem,
//                                                 Mode=TwoWay}"
//                          VerticalOptions="FillAndExpand">
//                    <ListView.GroupHeaderTemplate>
//                        <DataTemplate>
//                            <ViewCell Height = "40" >
//                                < Grid BackgroundColor="#3498DB" Padding="5">
//                                    <Grid.ColumnDefinitions>
//                                        <ColumnDefinition Width = "*" />
//                                        < ColumnDefinition Width="50" />
//                                        <ColumnDefinition Width = "150" />
//                                    </ Grid.ColumnDefinitions >
//                                    < Label Grid.Column="0"
//                                           Grid.ColumnSpan="2"
//                                           FontSize="Large"
//                                           Text="{Binding KeyDescription}"
//                                           TextColor="White"
//                                           VerticalOptions="Center" />
//                                    <Label Grid.Column="2"
//                                           FontSize= "Large"
//                                           Text= "{Binding KeyAmount,
//                                                          Converter={ StaticResource currencyConverter}}"
//                                           TextColor="White"
//                                           VerticalOptions="Center" />
//                                </Grid>
//                            </ViewCell>
//                        </DataTemplate>
//                    </ListView.GroupHeaderTemplate>
//                    <ListView.ItemTemplate>
//                        <DataTemplate>
//                            <ViewCell Height = "70" >
//                                < Grid Padding="5">
//                                    <Grid.RowDefinitions>
//                                        <RowDefinition Height = "30" />
//                                        < RowDefinition Height="30" />
//                                    </Grid.RowDefinitions>
//                                    <Grid.ColumnDefinitions>
//                                        <ColumnDefinition Width = "*" />
//                                        < ColumnDefinition Width="50" />
//                                        <ColumnDefinition Width = "150" />
//                                    </ Grid.ColumnDefinitions >
//                                    < Label Grid.Row="0"
//                                           Grid.Column="0"
//                                           FontSize="Medium"
//                                           LineBreakMode="TailTruncation"
//                                           Text="{Binding TransDate,
//                                                          Converter={StaticResource dateConverter}}" />
//                                    <Label Grid.Row="0"
//                                           Grid.Column= "1"
//                                           FontSize= "Medium"
//                                           LineBreakMode= "TailTruncation"
//                                           Text= "{Binding Currency}" />
//                                    < Label Grid.Row= "0"
//                                           Grid.Column= "2"
//                                           FontSize= "Medium"
//                                           LineBreakMode= "TailTruncation"
//                                           Text= "{Binding Amount,
//                                                          Converter={ StaticResource currencyConverter}}" />

//                                    <StackLayout Grid.Row="1"
//                                                 Grid.Column= "0"
//                                                 Grid.ColumnSpan= "3" >
//                                        < Label FontSize= "Small"
//                                               LineBreakMode= "TailTruncation"
//                                               Text= "{Binding Description}" />
//                                    </ StackLayout >
//                                </ Grid >
//                            </ ViewCell >
//                        </ DataTemplate >
//                    </ ListView.ItemTemplate >
//                </ ListView >
//            </ StackLayout >
//        </ ScrollView >
//        < view:FloatingActionButtonView Name = "fab"
//                                       AbsoluteLayout.LayoutBounds= "1,1"
//                                       AbsoluteLayout.LayoutFlags= "PositionProportional"
//                                       ColorNormal= "#3498db"
//                                       ColorPressed= "Black"
//                                       ColorRipple= "#3498db"
//                                       ImageName= "ic_add.png" />
//    </ AbsoluteLayout >
    }
}
