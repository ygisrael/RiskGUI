using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXLightspeedGUI.Config;
using EZXWPFLibrary.Helpers;
using EZXLib;
using System.Reflection;
using EZX.LightSpeedEngine;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using EZXLightspeedGUI.Model;

namespace EZXLightspeedGUI
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


        private LSConfigInfo config;
        private GUILSEngine guiLSEngine;
        private bool isNodeStartDraging;
        private ObservableCollection<WashCheckComboData> washCheckList;

        public LSConfigInfo Config
        {
            get { return config; }
            set 
            { 
                config = value;
                this.RaisePropertyChanged(p => p.Config);
            }
        }
        public GUILSEngine GUILSEngine
        {
            get { return guiLSEngine; }
            set
            {
                guiLSEngine = value;
                this.RaisePropertyChanged(p => p.GUILSEngine);
            }
        }
        public bool IsNodeStartDraging
        {
            get { return isNodeStartDraging; }
            set
            {
                isNodeStartDraging = value;
                this.RaisePropertyChanged(p => p.IsNodeStartDraging);
            }
        }
        public ObservableCollection<WashCheckComboData> WashCheckList
        {
            get { return washCheckList; }
            set
            {
                washCheckList = value;
                this.RaisePropertyChanged(p => p.WashCheckList);
            }
        }


        public virtual List<string> LogCategoryList { get; set; }

        
        public ApplicationManager()
        {
            Logger.Init("eval.ezxinc.com", "EZXLightsppedGUI", "EZXLightsppedGUI", "userName", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            
            Logger.DEBUG("ApplicationManager() ...");

            currentDispatcher = Dispatcher.CurrentDispatcher;


            InitializeLogCategory();

            this.Config = XmlHelper.ReadFromFile<LSConfigInfo>(LSConfigInfo.FileNameWithPath);
            if (this.Config == null)
            {
                this.Config = new LSConfigInfo();
                if (MOCK_MODE)
                {
                    this.Config.Username = "mockusername";
                    this.Config.Password = "mockusername";
                    this.Config.LSConnectionInfo = new EZX.LightSpeedEngine.Config.LSConnectionInfo("TESTCOMPANY", "test.asa.assa", 1000, false);
                    this.Config.ApplyLogSettingForAllSession = true;
                    this.Config.ShowChangeSettingPopup = false;
                }
            }

            this.GUILSEngine = new GUILSEngine();
            ILSCommunicationManager comMgr;          

            if (MOCK_MODE)
            {
                comMgr = new EZX.LightspeedMockup.Mock.MockLSCommunicationManager(this.GUILSEngine); 
            }
            else
            {
                comMgr = new LSCommunicationManager(this.GUILSEngine);
            }

            if (this.GUILSEngine.Init(Config, comMgr))
            {
                this.GUILSEngine.SessionId = Guid.NewGuid().ToString();
            }
            else
            {
                throw new Exception("Problem to load LSEngine Instance in ApplicationManager()");
            }

            this.WashCheckList = new ObservableCollection<WashCheckComboData>();
            foreach (FieldInfo field in typeof(WashTradeCheck).GetFields())
            {
                string fieldValue = field.GetRawConstantValue().ToString();
                string displayName = WashCheckComboData.GetTextFormRersources(fieldValue);
                this.WashCheckList.Add(new WashCheckComboData(displayName, fieldValue));
            }
        }

        private void InitializeLogCategory()
        {
            LogCategoryList = new List<string>();
            LogCategoryList.Add(LogCategory.DEBUG.ToString());
            LogCategoryList.Add(LogCategory.INFO.ToString());
            LogCategoryList.Add(LogCategory.WARN.ToString());
            LogCategoryList.Add(LogCategory.ERROR.ToString());
            LogCategoryList.Add(LogCategory.FATAL.ToString());
            LogCategoryList.Add(LogCategory.ALL.ToString());
            LogCategoryList.Add(LogCategory.OFF.ToString());
        }

        public void SaveSettings()
        {
            EZXLib.Logger.DEBUG("ApplicationManager.SaveSettings() Started");
            this.Config.IsAlphabetize = this.GUILSEngine.DataManager.IsAlphabetize;
            XmlHelper.WriteToFile<LSConfigInfo>(Config, LSConfigInfo.FileNameWithPath);
            App.AppManager.GUILSEngine.DataManager.SaveGroupAccountConfig(string.Empty);
            EZXLib.Logger.DEBUG("ApplicationManager.SaveSettings() finished");
        }

        Dispatcher currentDispatcher = null;
        public void RunOnDispatcherThread(Action action)
        {
            Logger.DEBUG("RunOnDispatcherThread()...");

            App.Current.Dispatcher.BeginInvoke(action, null);
        }

        public void ThrowException(Exception exp)
        {
            if (this.ErrorOccurred != null)
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
                this.ErrorOccurred(this, e);
            }
        }

        public void AddingNewGroupAccount()
        {
            if (this.AddingGroupAccount != null)
            {
                this.AddingGroupAccount(this, null);
            }
        }

        public void KeyPressedToOpenContextMenu()
        {
            if (this.ContextMenuOpeningThrougKey != null)
            {
                this.ContextMenuOpeningThrougKey(this, null);
            }
        }

    }
}
