using CashApp.Converters;
using System;
using Xamarin.Forms;

namespace CashApp.Views
{
    public class ItemPage : ContentPage
    {
        public ItemPage()
        {
            Title = "Add / Edit Item";

            var labelStyle = new Style(typeof(Label))
            {
                Setters =
                {
                    new Setter { Property = Label.FontSizeProperty, Value = 12 }
                }
            };

            Resources = new ResourceDictionary();
            Resources.Add(labelStyle);

            var layout = new AbsoluteLayout();
            layout.HorizontalOptions = LayoutOptions.FillAndExpand;
            layout.VerticalOptions = LayoutOptions.FillAndExpand;
            layout.Children.Add(GetScrollView());

            Content = layout;
        }
        
        private ScrollView GetScrollView()
        {
            var scroll = new ScrollView();
            AbsoluteLayout.SetLayoutBounds(scroll, new Rectangle(0, 0, 1, 1));
            AbsoluteLayout.SetLayoutFlags(scroll, AbsoluteLayoutFlags.All);

            var stack = GetStackView();
            scroll.Content = stack;
            return scroll;
        }

        private StackLayout GetStackView()
        {
            var stack = new StackLayout();
            stack.Orientation = StackOrientation.Vertical;
            stack.Spacing = 10;
            stack.VerticalOptions = LayoutOptions.StartAndExpand;

            var descLabel = new Label();
            descLabel.Text = "Description";

            var descEntry = new Entry();
            descEntry.Placeholder = "Transaction Description";
            descEntry.SetBinding(Entry.TextProperty, new Binding("Description", BindingMode.TwoWay));

            var dateLabel = new Label();
            dateLabel.Text = "Date";

            var datePicker = new DatePicker();
            datePicker.Format = "dd-MM-yyyy";
            datePicker.MaximumDate = new DateTime(2050, 12, 31);
            datePicker.MinimumDate = new DateTime(2000, 1, 1);
            datePicker.VerticalOptions = LayoutOptions.CenterAndExpand;
            datePicker.SetBinding(DatePicker.DateProperty, new Binding("TransDate", BindingMode.TwoWay));

            var amountLabel = new Label();
            amountLabel.Text = "Amount";

            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var currencyPicker = new Picker();
            currencyPicker.HorizontalOptions = LayoutOptions.FillAndExpand;
            currencyPicker.VerticalOptions = LayoutOptions.FillAndExpand;
            currencyPicker.SetBinding(Picker.SelectedIndexProperty, new Binding("CurrencyIndex", BindingMode.TwoWay));
            currencyPicker.Items.Add("IDR");
            currencyPicker.Items.Add("SGD");
            Grid.SetColumn(currencyPicker, 0);

            var amountEntry = new Entry();
            amountEntry.Placeholder = "Amount";
            amountEntry.SetBinding(Entry.TextProperty, new Binding("Amount", BindingMode.TwoWay, new DecimalConverter()));
            amountEntry.HorizontalOptions = LayoutOptions.FillAndExpand;
            amountEntry.VerticalOptions = LayoutOptions.FillAndExpand;
            //amountEntry.Keyboard = Keyboard.Numeric;
            Grid.SetColumn(amountEntry, 1);

            grid.Children.Add(currencyPicker);
            grid.Children.Add(amountEntry);

            var saveButton = new Button();
            saveButton.Text = "Save";
            saveButton.SetBinding(Button.CommandProperty, new Binding("SaveItemCommand"));

            var deleteButton = new Button();
            deleteButton.Text = "Delete";
            deleteButton.SetBinding(Button.CommandProperty, new Binding("DeleteItemCommand"));

            stack.Children.Add(descLabel);
            stack.Children.Add(descEntry);
            stack.Children.Add(dateLabel);
            stack.Children.Add(datePicker);
            stack.Children.Add(amountLabel);
            stack.Children.Add(grid);
            stack.Children.Add(saveButton);
            stack.Children.Add(deleteButton);

            return stack;
        }
    }
}

