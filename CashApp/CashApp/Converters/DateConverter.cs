using System;
using System.Globalization;
using Xamarin.Forms;

namespace CashApp.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                DateTime dt;
                if (DateTime.TryParse(value.ToString(), out dt))
                {
                    return string.Format("{0:dd MMM yyyy}", dt);
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
