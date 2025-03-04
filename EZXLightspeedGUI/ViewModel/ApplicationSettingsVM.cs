using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZX.LightSpeedEngine.Config;
using System.Windows.Input;
using EZXWPFLibrary.Helpers;
using System.Collections.ObjectModel;
using EZXLib;

namespace EZXLightspeedGUI.ViewModel
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
            get { return company; }
            set { company = value; this.RaisePropertyChanged(vm => vm.Company); }
        }
        public string Host
        {
            get { return host; }
            set { host = value; this.RaisePropertyChanged(vm => vm.Host); }
        }
        public int Port
        {
            get { return port; }
            set { port = value; this.RaisePropertyChanged(vm => vm.Port); }
        }
        public bool IsSSL
        {
            get { return isSSL; }
            set { isSSL = value; this.RaisePropertyChanged(vm => vm.IsSSL); }
        }
        public string SysLogHost
        {
            get { return sysLogHost; }
            set
            {
                sysLogHost = value;
                this.RaisePropertyChanged(vm => vm.SysLogHost);
            }
        }
        public string ErrorString
        {
            get { return errorString; }
            set { errorString = value; this.RaisePropertyChanged(vm => vm.ErrorString); }
        }
        public bool ApplyLogSettingsForAllSessions
        {
            get { return applyLogSettingsForAllSessions; }
            set 
            { 
                applyLogSettingsForAllSessions = value; 
                this.RaisePropertyChanged(vm => vm.ApplyLogSettingsForAllSessions); 
            }
        }
        public ObservableCollection<LogAppenderInfo> LogAppenderInfoList
        {
            get { return logAppenderInfoList; }
            set { logAppenderInfoList = value; this.RaisePropertyChanged(vm => vm.LogAppenderInfoList); }
        }
        public bool ShowChangeSettingPopup
        {
            get { return showChangeSettingPopup; }
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
            this.ApplySettingsCommand = new DelegateCommand(ApplySetting, (param) => true);

            Logger.INFO("Finished: ApplicationSettingsVM()");
        }

        private void LoadLogSetting()
        {            
            LogAppenderInfoList = new ObservableCollection<LogAppenderInfo>();
            foreach (KeyValuePair<string, LogAppenderInfo> logAppender in Logger.LogAppenders)
            {
                LogAppenderInfo logAppenderInfo = new LogAppenderInfo();
                logAppenderInfo.Name = logAppender.Value.Name;
                logAppenderInfo.Enabled = logAppender.Value.Enabled;
                logAppenderInfo.Level = logAppender.Value.Level;
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

            this.Company = App.AppManager.Config.LSConnectionInfo.Company;
            this.Host = App.AppManager.Config.LSConnectionInfo.Host;
            this.Port = App.AppManager.Config.LSConnectionInfo.Port;
            this.IsSSL = App.AppManager.Config.LSConnectionInfo.IsSSL;


            Logger.INFO("Finish: ApplicationSettingsVM.LoadComMgrConnectionInfo()");
        }

        private void SaveComMgrConnectionInfo()
        {
            Logger.INFO("Start: ApplicationSettingsVM.SaveComMgrConnectionInfo()");
            App.AppManager.Config.LSConnectionInfo.Company = this.Company;
            App.AppManager.Config.LSConnectionInfo.Host = this.Host;
            App.AppManager.Config.LSConnectionInfo.Port = this.Port;
            App.AppManager.Config.LSConnectionInfo.IsSSL = this.IsSSL;
            Logger.INFO("Finish: ApplicationSettingsVM.SaveComMgrConnectionInfo()");
        }

        public void ApplySetting(object parameter)
        {
            EZXLib.Logger.DEBUG("ApplicationSettingsVM.ApplySetting()...");

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

            if (this.ApplyLogSettingsForAllSessions == true)
            {
                Logger.PersistLogSettings();
            }

            AppManager.Config.ApplyLogSettingForAllSession = ApplyLogSettingsForAllSessions;
            AppManager.Config.ShowChangeSettingPopup = ShowChangeSettingPopup;
            AppManager.Config.SysLogHost = SysLogHost;
        }

        private bool Validate()
        {
            EZXLib.Logger.DEBUG("ApplicationSettingsVM.Validate()...");

            ErrorString = string.Empty;
            List<string> erros = new List<string>();
            if (string.IsNullOrEmpty(Company))
            {
                erros.Add(Properties.Resources.LoginCompanyError);
            }
            if (string.IsNullOrEmpty(Host))
            {
                erros.Add(Properties.Resources.LoginHostError);
            }
            if (Port == 0)
            {
                erros.Add(Properties.Resources.LoginPortValueError);
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
