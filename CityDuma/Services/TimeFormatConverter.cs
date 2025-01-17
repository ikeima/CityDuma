using System;
using System.Globalization;
using System.Windows.Data;

namespace CityDuma.Services
{
    public class TimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan timeSpan)
            {
                return timeSpan.ToString(@"hh\:mm");
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return TimeSpan.ParseExact(value.ToString(), "hh':'mm", culture);
        }
    }
}
