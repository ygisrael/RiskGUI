using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace EZXLightspeedGUI.Converters
{
    public class ConnectionTextConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string text = string.Empty;
            if (values != null && values.Count() == 2 && values[0] is bool && values[1] is bool)
            {
                bool isConnected = (bool)values[0];
                bool isLoggedin = (bool)values[1];
                if (isConnected && isLoggedin)
                {
                    text = "Connected.";
                }
                else if (isConnected && !isLoggedin)
                {
                    text = "User is Logout.";
                }
                else
                {
                    text = "Disconnected.";
                }
            }

            return text;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

}
