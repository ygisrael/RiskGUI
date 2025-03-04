using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace EZXLightspeedGUI.Converters
{
    public class ConnectionColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush color = null;
            color = App.Current.Resources["SolidColorBrush_Transparent"] as SolidColorBrush;

            if (values != null && values.Count() == 2 && values[0] is bool && values[1] is bool)
            {
                bool isConnected = (bool)values[0];
                bool isLoggedin = (bool)values[1];
                if (isConnected && isLoggedin)
                {
                    color = App.Current.Resources["SolidColorBrush_Connected"] as SolidColorBrush;
                }
                else if (isConnected && !isLoggedin)
                {
                    color = App.Current.Resources["SolidColorBrush_Logout"] as SolidColorBrush;
                }
                else
                {
                    color = App.Current.Resources["SolidColorBrush_Disconnected"] as SolidColorBrush;
                }
            }

            return color;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
