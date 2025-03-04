using System.Reflection;
using System.Windows;
using System.Windows.Input;
using EZXLib;

namespace EZXApexRiskGUI.View
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            Logger.DEBUG("AboutWindow()...");

            InitializeComponent();
            VersionDate.Text = string.Format(Properties.Resources.VersionDateText, "03/09/2020");
            Version.Text = string.Format(Properties.Resources.VersionNumberText, Assembly.GetExecutingAssembly().GetName().Version);
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("AboutWindow.OKButton_Click()...");

            Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.GetHashCode() == 13)
            {
                Logger.DEBUG("AboutWindow.Window_PreviewKeyDown() Key=13 (Esc)...");

                Close();
            }
        }
    }
}
