using EZX.LightSpeedEngine;
using EZXApexRiskGUI.Model;
using EZXApexRiskGUI.View;
using EZXLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace EZXApexRiskGUI.ViewModel
{
    public class AccountsEnableDisableVM : ViewModelBase
    {
        #region Constructor
        
        public AccountsEnableDisableVM()
        {
            InitEvents();
            ApplicationTitle = string.Format(Properties.Resources.ApplicationTitle, Assembly.GetExecutingAssembly().GetName().Version);
            InitCommands();
            LoadAccounts();
        }
        
        private void InitEvents()
        {
            App.AppManager.GUILSEngine.LoggedOut += GUILSEngine_LoggedOut;
            App.AppManager.GUILSEngine.Disconnected += GUILSEngine_Disconnected;
            App.AppManager.GUILSEngine.Connected += GUILSEngine_Connected;
            App.AppManager.GUILSEngine.LoginCompleted += GUILSEngine_LoginCompleted;
            App.AppManager.ErrorOccurred += AppManager_ErrorOccurred;
            App.AppManager.GUILSEngine.LoadAllGroupAndAccountCompleted += GUILSEngine_LoadAllGroupAndAccountCompleted;
        }

        private void InitCommands()
        {
            EnableAccountCommand = new RelayCommand(param => DisableSelectedAccounts(false), param => CanEnableSelectedAccounts);
            DisableAccountCommand = new RelayCommand(param => DisableSelectedAccounts(true), param => CanDisableSelectedAccounts);
            RefreshAccountCommand = new RelayCommand(param => RefreshAccounts(), param => true);
            LogoutCommand = new RelayCommand(param => Logout(), param => true);
            ExitCommand = new RelayCommand(param => CloseApp(), param => true);
            AboutCommand = new RelayCommand(param => AboutWindowOpen(), param => true);
            SettingsCommand = new RelayCommand(param => SettingsWindowOpen(), param => true);
        }

        #endregion

        #region Properties

        private ObservableCollection<GroupAccountWPF> accountsList = new ObservableCollection<GroupAccountWPF>();
        public ObservableCollection<GroupAccountWPF> AccountsList
        {
            get => accountsList;
            set
            {
                accountsList = value;
                RaisePropertyChanged("AccountsList");
            }
        }

        public IEnumerable<GroupAccountWPF> SelectedAccounts => AccountsList.Where(account => account.IsItemSelected);

        private string applicationTitle;

        public string ApplicationTitle
        {
            get => applicationTitle;
            set
            {
                applicationTitle = value;
                RaisePropertyChanged("ApplicationTitle");
            }
        }

        private bool isControlEnabled = true;

        public bool IsControlEnabled
        {
            get => isControlEnabled;
            set
            {
                isControlEnabled = value;
                RaisePropertyChanged("IsControlEnabled");
            }
        }

        private bool? dialogResult;
        public bool? DialogResult
        {
            get => dialogResult;
            set
            {
                dialogResult = value;
                RaisePropertyChanged("DialogResult");
            }
        }
        #endregion

        #region Commands

        private bool CanEnableSelectedAccounts => SelectedAccounts.Any(account => !account.IsEnabled && !CanDisableSelectedAccounts);
        private bool CanDisableSelectedAccounts => SelectedAccounts.Any(account => account.IsEnabled);

        public RelayCommand EnableAccountCommand { get; private set; }
        public RelayCommand DisableAccountCommand { get; private set; }
        public RelayCommand RefreshAccountCommand { get; private set; }
        public RelayCommand LogoutCommand { get; private set; }
        public RelayCommand ExitCommand { get; private set; }
        public RelayCommand AboutCommand { get; private set; }
        public RelayCommand SettingsCommand { get; private set; }

        #endregion

        #region Methods

        public List<GroupAccount> accList;
        internal void LoadAccounts()
        {
            GroupAccountWPF_MockData md = new GroupAccountWPF_MockData();

            try
            {
                accountsList.Clear();
                /*
                accList = App.AppManager.GUILSEngine.DataManager.GetFilteredAccountList();
                foreach (GroupAccount groupAccount in accList)
                {
                    accountsList.Add(new GroupAccountWPF(groupAccount));
                }
                */

                accountsList = GroupAccountWPF_MockData.accountsList;

                Mouse.OverrideCursor = Cursors.Arrow;
                App.AppManager.GUILSEngine.StatusMessage = "Accounts loaded successfully.";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "EZX Apex Risk", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.ERROR($"LoadAccounts(): {ex}");
            }
        }

        private void ClearSelection()
        {
            foreach (GroupAccountWPF groupAccount in AccountsList.Where(gAccount => gAccount.IsItemSelected))
            {
                groupAccount.IsItemSelected = false;
            }
        }

        private void DisableSelectedAccounts(bool disable)
        {
            foreach (GroupAccountWPF groupAccount in SelectedAccounts.Where(gAccount => gAccount.IsEnabled == disable))
            {
                string tradeSetting = disable ? CompanySetting.TRADING_DISABLED : CompanySetting.TRADING_ENABLED;
                bool saveGroupAcc = App.AppManager.GUILSEngine.DataManager.SaveGroupAccount(groupAccount, tradeSetting) || ApplicationManager.MOCK_MODE;

                if (saveGroupAcc)
                {
                    groupAccount.IsEnabled = !disable;
                }
                else
                {
                    string msg = disable ? "Disabling" : "Enabling";
                    msg += $" of account {groupAccount.DisplayName} failed!\nPlease check connection to server.";
                    MessageBox.Show(msg, "EZX Apex Risk", MessageBoxButton.OK, MessageBoxImage.Error);
                    Logger.ERROR($"DisabledSelectedAccounts(): {msg}");
                    return;
                }
            }

            ClearSelection();
        }

        private void RefreshAccounts()
        {
            bool getGrpAccData = App.AppManager.GUILSEngine.GetGroupAccountData() || ApplicationManager.MOCK_MODE;
            if (getGrpAccData)
            {
                LoadAccounts();
            }
            else
            {
                string msg = "Refreshing accounts failed!\nPlease check connection to server.";
                MessageBox.Show(msg, "EZX Apex Risk", MessageBoxButton.OK, MessageBoxImage.Error);
                Logger.ERROR($"RefreshAccounts(): {msg}");
            }
        }

        private void Logout()
        {
            App.AppManager.GUILSEngine.Logout();
            PostLogout();
            Logger.DEBUG($"AccountsEnableDisableVM.Logout(). Return DialogResult={DialogResult}.");
        }

        private void CloseApp()
        {
            Application.Current.Shutdown();
        }

        private void AboutWindowOpen()
        {
            AboutWindow about = new AboutWindow();
            about.ShowDialog();
        }

        private void SettingsWindowOpen()
        {
            ApplicationSettingsView view = new ApplicationSettingsView();
            view.ShowDialog();
        }
        #endregion

        #region Init Events

        
        internal void GUILSEngine_LoadAllGroupAndAccountCompleted(object sender, LSConnectionEventArgs e)
        {
            App.AppManager.RunOnDispatcherThread(LoadAccounts);
        }

        private void GUILSEngine_LoginCompleted(object sender, LSConnectionEventArgs e)
        {
            App.AppManager.RunOnDispatcherThread(() =>
            {
                App.AppManager.GUILSEngine.LoggedIn = true;
            });
        }

        private void AppManager_ErrorOccurred(object sender, LightspeedExceptionEventAgrs e)
        {
            App.AppManager.RunOnDispatcherThread(() =>
            {
                if (e.ExceptionType == LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION)
                {
                    MessageBox.Show(e.ExceptionMessage, "Data Validation Error!", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                else if ((e.ExceptionType == LIGHTSPEED_EXCEPTION_TYPE.GUI_KNOWN))
                {
                    MessageBox.Show(e.ExceptionMessage, "Error occurred!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ExceptionView view = new ExceptionView(e.Exception);
                    view.ShowDialog();
                }
            });
        }

        private void PostLogout()
        {
            IsControlEnabled = false;
            LoginView view = LoginView.GetLoginView();
            view.ShowDialog();
            view.BringIntoView();
        }

        private void GUILSEngine_LoggedOut(object sender, LSConnectionEventArgs e)
        {
            //for some reason when logging out this is called twice, so changed PostLogout from Logout().
            //App.AppManager.RunOnDispatcherThread(PostLogout);
        }

        private void GUILSEngine_Connected(object sender, LSConnectionEventArgs e)
        {
            App.AppManager.GUILSEngine.IsConnected = true;
             App.AppManager.RunOnDispatcherThread(() => IsControlEnabled = true);
        }

        private void GUILSEngine_Disconnected(object sender, LSConnectionEventArgs e)
        {
            App.AppManager.GUILSEngine.IsConnected = false;
            App.AppManager.RunOnDispatcherThread(() => IsControlEnabled = false);

        }

        #endregion

        #region Close Window

        #endregion
    }

}
