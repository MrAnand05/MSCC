using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ViewSwitchingNavigation.Infrastructure.Converters
{
    public class TotalSumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var users = value as IEnumerable<object>;
            if (users == null)
                return "$0.00";

            double sum = 0;

            foreach (var u in users)
            {
                //sum += ((User)u).Total;
            }


            return sum.ToString("c");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new System.NotImplementedException();
        }
    }
}
