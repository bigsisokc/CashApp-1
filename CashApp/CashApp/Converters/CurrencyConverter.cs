using System;
using System.Globalization;
using Xamarin.Forms;

namespace CashApp.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal amount = 0;

            if (value != null)
            {
                amount = decimal.Parse(value.ToString());
            }

            return string.Format("{0:N0}", amount);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
