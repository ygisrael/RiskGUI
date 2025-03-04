using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for ChangeSetting.xaml
    /// </summary>
    public partial class ChangeSetting : Window
    {
        public ChangeSetting()
        {
            InitializeComponent();
        }

        public ChangeSetting(string account)
            : this()
        {
            this.Title = string.Format("{0} - {1}", account, this.Title);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            App.AppManager.GUILSEngine.ChangeSettingReason = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtReason.Text))
            {
                App.AppManager.GUILSEngine.ChangeSettingReason = TxtReason.Text;
            }
            this.Close();
        }

    }
}


