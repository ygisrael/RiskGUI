using System.Windows;
using System.Windows.Input;
using EZXApexRiskGUI.ViewModel;

namespace EZXApexRiskGUI.View
{
    /// <summary>
    /// Interaction logic for ApplicationSettingsView.xaml
    /// </summary>
    public partial class ApplicationSettingsView : Window
    {
        private ApplicationSettingsVM VM => this.DataContext as ApplicationSettingsVM;


        public ApplicationSettingsView()
        {
            InitializeComponent();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.GetHashCode() == 13)
            {
                EZXLib.Logger.DEBUG("ApplicationSettingsView.Window_PreviewKeyDown() Key=13 (Esc)...");
                this.Close();
            }
        }


        private void SettingCancelButton_Click(object sender, RoutedEventArgs e)
        {
            EZXLib.Logger.DEBUG("ApplicationSettingsView.SettingCancelButton_Click()...");

            this.Close();

        }

        private void SettingOKButton_Click(object sender, RoutedEventArgs e)
        {
            EZXLib.Logger.DEBUG("ApplicationSettingsView.SettingOKButton_Click()...");

            VM.ApplySettingsCommand.Execute(null);
            if (string.IsNullOrEmpty(VM.ErrorString))
            {                
                this.Close();
            }
        }
    }
}
