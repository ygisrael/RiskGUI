using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace EZXLightspeedGUI.Converters
{
    public class PriceCheckToHeaderValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return "Price Check";
            }
            else
            {
                if (value is bool)
                {
                    bool priceCheckON = (bool)value;
                    if (priceCheckON)
                    {
                        return "Disable Price Check";
                    }
                    else
                    {
                        return "Enable Price Check";
                    }
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
