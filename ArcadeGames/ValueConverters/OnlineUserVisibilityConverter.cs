using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ArcadeGames.ValueConverters
{
    public class OnlineUserVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string UserName = String.Empty;
            Visibility visibility = Visibility.Collapsed;
            if (value != null &&
                value is string)
                UserName = ((string)value).Replace(new char(), ' '); // Remove null chars

                if(!String.IsNullOrWhiteSpace(UserName))
                    visibility = Visibility.Visible;
            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
