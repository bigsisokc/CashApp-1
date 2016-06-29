using System;
using System.Globalization;
using Xamarin.Forms;

namespace CashApp.Converters
{
    public class DecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal)
            {
                return string.Format("{0:#,##0.##}", value);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                decimal result = 0;
                if (decimal.TryParse(value.ToString(), out result))
                {
                    return result;
                }
            }
            return value;
        }
    }
}
