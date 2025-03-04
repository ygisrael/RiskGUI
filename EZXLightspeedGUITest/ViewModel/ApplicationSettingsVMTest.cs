using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EZXLightspeedGUI;
using EZXLightspeedGUI.ViewModel;
using EZXLib;

namespace EZXLightspeedGUITest.ViewModel
{
    [TestClass()]
    public class ApplicationSettingsVMTest
    {
        ApplicationManager appManager;

        [TestInitialize]
        public void Setup()
        {
            ApplicationManager.MOCK_MODE = true;
            appManager = new ApplicationManager();

            EZXLightspeedGUI.App testApplication;

            if (App.Current == null)
            {
                testApplication = new App();
                testApplication.InitializeComponent();

                App.Current.Resources.Add("AppManager", appManager);
            }
            App.Current.Resources["AppManager"] = appManager;
        }

        [TestMethod]
        public void LoadComMgrConnectionInfoTest()
        {            
            ApplicationSettingsVM vm = new ApplicationSettingsVM();
            Assert.AreEqual(appManager.Config.LSConnectionInfo.Company, vm.Company);
            Assert.AreEqual(appManager.Config.LSConnectionInfo.Host, vm.Host);
            Assert.AreEqual(appManager.Config.LSConnectionInfo.Port, vm.Port);
            Assert.AreEqual(appManager.Config.LSConnectionInfo.IsSSL, vm.IsSSL);
        }

        //[TestMethod]
        public void LoadLogSettingTest()
        {
            ApplicationSettingsVM vm = new ApplicationSettingsVM();
            Assert.IsNotNull(vm.LogAppenderInfoList);
            Assert.AreEqual(3, vm.LogAppenderInfoList.Count);

            Assert.AreEqual(appManager.Config.ApplyLogSettingForAllSession, vm.ApplyLogSettingsForAllSessions);
            Assert.AreEqual(appManager.Config.SysLogHost, vm.SysLogHost);
        }

        [TestMethod]
        public void ApplySettingTest()
        {
            ApplicationSettingsVM vm = new ApplicationSettingsVM();
            vm.Company = "Test";
            vm.Host = "TestHost";
            vm.Port = 100;
            vm.IsSSL = false;

            for(int i=0; i< vm.LogAppenderInfoList.Count; i++)
            {
                vm.LogAppenderInfoList[i].Enabled = true;
                vm.LogAppenderInfoList[i].Level = "ERROR";
            }

            vm.ApplySetting(null);

            Assert.AreEqual(vm.Company, appManager.Config.LSConnectionInfo.Company);
            Assert.AreEqual(vm.Host, appManager.Config.LSConnectionInfo.Host);
            Assert.AreEqual(vm.Port, appManager.Config.LSConnectionInfo.Port);
            Assert.AreEqual(vm.IsSSL, appManager.Config.LSConnectionInfo.IsSSL);

            foreach (KeyValuePair<string, LogAppenderInfo> logAppender in Logger.LogAppenders)
            {
                if (logAppender.Value != null)
                {
                    Assert.AreEqual(true, logAppender.Value.Enabled);
                    Assert.AreEqual("ERROR", logAppender.Value.Level);
                }
            }

        }
    }
}
