using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EZXApexRiskGUI.Properties;
using EZXLib;
using EZXWPFLibrary.Helpers;

namespace EZXApexRiskGUI.ViewModel
{
    public class ApplicationSettingsVM : ViewModelBase
    {
        private string company;
        private string host;
        private int port;
        private bool isSSL;
        private string sysLogHost;
        private string errorString;
        private bool applyLogSettingsForAllSessions;
        private ObservableCollection<LogAppenderInfo> logAppenderInfoList;
        private bool showChangeSettingPopup;


        public ICommand ApplySettingsCommand { get; private set; }

        public string Company
        {
            get => company;
            set { company = value; this.RaisePropertyChanged(vm => vm.Company); }
        }
        public string Host
        {
            get => host;
            set { host = value; this.RaisePropertyChanged(vm => vm.Host); }
        }
        public int Port
        {
            get => port;
            set { port = value; this.RaisePropertyChanged(vm => vm.Port); }
        }
        public bool IsSSL
        {
            get => isSSL;
            set { isSSL = value; this.RaisePropertyChanged(vm => vm.IsSSL); }
        }
        public string SysLogHost
        {
            get => sysLogHost;
            set
            {
                sysLogHost = value;
                this.RaisePropertyChanged(vm => vm.SysLogHost);
            }
        }
        public string ErrorString
        {
            get => errorString;
            set { errorString = value; this.RaisePropertyChanged(vm => vm.ErrorString); }
        }
        public bool ApplyLogSettingsForAllSessions
        {
            get => applyLogSettingsForAllSessions;
            set 
            { 
                applyLogSettingsForAllSessions = value; 
                this.RaisePropertyChanged(vm => vm.ApplyLogSettingsForAllSessions); 
            }
        }
        public ObservableCollection<LogAppenderInfo> LogAppenderInfoList
        {
            get => logAppenderInfoList;
            set { logAppenderInfoList = value; this.RaisePropertyChanged(vm => vm.LogAppenderInfoList); }
        }
        public bool ShowChangeSettingPopup
        {
            get => showChangeSettingPopup;
            set
            {
                showChangeSettingPopup = value;
                this.RaisePropertyChanged(vm => vm.ShowChangeSettingPopup);
            }
        }

        public ApplicationSettingsVM()
        {
            Logger.INFO("Start: ApplicationSettingsVM()");

            LoadComMgrConnectionInfo();
            LoadLogSetting();
            ApplySettingsCommand = new DelegateCommand(ApplySetting, param => true);

            Logger.INFO("Finished: ApplicationSettingsVM()");
        }

        private void LoadLogSetting()
        {            
            LogAppenderInfoList = new ObservableCollection<LogAppenderInfo>();
            foreach (KeyValuePair<string, LogAppenderInfo> logAppender in Logger.LogAppenders)
            {
                LogAppenderInfo logAppenderInfo = new LogAppenderInfo
                {
                    Name = logAppender.Value.Name, 
                    Enabled = logAppender.Value.Enabled, 
                    Level = logAppender.Value.Level
                };
                LogAppenderInfoList.Add(logAppenderInfo);
            }
            
            ApplyLogSettingsForAllSessions = AppManager.Config.ApplyLogSettingForAllSession;
            ShowChangeSettingPopup = AppManager.Config.ShowChangeSettingPopup;
            if (!string.IsNullOrEmpty(AppManager.Config.SysLogHost))
            {
                SysLogHost = AppManager.Config.SysLogHost;
            }
            else
            {
                SysLogHost = AppManager.Config.LSConnectionInfo.Host;
            }
        }

        private void LoadComMgrConnectionInfo()
        {
            Logger.INFO("Start: ApplicationSettingsVM.LoadComMgrConnectionInfo()");

            Company = App.AppManager.Config.LSConnectionInfo.Company;
            Host = App.AppManager.Config.LSConnectionInfo.Host;
            Port = App.AppManager.Config.LSConnectionInfo.Port;
            IsSSL = App.AppManager.Config.LSConnectionInfo.IsSSL;

            Logger.INFO("Finish: ApplicationSettingsVM.LoadComMgrConnectionInfo()");
        }

        private void SaveComMgrConnectionInfo()
        {
            Logger.INFO("Start: ApplicationSettingsVM.SaveComMgrConnectionInfo()");
            App.AppManager.Config.LSConnectionInfo.Company = Company;
            App.AppManager.Config.LSConnectionInfo.Host = Host;
            App.AppManager.Config.LSConnectionInfo.Port = Port;
            App.AppManager.Config.LSConnectionInfo.IsSSL = IsSSL;
            Logger.INFO("Finish: ApplicationSettingsVM.SaveComMgrConnectionInfo()");
        }

        public void ApplySetting(object parameter)
        {
            Logger.DEBUG("ApplicationSettingsVM.ApplySetting()...");

            if (!Validate())
            {
                return;
            }

            foreach (KeyValuePair<string, LogAppenderInfo> logAppender in Logger.LogAppenders)
            {
                LogAppenderInfo logAppenderInfo = LogAppenderInfoList.Where(log => log.Name == logAppender.Value.Name).FirstOrDefault();
                if (logAppenderInfo != null)
                {
                    logAppender.Value.Name = logAppenderInfo.Name;
                    logAppender.Value.Level = logAppenderInfo.Level;
                    logAppender.Value.Enabled = logAppenderInfo.Enabled;
                }
            }

            SaveLogSetting();
            SaveComMgrConnectionInfo();
        }

        private void SaveLogSetting()
        {
            Dictionary<string, LogAppenderInfo> lais = Logger.LogAppenders;
            foreach (LogAppenderInfo logAppenderInfo in LogAppenderInfoList.ToList())
            {
                string loggerName = logAppenderInfo.Name;
                if (!string.IsNullOrEmpty(loggerName) && lais.ContainsKey(loggerName))
                {
                    LogAppenderInfo lai = lais[loggerName];
                    lai.Enabled = logAppenderInfo.Enabled;
                    string level = logAppenderInfo.Level;
                    if (!string.IsNullOrEmpty(level))
                    {
                        lai.Level = level;
                    }
                }
            }
            Logger.LogAppenders = lais;

            if (ApplyLogSettingsForAllSessions)
            {
                Logger.PersistLogSettings();
            }

            AppManager.Config.ApplyLogSettingForAllSession = ApplyLogSettingsForAllSessions;
            AppManager.Config.ShowChangeSettingPopup = ShowChangeSettingPopup;
            AppManager.Config.SysLogHost = SysLogHost;
        }

        private bool Validate()
        {
            Logger.DEBUG("ApplicationSettingsVM.Validate()...");

            ErrorString = string.Empty;
            List<string> erros = new List<string>();
            if (string.IsNullOrEmpty(Company))
            {
                erros.Add(Resources.LoginCompanyError);
            }
            if (string.IsNullOrEmpty(Host))
            {
                erros.Add(Resources.LoginHostError);
            }
            if (Port == 0)
            {
                erros.Add(Resources.LoginPortValueError);
            }
            if (erros.Count > 0)
            {
                ErrorString = (string.Join("\n", erros.ToArray()));
                return false;
            }
            return true;
        }
    }
}
