using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EZXLightspeedGUI;
using EZXLightspeedGUI.ViewModel;
using System.Threading;

namespace EZXLightspeedGUITest.ViewModel
{
    [TestClass()]
    public class LoginVMTest
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
        public void DoLoginTest()
        {
            appManager.Config.LSConnectionInfo.Company = "TESTCOMPANY";
            appManager.Config.LSConnectionInfo.Host = "mock.mock.mock";
            LoginVM vm = new LoginVM();
            int countStatus = vm.StatusMessages.Count;
            vm.DoLogin();
            string connectText = vm.StatusMessages[0];
            bool expected = true;
            bool actual = false;
            if (connectText.Contains("Login Complete"))
            {
                actual = true;
            }
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void DoLogin_WhenConnectionErrorOccurredTest()
        {
            appManager.Config.LSConnectionInfo.Company = "TESTCOMPANY";
            appManager.Config.LSConnectionInfo.Host = "mock.mock.mock1";
            LoginVM vm = new LoginVM();
            int countStatus = vm.StatusMessages.Count;
            vm.DoLogin();
            string connectText = vm.StatusMessages[0];
            bool expected = true;
            bool actual = false;
            if (connectText.Contains("Failed"))
            {
                actual = true;
            }
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DoLogin_WhenCompanyIsInValidTest()
        {
            appManager.Config.LSConnectionInfo.Company = "TEST";
            appManager.Config.LSConnectionInfo.Host = "mock.mock.mock";
            LoginVM vm = new LoginVM();
            int countStatus = vm.StatusMessages.Count;
            vm.DoLogin();
            string connectText = vm.StatusMessages[0];
            bool expected = true;
            bool actual = false;
            if (connectText.Contains("Connection Failed for"))
            {
                actual = true;
            }
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void DoLogin_ErrorConnectedTest()
        {

            LoginVM vm = new LoginVM();
            int countStatus = vm.StatusMessages.Count;
            App.AppManager.GUILSEngine.ConfigInfo.LSConnectionInfo.Company = "TEST";
            appManager.Config.LSConnectionInfo.Host = "mock.mock.mock";
            vm.DoLogin();
            string connectText = vm.StatusMessages[0];
            bool expected = true;
            bool actual = connectText.Contains("Failed");
            Assert.AreEqual(expected, actual);
        }
    }
}
