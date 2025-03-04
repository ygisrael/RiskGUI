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
using EZXWPFLibrary.Helpers;
using EZXLib;
using System.Windows.Threading;
using System.Reflection;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {

        private LoginVM VM
        {
            get
            {
                return this.DataContext as LoginVM;
            }
        }

        private static bool IsOpen;
        private static LoginView loginView;
        private DispatcherTimer LoginTimer;

        public static LoginView GetLoginView()
        {
            EZXLib.Logger.DEBUG("LoginView.GetLoginView()...");

            if (IsOpen)
            {
                return loginView;
            }
            return new LoginView();
        }

        public LoginView()
        {
            EZXLib.Logger.DEBUG("LoginView()...");

            InitializeComponent();

            LoginTimer = new DispatcherTimer();
            LoginTimer.Tick += new EventHandler(LoginTimer_Tick);
            LoginTimer.Interval = new TimeSpan(0, 0, 1);

            this.Title = Properties.Resources.LoginWindowTitle + " - " + string.Format(Properties.Resources.VersionNumberText, Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        void LoginTimer_Tick(object sender, EventArgs e)
        {
            EZXLib.Logger.DEBUG("LoginView.LoginTimer_Tick()...");

            if (VM.IsConnected == true)
            {
                this.VM.AddStatusMessage("Login Completed.");
                MainView mainView = null;
                foreach (Window wnd in Application.Current.Windows)
                {
                    if (wnd is MainView)
                    {
                        mainView = wnd as MainView;
                        break;
                    }
                }

                if (mainView == null)
                {
                    mainView = new MainView();
                    mainView.Show();
                    mainView.Focus();
                }

                LoginTimer.Stop();
                VM.IsAuthenticated = true;
            }
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("LoginView.Login_Click()...");
            this.VM.Password = this.passwordBox.Password;
            LoginTimer.Start();
            this.VM.DoLogin();
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("LoginView.SettingButton_Click()...");

            ApplicationSettingsView view = new ApplicationSettingsView();
            view.ShowDialog();

        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            EZXLib.Logger.DEBUG("LoginView.Grid_Loaded()...");

            //listen to the IsAuthenticated property of viewmodel
            ViewModel.LoginVM vm = (sender as FrameworkElement).DataContext as ViewModel.LoginVM;
            if (vm != null)
            {
                //get any saved password
                if (!string.IsNullOrEmpty(vm.Password))
                    this.passwordBox.Password = vm.Password;

                //register for property changed event
                vm.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(vm_PropertyChanged);
            }
        }

        void vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            EZXLib.Logger.DEBUG("LoginView.vm_PropertyChanged()...");

            LoginVM vm = sender as LoginVM;

            if (vm != null)
            {
                //if IsAuthenticated property is changed
                if (e.PropertyName == vm.GetPropertyName(v => v.IsAuthenticated))
                {
                    if (vm.IsAuthenticated)
                    {
                        this.Dispatcher.Invoke((Action)Close);
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            IsOpen = false;
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            IsOpen = true;
        }

    }
}
