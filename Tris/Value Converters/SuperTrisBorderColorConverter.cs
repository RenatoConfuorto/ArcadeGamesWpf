using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Tris.Value_Converters
{
    /// <summary>
    /// Converter un bool in un colore per il bordo della cella (bianco => false; rosso => true)
    /// </summary>
    public class SuperTrisBorderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            SolidColorBrush brush = (SolidColorBrush)Application.Current.Resources["TextColor"];
            if (value != null) if (value is bool) if ((bool)value) brush = (SolidColorBrush)Application.Current.Resources["AccentColor"];
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
