using System;
using System.Globalization;
using System.Windows.Data;

namespace ViewSwitchingNavigation.Infrastructure.Converters
{
    public class boolNotConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value as bool?;
            return val.HasValue && val.Value ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
