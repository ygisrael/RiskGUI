using System;
using System.Windows.Data;
using System.Windows.Media;

namespace EZXApexRiskGUI.Converters
{
    public class AccountEnableDisableColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush color = App.Current.Resources["SolidColorBrush_Transparent"] as SolidColorBrush;

            if (value is bool)
            {
                bool isEnabled = (bool) value;
                if (isEnabled == false)
                {
                    color = App.Current.Resources["SolidColorBrush_Disabled"] as SolidColorBrush;
                }
                else
                {
                    color = App.Current.Resources["SolidColorBrush_Enabled"] as SolidColorBrush;
                }
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object o = value;
            return o;
        }
    }
}
