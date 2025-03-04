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
using System.Reflection;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            EZXLib.Logger.DEBUG("AboutWindow()...");

            InitializeComponent();
            this.VersionDate.Text = string.Format(Properties.Resources.VersionDateText, "03/03/2020");
            this.Version.Text = string.Format(Properties.Resources.VersionNumberText, Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            EZXLib.Logger.DEBUG("AboutWindow.OKButton_Click()...");

            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.GetHashCode() == 13)
            {
                EZXLib.Logger.DEBUG("AboutWindow.Window_PreviewKeyDown() Key=13 (Esc)...");

                this.Close();
            }
        }
    }
}
