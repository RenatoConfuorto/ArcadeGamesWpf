using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace LIB.ValueConverters
{
    public class ActivePlayerBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush result = (SolidColorBrush)Application.Current.Resources["TextColor"];
            if (value != null) if (value is bool data) if(data) result = (SolidColorBrush)Application.Current.Resources["AccentColor"];
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
