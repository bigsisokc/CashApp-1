using CashApp.Models;
using System.Collections.Generic;
using Xamarin.Forms;

namespace CashApp.Pages
{
    public class MenuListView : ListView
    {
        public MenuListView()
        {
            List<CashApp.Models.MenuItem> data = new MenuListData();

            ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;
            SeparatorVisibility = SeparatorVisibility.None;

            var cell = new DataTemplate(() =>
            {
                var labelMenu = new Label();
                labelMenu.FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));
                labelMenu.SetBinding(Label.TextProperty, new Binding("Title"));
                labelMenu.TextColor = Color.FromHex("AAAAAA");
                labelMenu.Margin = new Thickness(15, 5, 0, 5);

                return new ViewCell { View = labelMenu };
            });

            ItemTemplate = cell;
        }
    }
}
