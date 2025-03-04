using EZXApexRiskGUI.ViewModel;
using EZXLib;
using EZXWPFLibrary.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace EZXApexRiskGUI.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView
    {

        private LoginVM VM => DataContext as LoginVM;

        private static bool IsOpen;
        private static LoginView loginView;

        public static LoginView GetLoginView()
        {
            Logger.DEBUG("LoginView.GetLoginView()...");

            if (IsOpen)
            {
                return loginView;
            }

            return new LoginView();
        }

        public LoginView()
        {
            Logger.DEBUG("LoginView()...");

            InitializeComponent();
            loginView = this;
            VM.LoginTimerInit();
            Title = Properties.Resources.LoginWindowTitle + " - " + string.Format(Properties.Resources.VersionNumberText, Assembly.GetExecutingAssembly().GetName().Version);
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("LoginView.Login_Click()...");

            VM.Password = passwordBox.Password;
            VM.Login();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("LoginView.SettingButton_Click()...");

            ApplicationSettingsView view = new ApplicationSettingsView();
            view.ShowDialog();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("LoginView.Grid_Loaded()...");

            //listen to the IsAuthenticated property of viewmodel
            if ((sender as FrameworkElement)?.DataContext is LoginVM vm)
            {
                //get any saved password
                if (!string.IsNullOrEmpty(vm.Password))
                {
                    passwordBox.Password = vm.Password;
                }

                //register for property changed event
                vm.PropertyChanged += VM_PropertyChanged;
            }
        }

        private void VM_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Logger.DEBUG("LoginView.VM_PropertyChanged()...");

            if (sender is LoginVM vm)
            {
                //if IsAuthenticated property is changed
                if (e.PropertyName == vm.GetPropertyName(v => v.IsAuthenticated))
                {
                    if (vm.IsAuthenticated)
                    {
                        Dispatcher.Invoke(Close);
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            IsOpen = false;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            IsOpen = true;
        }

        private void CloseWindow()
        {
            if (loginView.Owner is Window window && !App.AppManager.GUILSEngine.IsConnected)
            {
                loginView.Owner.Close();
            }
            Logger.ERROR($"LoginView.CloseWindow()");
            Environment.Exit(0);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            bool wasCodeClosed = (new StackTrace().GetFrames() ?? throw new InvalidOperationException()).FirstOrDefault(x => x.GetMethod() == typeof(Window).GetMethod("Close")) != null;
            if (!wasCodeClosed) //Not closed with this.Close(), i.e. 'X' button or Alt+F4. Cancel button and ESC button will be ignored
            {
                Logger.DEBUG("LoginView.OnClosing()...");
                CloseWindow();
            }
        }
    }
}
