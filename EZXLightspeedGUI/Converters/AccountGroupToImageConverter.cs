using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using EZX.LightspeedEngine.Entity;

namespace EZXLightspeedGUI.Converters
{
    public class AccountGroupToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource imageSource = null;
            Image image = null;
            if (value is Group)
            {
                if ((value as Group).IsDefaultGroup)
                {
                    image = App.Current.Resources["DefaultGroupNodeImage"] as Image;
                }
                else if ((value as Group).IsAccountGroup)
                {
                    image = App.Current.Resources["AccountWithoutGroupNodeImage"] as Image;
                }
                else
                {
                    image = App.Current.Resources["GroupNodeImage"] as Image;
                }
            }
            else if (value is Account)
            {
                if ((value as Account).IsOwnGroup)
                {
                    image = App.Current.Resources["AccountWithoutGroupNodeImage"] as Image;
                }
                else
                {
                    image = App.Current.Resources["AccountNodeImage"] as Image;
                }
            }

            imageSource = image.Source;
            return imageSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
