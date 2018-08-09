using System;
using System.Windows.Data;

namespace ViewSwitchingNavigation.Infrastructure.Converters
{
    public class GenderToChar : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !(value is Gender))
            {
                return null;
            }

            Gender gender = (Gender)value;
            return (gender == Gender.Male ? 'M' : 'F') + " ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || !((char)value == 'M') || !((char)value == 'F'))
            {
                return null;
            }

            //Gender transactionType = (Gender)value;
            return ((char)value == 'M' ? "Male" : "Female") + " ";
        }
    }
}
