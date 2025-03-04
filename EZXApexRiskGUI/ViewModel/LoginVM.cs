using EZX.LightSpeedEngine;
using EZXApexRiskGUI.Properties;
using EZXLib;
using EZXWPFLibrary.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EZXApexRiskGUI.View;
using System.Web.UI.WebControls;

namespace EZXApexRiskGUI.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        #region Properties

        private string username;
        private string password;
        private bool isAuthenticated;
        private bool isConnected;
        private DispatcherTimer loginTimer;

        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                this.RaisePropertyChanged(vm => vm.IsConnected);
            }
        }

        ObservableCollection<string> statusMessages = new ObservableCollection<string>();

        public string Password
        {
            get => password;
            set { password = value; this.RaisePropertyChanged(vm => vm.Password); }
        }

        public string UserName
        {
            get => username;
            set { username = value; this.RaisePropertyChanged(vm => vm.UserName); }
        }
        public bool IsAuthenticated
        {
            get => isAuthenticated;
            set
            {
                isAuthenticated = value;
                this.RaisePropertyChanged(vm => vm.IsAuthenticated);
            }
        }

        public ObservableCollection<string> StatusMessages
        {
            get => statusMessages;
            set { statusMessages = value; this.RaisePropertyChanged(vm => vm.statusMessages); }
        }

        #endregion Properties

        #region Constructor

        public LoginVM()
        {
            Logger.DEBUG("LoginVM() started");

            if (!IsDesignMode)
            {
                UserName = App.AppManager.Config.Username;
                Password = App.AppManager.Config.Password;
                AddStatusMessage("Please enter your User ID and Password.");
                AttachEvents();
            }

            Logger.DEBUG("LoginViewVM() finished ");
        }

        private void AttachEvents()
        {
            Logger.DEBUG("LoginViewVM.AttachEvents() started");

            App.AppManager.GUILSEngine.Connected += LSEngine_Connected;
            App.AppManager.GUILSEngine.ConnectErrorOccured += LSEngine_ConnectErrorOccured;
            App.AppManager.GUILSEngine.LoginCompleted += LSEngine_LoginCompleted;
            App.AppManager.GUILSEngine.Disconnected += LSEngine_Disconnected;

            Logger.DEBUG("LoginViewVM.AttachEvents() finished");
        }

        #endregion Constructor

        #region Events

        void LSEngine_Disconnected(object sender, LSConnectionEventArgs e)
        {
            Logger.DEBUG("LoginViewVM.LSEngine_Disconnected()...");
            string errMsg = string.Format(Resources.LoginDisconnectText, e.LsComMgr.ConnectionInfo.Host, e.LsComMgr.ConnectionInfo.Port, e.LsComMgr.ConnectionInfo.Company, e.LsComMgr.ConnectionInfo.IsSSL, DateTime.Now.ToString("HH:mm:ss"));
            AddStatusMessage("Connection Failed for " + e.LsComMgr.ConnectionInfo.Company + " (" + errMsg + ")");

            Logger.DEBUG(errMsg);
        }

        void LSEngine_LoginCompleted(object sender, LSConnectionEventArgs e)
        {
            Logger.DEBUG("LoginViewVM.LSEngine_LoginCompleted()...");
            AddStatusMessage(e.LsComMgr.ConnectionInfo.Company + " Login Completed.");
            IsConnected = true;
            App.AppManager.Config.lastLoginDate = DateTimeUtils.GetLoginDateStringFormat(DateTime.Now);
        }

        void LSEngine_ConnectErrorOccured(object sender, LSConnectionEventArgs e)
        {
            Logger.DEBUG("LSEngine_ConnectErrorOccurred()...");

            string errorMsg = string.Format(Resources.LoginFailedToConnectText, e.LsComMgr.ConnectionInfo.Host, e.LsComMgr.ConnectionInfo.Port, e.LsComMgr.ConnectionInfo.Company, e.LsComMgr.ConnectionInfo.IsSSL, DateTime.Now.ToString("HH:mm:ss"));
            AddStatusMessage(errorMsg);
        }

        void LSEngine_Connected(object sender, LSConnectionEventArgs e)
        {
            Logger.DEBUG("LoginVM.LSEngine_Connected()...");
            string errMsg = string.Format(Resources.LoginConnectText, e.LsComMgr.ConnectionInfo.Host, e.LsComMgr.ConnectionInfo.Port, DateTime.Now.ToString("HH:mm:ss"));
            AddStatusMessage("Connected " + e.LsComMgr.ConnectionInfo.Company + " (" + errMsg + ")");
            Logger.DEBUG(errMsg);
        }

        void LoginTimer_Tick(object sender, EventArgs e)
        {
            Logger.DEBUG("LoginView.LoginTimer_Tick()...");

            if (IsConnected)
            {
                AddStatusMessage("Login Completed.");
                AccountsEnableDisableView mainView = null;
                foreach (Window wnd in Application.Current.Windows)
                {
                    if (wnd is AccountsEnableDisableView)
                    {
                        mainView = wnd as AccountsEnableDisableView;
                        break;
                    }
                }

                if (mainView == null)
                {
                    mainView = new AccountsEnableDisableView();
                    mainView.Show();
                    mainView.Focus();
                }

                loginTimer.Stop();

                IsAuthenticated = true;
            }
        }

        internal void Login()
        {
            Logger.DEBUG("LoginVM.Login()...");
            loginTimer.Start();

            App.AppManager.Config.Username = UserName;
            App.AppManager.Config.Password = Password;
            App.AppManager.GUILSEngine.Connect();
        }

        #endregion Events

        #region Methods

        public void AddStatusMessage(string msg)
        {
            RunOnDispatcherThread(() =>
            {
                //insert at first
                StatusMessages.Insert(0, msg);
            });
        }

        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            App.AppManager.Config.Username = UserName;
            App.AppManager.Config.Password = Password;
            App.AppManager.SaveSettings();
        }

        internal void LoginTimerInit()
        {
            loginTimer = new DispatcherTimer();
            loginTimer.Tick += LoginTimer_Tick;
            loginTimer.Interval = TimeSpan.FromSeconds(1);
        }

        #endregion

    }
}