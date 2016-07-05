using CashApp.Models;
using Xamarin.Forms;

namespace CashApp.Views
{
    public class AboutPage : ContentPage
    {
        public AboutPage()
        {
            Title = "About";
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

            var layout = GetStackLayout();

            Content = layout;
        }

        private StackLayout GetStackLayout()
        {
            var stack = new StackLayout();
            stack.Orientation = StackOrientation.Vertical;
            stack.Spacing = 10;
            stack.VerticalOptions = LayoutOptions.StartAndExpand;

            var titleLabel = new Label();
            titleLabel.Text = "My Cash";
            titleLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
            titleLabel.HorizontalTextAlignment = TextAlignment.Center;
            titleLabel.FontSize = 24;

            var infoService = DependencyService.Get<IInfoService>();
            var versionLabel = new Label();
            versionLabel.Text = string.Format("{0}.{1}", infoService.AppVersionName, infoService.AppVersionCode);
            versionLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
            versionLabel.HorizontalTextAlignment = TextAlignment.Center;
            versionLabel.FontSize = 24;

            stack.Children.Add(titleLabel);
            stack.Children.Add(versionLabel);

            return stack;
        }
    }
}
