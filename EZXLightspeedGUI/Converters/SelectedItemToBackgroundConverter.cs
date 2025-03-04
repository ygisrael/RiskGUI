using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace EZXLightspeedGUI.Converters
{
    public class SelectedItemToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Brush color = null;
            color = App.Current.Resources["SolidColorBrush_Transparent"] as SolidColorBrush;
            if (value is bool)
            {
                if ((bool)value)
                {
                    color = App.Current.Resources["SolidColorBrush_TreeViewSelectedColor"] as SolidColorBrush;
                }
            }
            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
