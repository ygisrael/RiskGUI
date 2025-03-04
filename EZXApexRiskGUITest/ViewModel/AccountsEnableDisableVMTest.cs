using EZXApexRiskGUI;
using EZXApexRiskGUI.ViewModel;
using EZXLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Assert = NUnit.Framework.Assert;

namespace EZXApexRiskGUITest.ViewModel
{
    [TestFixture, Apartment(ApartmentState.STA)]
    public class AccountsEnableDisableVMTest
    {
        private object expected;
        private object actual;
        private ApplicationManager appManager;

        [OneTimeSetUp]
        public void Setup()
        {
            appManager = new ApplicationManager();
            
            if (Application.Current == null)
            {
                App testApplication = new App();
                testApplication.InitializeComponent();
                
                Application.Current?.Resources.Add("AppManager", appManager);
            }

            if (Application.Current != null) Application.Current.Resources["AppManager"] = appManager;

            ApplicationManager.MOCK_MODE = true;

            CreateAccountList();
        }

        private void CreateAccountList()
        {
            Properties properties = new Properties
            {
                PropertyMap = new TagValueMsg { tagValues = { ["TRADEONOFF"] = 1 } }
            };

            App.AppManager.GUILSEngine.DataManager.AddNewGroupAccount(new CreateGroupAccount() { GroupAccount = new GroupAccount { Id = "SCG111", Settings = properties }, ReturnCode = 0 });
            App.AppManager.GUILSEngine.DataManager.AddNewGroupAccount(new CreateGroupAccount() { GroupAccount = new GroupAccount { Id = "SCG222", Settings = properties }, ReturnCode = 0 });
            App.AppManager.GUILSEngine.DataManager.AddNewGroupAccount(new CreateGroupAccount() { GroupAccount = new GroupAccount { Id = "SCG333", Settings = properties }, ReturnCode = 0 });
            App.AppManager.GUILSEngine.DataManager.AddNewGroupAccount(new CreateGroupAccount() { GroupAccount = new GroupAccount { Id = "ACG111", Settings = properties }, ReturnCode = 0 });
            App.AppManager.GUILSEngine.DataManager.AddNewGroupAccount(new CreateGroupAccount() { GroupAccount = new GroupAccount { Id = "ACG222", Settings = properties }, ReturnCode = 0 });
            App.AppManager.GUILSEngine.DataManager.AddNewGroupAccount(new CreateGroupAccount() { GroupAccount = new GroupAccount { Id = "ACG333", Settings = properties }, ReturnCode = 0 });
        }

        [Test]
        public void ApplicationSettingsViewVMConstructorTest()
        {
            AccountsEnableDisableVM VM = new AccountsEnableDisableVM();
            Assert.IsNotNull(VM.AccountsList);
        }

        [Test]
        public void LoadAccountsTest()
        {
            App.AppManager.GUILSEngine.ApexFilterList = new List<string> { "SCG%" };

            AccountsEnableDisableVM target = new AccountsEnableDisableVM();
            PrivateObject obj = new PrivateObject(target);
            
            obj.Invoke("LoadAccounts");
            actual = target.accList.Count;
            expected = 3;
            Assert.AreEqual(expected, actual);

            App.AppManager.GUILSEngine.ApexFilterList.Add("ACG%");
            obj.Invoke("LoadAccounts");
            actual = target.accList.Count;
            expected = 6;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CanDisableSelectedAccountsTest()
        {
            App.AppManager.GUILSEngine.ApexFilterList = new List<string> { "SCG%" ,"ACG%"};

            AccountsEnableDisableVM target = new AccountsEnableDisableVM();
            PrivateObject obj = new PrivateObject(target);
            obj.Invoke("LoadAccounts");

            target.AccountsList[0].IsItemSelected = true;
            target.AccountsList[0].IsEnabled = true;
            Assert.True((bool)obj.GetFieldOrProperty("CanDisableSelectedAccounts"));

            //Can disable multiple accounts if both enabled and disabled are selected
            target.AccountsList[1].IsItemSelected = true;
            target.AccountsList[1].IsEnabled = false;
            Assert.True((bool)obj.GetFieldOrProperty("CanDisableSelectedAccounts"));

            target.AccountsList[2].IsItemSelected = true;
            target.AccountsList[2].IsEnabled = true;
            Assert.True((bool)obj.GetFieldOrProperty("CanDisableSelectedAccounts"));
        }

        [Test]
        public void CanEnableSelectedAccountsTest()
        {
            App.AppManager.GUILSEngine.ApexFilterList = new List<string> { "SCG%" ,"ACG%"};

            AccountsEnableDisableVM target = new AccountsEnableDisableVM();
            PrivateObject obj = new PrivateObject(target);
            obj.Invoke("LoadAccounts");

            target.AccountsList[0].IsItemSelected = true;
            target.AccountsList[0].IsEnabled = false;
            Assert.True((bool)obj.GetFieldOrProperty("CanEnableSelectedAccounts"));

            target.AccountsList[1].IsItemSelected = true;
            target.AccountsList[1].IsEnabled = false;
            Assert.True((bool)obj.GetFieldOrProperty("CanEnableSelectedAccounts"));

            //Cannot enable multiple accounts if both enabled and disabled are selected
            target.AccountsList[2].IsItemSelected = true;
            target.AccountsList[2].IsEnabled = true;
            Assert.True(!(bool)obj.GetFieldOrProperty("CanEnableSelectedAccounts"));
        }

        [Test]
        public void DisableSelectedAccountsTest()
        {
            App.AppManager.GUILSEngine.ApexFilterList = new List<string> { "SCG%" ,"ACG%"};

            AccountsEnableDisableVM target = new AccountsEnableDisableVM();
            PrivateObject obj = new PrivateObject(target);
            obj.Invoke("LoadAccounts");

            target.AccountsList[0].IsItemSelected = true;
            target.AccountsList[0].IsEnabled = true;
            target.AccountsList[1].IsItemSelected = true;
            target.AccountsList[1].IsEnabled = true;

            obj.Invoke("DisableSelectedAccounts", true);

            Assert.True(!target.AccountsList[0].IsEnabled);
            Assert.True(!target.AccountsList[1].IsEnabled);
            Assert.True(!target.AccountsList[0].IsItemSelected);
            Assert.True(!target.AccountsList[1].IsItemSelected);
        }

        [Test]
        public void EnableSelectedAccountsTest()
        {
            App.AppManager.GUILSEngine.ApexFilterList = new List<string> { "SCG%" ,"ACG%"};

            AccountsEnableDisableVM target = new AccountsEnableDisableVM();
            PrivateObject obj = new PrivateObject(target);
            obj.Invoke("LoadAccounts");

            target.AccountsList[0].IsItemSelected = true;
            target.AccountsList[0].IsEnabled = false;
            target.AccountsList[1].IsItemSelected = true;
            target.AccountsList[1].IsEnabled = false;

            obj.Invoke("DisableSelectedAccounts", false);

            Assert.True(target.AccountsList[0].IsEnabled);
            Assert.True(target.AccountsList[1].IsEnabled);
            Assert.True(!target.AccountsList[0].IsItemSelected);
            Assert.True(!target.AccountsList[1].IsItemSelected);
        }

        [Test]
        public void RefreshAccountsTest()
        {
            App.AppManager.GUILSEngine.ApexFilterList = new List<string> { "SCG%" };

            AccountsEnableDisableVM target = new AccountsEnableDisableVM();
            PrivateObject obj = new PrivateObject(target);
            
            obj.Invoke("LoadAccounts");
            actual = target.accList.Count;
            expected = 3;
            Assert.AreEqual(expected, actual);

            App.AppManager.GUILSEngine.ApexFilterList.Add("ACG%");
            obj.Invoke("RefreshAccounts");
            actual = target.accList.Count;
            expected = 6;
            Assert.AreEqual(expected, actual);
        }
    }
}
