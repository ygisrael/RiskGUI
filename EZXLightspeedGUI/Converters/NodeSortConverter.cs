using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Collections.ObjectModel;
using EZX.LightspeedEngine.Entity;

namespace EZXLightspeedGUI.Converters
{
    public class NodeSortConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                ObservableCollection<INodeEntity> nodes = value as ObservableCollection<INodeEntity>;
                if (nodes != null)
                {
                    List<INodeEntity> sorted = new List<INodeEntity>(nodes);
                    sorted.Sort(new Comparison<INodeEntity>((x, y) => x.DisplayIndex.CompareTo(y.DisplayIndex)));
                    return sorted;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
