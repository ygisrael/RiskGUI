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
using EZXLightspeedGUI.ViewModel;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for ApplicationSettingsView.xaml
    /// </summary>
    public partial class ApplicationSettingsView : Window
    {
        private ApplicationSettingsVM VM
        {
            get
            {
                return this.DataContext as ApplicationSettingsVM;
            }
        }


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
