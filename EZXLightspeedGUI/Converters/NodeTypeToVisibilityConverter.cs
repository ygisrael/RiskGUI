using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows;
using EZX.LightspeedEngine.Entity;

namespace EZXLightspeedGUI.Converters
{
    public class NodeTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null && parameter != null)
            {
                string param = parameter.ToString();
                if ((value is Account) || ((value as Group).IsAccountGroup))
                {
                    if (param.Equals("DELACCOUNT")
                        || param.Equals("CONTEXTMENU")
                        || param.Equals("RISKSETTING"))
                    {
                        return Visibility.Visible;
                    }
                }
                else if (value is Group)
                {
                    if (!(value as Group).IsDefaultGroup)
                    {
                        if ((value as Group).IsInEditMode)
                        {
                            if (param.Equals("DELGROUP"))
                            {
                                return Visibility.Visible;
                            }
                        }
                        else
                        {
                            if (param.Equals("DELGROUP")
                                || param.Equals("RENGROUP")
                                || param.Equals("CREACCOUNT")
                                || param.Equals("CONTEXTMENU")
                                || param.Equals("RISKSETTING"))
                            {
                                return Visibility.Visible;
                            }
                        }
                    }
                    else
                    {
                        return Visibility.Collapsed;
                    }

                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
