using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Threading;
using EZX.LightSpeedEngine;
using EZX.LightSpeedEngine.Config;
using EZXLib;
using EZXWPFLibrary.Helpers;

namespace EZXApexRiskGUI
{
    public delegate void ErrorOccurredHandler(object sender, LightspeedExceptionEventAgrs e);
    public delegate void AddingGroupAccountHandler(object sender, LightspeedExceptionEventAgrs e);
    public delegate void ContextMenuKeyHandler(object sender, EventArgs e);

    public class ApplicationManager : ObservableBase
    {
        public event ErrorOccurredHandler ErrorOccurred;
        public event AddingGroupAccountHandler AddingGroupAccount;
        public event ContextMenuKeyHandler ContextMenuOpeningThrougKey;

        public static bool MOCK_MODE = false;
        public static bool TEST_MODE = false;
        

        private ConfigInfo config;
        private LSEngine guiLSEngine;
        private bool isNodeStartDraging;

        public ConfigInfo Config
        {
            get => config;
            set
            {
                config = value;
                this.RaisePropertyChanged(p => p.Config);
            }
        }
        public LSEngine GUILSEngine
        {
            get => guiLSEngine;
            set
            {
                guiLSEngine = value;
                this.RaisePropertyChanged(p => p.GUILSEngine);
            }
        }
        public bool IsNodeStartDraging
        {
            get => isNodeStartDraging;
            set
            {
                isNodeStartDraging = value;
                this.RaisePropertyChanged(p => p.IsNodeStartDraging);
            }
        }

        

        public virtual List<string> LogCategoryList { get; set; }


        public ApplicationManager()
        {
            Logger.Init("eval.ezxinc.com", "EZXLightsppedGUI", "EZXLightsppedGUI", "userName", Assembly.GetExecutingAssembly().GetName().Version.ToString());

            Logger.DEBUG("ApplicationManager() ...");

            currentDispatcher = Dispatcher.CurrentDispatcher;


            InitializeLogCategory();

            Config = XmlHelper.ReadFromFile<ConfigInfo>(ConfigInfo.FileNameWithPath);
            if (Config == null)
            {
                Config = new ConfigInfo();
                if (MOCK_MODE)
                {
                    Config.Username = "mockusername";
                    Config.Password = "mockusername";
                    Config.LSConnectionInfo = new LSConnectionInfo("TESTCOMPANY", "test.asa.assa", 1000, false);
                    Config.ApplyLogSettingForAllSession = true;
                    Config.ShowChangeSettingPopup = false;
                }
            }
        
        GUILSEngine = new LSEngine();
            ILSCommunicationManager comMgr;
            
            comMgr = new LSCommunicationManager(GUILSEngine);

            if (guiLSEngine.Init(Config, comMgr))
            {
                GUILSEngine.SessionId = Guid.NewGuid().ToString();
            }
            else
            {
                throw new Exception("Problem loading LSEngine Instance in ApplicationManager()");
            }


        }
        
        private void InitializeLogCategory()
        {
            LogCategoryList = new List<string>
            {
                LogCategory.DEBUG.ToString(),
                LogCategory.INFO.ToString(),
                LogCategory.WARN.ToString(),
                LogCategory.ERROR.ToString(),
                LogCategory.FATAL.ToString(),
                LogCategory.ALL.ToString(),
                LogCategory.OFF.ToString()
            };
        }

        public void SaveSettings()
        {
            Logger.DEBUG("ApplicationManager.SaveSettings() Started");
            Config.IsAlphabetize = GUILSEngine.DataManager.IsAlphabetize;
            XmlHelper.WriteToFile(Config, ConfigInfo.FileNameWithPath);
            App.AppManager.GUILSEngine.DataManager.SaveGroupAccountConfig(string.Empty);
            Logger.DEBUG("ApplicationManager.SaveSettings() finished");
        }

        Dispatcher currentDispatcher;
        public void RunOnDispatcherThread(Action action)
        {
            Logger.DEBUG("RunOnDispatcherThread()...");

            App.Current.Dispatcher.BeginInvoke(action, null);
        }

        public void ThrowException(Exception exp)
        {
            if (ErrorOccurred != null)
            {
                LightspeedExceptionEventAgrs e = new LightspeedExceptionEventAgrs();
                if (exp is LightspeedException)
                {
                    e.ExceptionMessage = exp.Message;
                    e.ExceptionType = (exp as LightspeedException).ExceptionType;
                }
                else
                {
                    e.ExceptionMessage = exp.StackTrace;
                    e.ExceptionType = LIGHTSPEED_EXCEPTION_TYPE.UNHANDELLED_EXCEPTION;
                }
                e.Exception = exp;
                ErrorOccurred(this, e);
            }
        }

        public void AddingNewGroupAccount()
        {
            if (AddingGroupAccount != null)
            {
                AddingGroupAccount(this, null);
            }
        }

        public void KeyPressedToOpenContextMenu()
        {
            if (ContextMenuOpeningThrougKey != null)
            {
                ContextMenuOpeningThrougKey(this, null);
            }
        }
       
    }
}
