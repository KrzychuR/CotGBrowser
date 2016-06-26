using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CotGBrowser.Common
{
    /// <summary>
    /// Konwersja bool na widoczność elementu
    /// Jeżeli jako parametr (parameter w metodach) zostanie przekazana wartość true to wartość loginczna (value) zostanie zanegowana przed konwersją
    /// </summary>
    public class Bool2VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool && targetType == typeof(Visibility))
            {
                bool neg = System.Convert.ToBoolean(parameter);

                if (neg)
                    return (!(bool)value) ? Visibility.Visible : Visibility.Collapsed;
                else
                    return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Visibility)
            {
                bool neg = System.Convert.ToBoolean(parameter);

                if (neg)
                    return !(((Visibility)value) == Visibility.Visible);
                else
                    return ((Visibility)value) == Visibility.Visible;
            }
            else
                return null;
        }
    }
}
