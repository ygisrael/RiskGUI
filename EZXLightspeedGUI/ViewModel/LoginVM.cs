using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using EZXWPFLibrary.Helpers;
using EZXLib;

namespace EZXLightspeedGUI.ViewModel
{
    public class LoginVM : ViewModelBase
    {
        string username;
        string password;
        bool isAuthenticated;
        bool isConnected;

        public bool IsConnected
        {
            get { return isConnected; }
            set 
            { 
                isConnected = value; 
                this.RaisePropertyChanged(vm => vm.IsConnected); 
            }
        }

        ObservableCollection<string> statusMessages = new ObservableCollection<string>();

        public string Password
        {
            get { return password; }
            set 
            { 
                password = value; this.RaisePropertyChanged(vm => vm.Password); 
            }
        }

        public string UserName
        {
            get { return username; }
            set { username = value; this.RaisePropertyChanged(vm => vm.UserName); }
        }
        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set 
            { 
                isAuthenticated = value; 
                this.RaisePropertyChanged(vm => vm.IsAuthenticated); 
            }
        }

        public ObservableCollection<string> StatusMessages
        {
            get { return statusMessages; }
            set { statusMessages = value; this.RaisePropertyChanged(vm => vm.statusMessages); }
        }

        public LoginVM()
        {
            EZXLib.Logger.DEBUG("LoginVM() started");

            if (!IsDesignMode)
            {
                UserName = App.AppManager.Config.Username;
                Password = App.AppManager.Config.Password;
                AddStatusMessage("Please enter your User ID and Password.");
                AttachEvents();
            }

            EZXLib.Logger.DEBUG("LoginViewVM() finished ");
        }

        private void AttachEvents()
        {
            EZXLib.Logger.DEBUG("LoginViewVM.AttachEvents() started");

            App.AppManager.GUILSEngine.Connected += new EZX.LightSpeedEngine.ConnectHandler(LSEngine_Connected);
            App.AppManager.GUILSEngine.ConnectErrorOccured += new EZX.LightSpeedEngine.ConnectErrorOccuredHandler(LSEngine_ConnectErrorOccured);
            App.AppManager.GUILSEngine.LoginCompleted += new EZX.LightSpeedEngine.LoginCompleteHandler(LSEngine_LoginCompleted);
            App.AppManager.GUILSEngine.Disconnected += new EZX.LightSpeedEngine.DisconnectHandler(LSEngine_Disconnected);
            //App.AppManager.LSEngine.DataMgr.ClientOrderProcessed += new DataManager.ClientOrderProcessedHandler(DataMgr_ClientOrderProcessed);

            EZXLib.Logger.DEBUG("LoginViewVM.AttachEvents() finished");
        }

        void LSEngine_Disconnected(object sender, EZX.LightSpeedEngine.LSConnectionEventArgs e)
        {
            EZXLib.Logger.DEBUG("LoginViewVM.LSEngine_Disconnected()...");
            string errMsg = string.Format(Properties.Resources.LoginDisconnectText, e.LsComMgr.ConnectionInfo.Host, e.LsComMgr.ConnectionInfo.Port, e.LsComMgr.ConnectionInfo.Company, e.LsComMgr.ConnectionInfo.IsSSL, DateTime.Now.ToString("HH:mm:ss"));
            AddStatusMessage("Connection Failed for " + e.LsComMgr.ConnectionInfo.Company + " (" + errMsg + ")");

            EZXLib.Logger.DEBUG(errMsg);

        }

        void LSEngine_LoginCompleted(object sender, EZX.LightSpeedEngine.LSConnectionEventArgs e)
        {
            EZXLib.Logger.DEBUG("LoginViewVM.LSEngine_LoginCompleted()...");
            AddStatusMessage(e.LsComMgr.ConnectionInfo.Company + " Login Completed.");
            OnLoginCompleted();
            
        }

        private void OnLoginCompleted()
        {
            IsConnected = true;
            App.AppManager.Config.lastLoginDate = DateTimeUtils.GetLoginDateStringFormat(DateTime.Now);
        }

        void LSEngine_ConnectErrorOccured(object sender, EZX.LightSpeedEngine.LSConnectionEventArgs e)
        {
            Logger.DEBUG("LSEngine_ConnectErrorOccurred()...");

            string errorMsg = string.Format(Properties.Resources.LoginFailedToConnectText, e.LsComMgr.ConnectionInfo.Host, e.LsComMgr.ConnectionInfo.Port, e.LsComMgr.ConnectionInfo.Company, e.LsComMgr.ConnectionInfo.IsSSL, DateTime.Now.ToString("HH:mm:ss"));
            AddStatusMessage(errorMsg);

        }

        void LSEngine_Connected(object sender, EZX.LightSpeedEngine.LSConnectionEventArgs e)
        {
            Logger.DEBUG("LoginVM.LSEngine_Connected()...");
            string errMsg = string.Format(Properties.Resources.LoginConnectText, e.LsComMgr.ConnectionInfo.Host, e.LsComMgr.ConnectionInfo.Port, DateTime.Now.ToString("HH:mm:ss"));
            AddStatusMessage("Connected " + e.LsComMgr.ConnectionInfo.Company + " (" + errMsg + ")");
            Logger.DEBUG(errMsg);
        }

        public void AddStatusMessage(string msg)
        {
            base.RunOnDispatcherThread(() =>
            {
                //insert at first
                StatusMessages.Insert(0, msg);
            });
        }

        public void DoLogin()
        {
            App.AppManager.Config.Username = this.UserName;
            App.AppManager.Config.Password = this.Password; 
            App.AppManager.GUILSEngine.Connect();

        }
    }
}
