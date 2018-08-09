// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Windows.Data;

namespace ViewSwitchingNavigation.Infrastructure.Converters
{
    public class CheckAllConverter : IValueConverter
    {
       
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {


            bool isChecked;
            if (value != null)
                isChecked = (bool)value;
            else
                isChecked = false;
            if (isChecked == false)
            {
                return false;
            }
            
            return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked;
            if (value != null)
                isChecked = (bool)value;
            else
                isChecked = false;
            if (isChecked == false)
            {
                return false;
            }

            return true;
        }
    }
}
