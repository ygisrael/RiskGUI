using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using EZX.LightspeedEngine.Entity;

namespace EZXLightspeedGUI.Converters
{
    public class NodeToHeaderTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                if (value is Account)
                {
                    Account account = value as Account;
                    return "Edit Settings for " + account.Name;
                }
                else if ((value as Group).IsAccountGroup)
                {
                    Group group = value as Group;
                    return "Edit Settings for " + group.Name;
                }
                return "Edit Risk Settings";
            }
            else
            {
                return "****";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
