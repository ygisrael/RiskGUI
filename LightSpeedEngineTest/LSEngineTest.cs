using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EZX.LightSpeedEngine;
using EZX.LightSpeedEngine.Config;
using EZX.LightspeedMockup.Mock;
using System.Threading;
using EZX.LightspeedEngine.Entity;
using EZXLib;

namespace LightSpeedEngineTest
{
    [TestClass]
    public class LSEngineTest
    {
        ConfigInfo config;
        LSEngine lsEngine;
        MockLSCommunicationManager lsComMgr;


        [TestInitialize]
        public void Setup()
        {
            config = new ConfigInfo();
            config.LSConnectionInfo.Company = "TESTCOMPANY";
            config.LSConnectionInfo.Host = "mock.mock.mock";
            config.LSConnectionInfo.Port = 15000;
            config.LSConnectionInfo.IsSSL = false;
            config.Username = "testuser";
            config.Password = "testuser";

            lsEngine = new LSEngine();
            MockLSCommunicationManager lsComMgr = new MockLSCommunicationManager(lsEngine);
            lsEngine.Init(config, lsComMgr);
        }


        [TestMethod]
        public void ConstructerTest()
        {
            LSEngine lsEngine1 = new LSEngine();
            Assert.IsNull(lsEngine1.LSComMgr);
            Assert.IsNull(lsEngine1.DataManager);
        }

        [TestMethod]
        public void InitTest()
        {
            ConfigInfo config1 = new ConfigInfo();
            LSEngine lsEngineMockUp1 = new LSEngine();
            MockLSCommunicationManager lsComMgr = new MockLSCommunicationManager(lsEngineMockUp1);
            
            bool status1 = lsEngineMockUp1.Init(config1, lsComMgr);
            Assert.IsNotNull(lsEngineMockUp1.LSComMgr);
            Assert.IsNotNull(lsEngineMockUp1.DataManager);
            Assert.IsTrue(status1);

            LSEngine lsEngine1 = new LSEngine();

            status1 = lsEngine1.Init(config1, new LSCommunicationManager(lsEngine1));
            Assert.IsNotNull(lsEngine1.LSComMgr);
            Assert.IsNotNull(lsEngine1.DataManager);
            Assert.IsTrue(status1);

            config1.LSConnectionInfo = null;
            LSEngine lsEngine2 = new LSEngine();
            status1 = lsEngine2.Init(config1, new LSCommunicationManager(lsEngine2));
            Assert.IsFalse(status1);
        }


        bool isConnectedEventCalled = false;
        [TestMethod]
        public void ConnectTest()
        {
            isConnectedEventCalled = false;
            lsEngine.Connected += new ConnectHandler(lsEngine_Connected);
            bool statusConnect = lsEngine.Connect();
            Assert.IsTrue(statusConnect);
            Assert.IsTrue(isConnectedEventCalled);
        }
        void lsEngine_Connected(object sender, LSConnectionEventArgs e)
        {
            isConnectedEventCalled = true;
        }

        [TestMethod]
        public void ConnectErrorTest()
        {
            isConnectedEventCalled = true;
            lsEngine.ConnectErrorOccured+=new ConnectErrorOccuredHandler(lsEngine_ConnectErrorOccured);
            lsEngine.LSComMgr.ConnectionInfo.Host = "Test";
            lsEngine.LSComMgr.ConnectionInfo.Company = "TESTCOMPANY";
            bool statusConnect = lsEngine.Connect();
            //Assert.IsFalse(statusConnect);
            Assert.IsFalse(isConnectedEventCalled);

            lsEngine.LSComMgr.ConnectionInfo.Host = "mock.mock.mock";

        }

        void lsEngine_ConnectErrorOccured(object sender, LSConnectionEventArgs e)
        {
            isConnectedEventCalled = false;
        }


        [TestMethod]
        public void ConnectToEVALServerTest()
        {
            ConfigInfo config = new ConfigInfo();
            LSEngine lsEngine1 = new LSEngine();

            config.Username = "gnadan";
            config.Password = "gnadan";

            config.LSConnectionInfo = new LSConnectionInfo() { Company = "EZX", Host = "eval.ezxinc.com", Port = 15000, IsSSL = false };
            LSCommunicationManager lsComMgr2 = new LSCommunicationManager(lsEngine1);

            lsEngine1.Init(config, lsComMgr2);
            isConnectedEventCalled = false;            

            lsEngine1.Connected += new ConnectHandler(lsEngine1_Connected2);
            lsEngine1.ConnectErrorOccured += new ConnectErrorOccuredHandler(lsEngine1_ConnectErrorOccured);

            bool statusConnect = lsEngine1.Connect();
            Assert.IsTrue(statusConnect);
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(isConnectedEventCalled);

        }

        void lsEngine1_ConnectErrorOccured(object sender, LSConnectionEventArgs e)
        {
            isConnectedEventCalled = false;
        }
        void lsEngine1_Connected2(object sender, LSConnectionEventArgs e)
        {
            isConnectedEventCalled = true;
        }


        [TestMethod]
        public void ConnectToEVALServer_InvalidHostTest()
        {
            LSEngine lsEngine1 = new LSEngine();
            ConfigInfo config = new ConfigInfo();
            config.LSConnectionInfo = new LSConnectionInfo() { Company = "EZX", Host = "test123.ezxinc.com", Port = 15000, IsSSL = false };
            LSCommunicationManager lsComMgr2 = new LSCommunicationManager(lsEngine1);
            
            isConnectedEventCalled = true;
            lsEngine1.Init(config, lsComMgr2);
            lsEngine1.Connected += new ConnectHandler(lsEngine1_Connected3);
            lsEngine1.ConnectErrorOccured += new ConnectErrorOccuredHandler(lsEngine1_ConnectErrorOccured3);

            bool statusConnect = lsEngine1.Connect();
            Assert.IsTrue(statusConnect);
            System.Threading.Thread.Sleep(1000);
            Assert.IsFalse(isConnectedEventCalled);
        }

        void lsEngine1_ConnectErrorOccured3(object sender, LSConnectionEventArgs e)
        {
            isConnectedEventCalled = false;
        }

        void lsEngine1_Connected3(object sender, LSConnectionEventArgs e)
        {
            isConnectedEventCalled = true;
        }


        bool isLoginCompletedCalled = false;
        [TestMethod]
        public void LogonTest()
        {
            isLoginCompletedCalled = false;
            lsEngine.LoginCompleted += new LoginCompleteHandler(lsEngine_LoginCompleted);
            lsEngine.Connect();
            Thread.Sleep(1000);
            Assert.IsTrue(isLoginCompletedCalled, "Not received logon-completed after connect! ");
        }

        void lsEngine_LoginCompleted(object sender, LSConnectionEventArgs e)
        {
            isLoginCompletedCalled = true;
        }

        [TestMethod]
        public void LogonEVALTest()
        {
            ConfigInfo config = new ConfigInfo();
            LSEngine lsEngine1 = new LSEngine();

            config.Username = "gnadan";
            config.Password = "gnadan";
            LSCommunicationManager lsComMgr2 = new LSCommunicationManager(lsEngine1);
            config.LSConnectionInfo = new LSConnectionInfo() { Company = "EZX", Host = "eval.ezxinc.com", Port = 15000, IsSSL = false };

            lsEngine1.Init(config, lsComMgr2);
            isConnectedEventCalled = false;

            lsEngine1.LoginCompleted += new LoginCompleteHandler(lsEngine1_LoginCompleted2);

            bool statusConnect = lsEngine1.Connect();
            Assert.IsTrue(statusConnect);
            System.Threading.Thread.Sleep(8000);
            Assert.IsTrue(isLoginCompletedCalled);

            Assert.IsTrue(isLoginCompletedCalled, "Not received logon-completed after connect! ");
        }
        void lsEngine1_LoginCompleted2(object sender, LSConnectionEventArgs e)
        {
            isLoginCompletedCalled = true;
        }

        [TestMethod]
        public void onUpdateGroupSettingsTest()
        {

            string groupId = "g1";
            string groupName = "Group1";
            string creditLimit = "200000";
            string maxDuplicate = "30,30000";
            string maxAmount = "400000";
            string maxpriceDiff = "50";
            string myId = "1212121212";
            int returnCode = 0;
            string returnDesc = string.Empty;

            UpdateGroupSettings updateGroupSettingsResponse = CreateAndGetUpdateSettingObj(groupId, groupName, creditLimit, maxDuplicate, maxAmount, maxpriceDiff, myId, returnCode, returnDesc);

            Group g1 = new Group();
            g1.Id = groupId;
            g1.Name = groupName;
            g1.AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>();
            g1.RiskSetting = new RiskSetting();
            //g1.RiskSetting.CreditLimit = 100000;
            g1.RiskSetting.ClientCreditLimit = 200000;
            g1.RiskSetting.MaxDuplicateOrder = 10;
            g1.RiskSetting.DuplicateOrderTimeInterval = 10000;
            g1.RiskSetting.MaxNotionalPerOrder = 100000;
            g1.RiskSetting.MaxPriceDiff = 10;
            this.lsEngine.DataManager.AccountGroupList = new EZXWPFLibrary.Helpers.MTObservableCollection<INodeEntity>();
            
            this.lsEngine.DataManager.AccountGroupList.Add(g1);
            this.lsEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(updateGroupSettingsResponse.GroupAccount);

            this.lsEngine.onUpdateGroupSettings(updateGroupSettingsResponse);

            Group modifiedSettingGroup = this.lsEngine.DataManager.AccountGroupList[0] as Group;
            Assert.IsNotNull(modifiedSettingGroup);
            Assert.IsNotNull(modifiedSettingGroup.RiskSetting);
            Assert.AreEqual(200000,modifiedSettingGroup.RiskSetting.CreditLimit);
            Assert.AreEqual(30, modifiedSettingGroup.RiskSetting.MaxDuplicateOrder);
            Assert.AreEqual(30000, modifiedSettingGroup.RiskSetting.DuplicateOrderTimeInterval);
            Assert.AreEqual(400000, modifiedSettingGroup.RiskSetting.MaxNotionalPerOrder);
            Assert.AreEqual(50, modifiedSettingGroup.RiskSetting.MaxPriceDiff);
        }

        [TestMethod]
        public void onUpdateGroupNameTest()
        {

            string groupId = "g1";
            string groupName = "Group1Ren";

            string myId = "1212121212";
            int returnCode = 0;
            string returnDesc = string.Empty;

            UpdateGroupName updateGroupNameResponse = CreateAndGetUpdateGroupNameObj(groupId, groupName, myId, returnCode, returnDesc);

            Group g1 = new Group();
            g1.Id = groupId;
            g1.Name = "Group1";
            g1.AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>();
            g1.RiskSetting = new RiskSetting();
            g1.RiskSetting.CreditLimit = 100000;
            g1.RiskSetting.MaxDuplicateOrder = 10;
            g1.RiskSetting.DuplicateOrderTimeInterval = 10000;
            g1.RiskSetting.MaxNotionalPerOrder = 100000;
            g1.RiskSetting.MaxPriceDiff = 10;
            this.lsEngine.DataManager.AccountGroupList = new EZXWPFLibrary.Helpers.MTObservableCollection<INodeEntity>();

            this.lsEngine.DataManager.AccountGroupList.Add(g1);
            this.lsEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(updateGroupNameResponse.GroupAccount);

            this.lsEngine.onUpdateGroupName(updateGroupNameResponse);

            Group modifiedGroup = this.lsEngine.DataManager.AccountGroupList[0] as Group;
            Assert.IsNotNull(modifiedGroup);
            Assert.AreEqual(groupName, modifiedGroup.Name);
        }

        private UpdateGroupName CreateAndGetUpdateGroupNameObj(string groupId, string groupName, string myId, int returnCode, string returnDesc)
        {
            UpdateGroupName updateGroupNameResponse = new UpdateGroupName();
            updateGroupNameResponse.ReturnCode = returnCode;
            updateGroupNameResponse.ReturnDesc = returnDesc;
            updateGroupNameResponse.MyID = myId;

            updateGroupNameResponse.GroupAccount = new GroupAccount();
            updateGroupNameResponse.GroupAccount.Id = groupId;
            updateGroupNameResponse.GroupAccount.DisplayName = groupName;
            updateGroupNameResponse.GroupAccount.OwnerID = null;
            updateGroupNameResponse.GroupAccount.Settings = new Properties();
            updateGroupNameResponse.GroupAccount.Settings.PropertyMap = new TagValueMsg();
            updateGroupNameResponse.GroupAccount.Settings.PropertyMap.tagValues = new System.Collections.Hashtable();

            return updateGroupNameResponse;
        }

        
        private static UpdateGroupSettings CreateAndGetUpdateSettingObj(string groupId, string groupName, string creditLimit, string maxDuplicate, string maxAmount, string maxpriceDiff, string myId, int returnCode, string returnDesc)
        {
            UpdateGroupSettings updateGroupSettingsResponse = new UpdateGroupSettings();
            updateGroupSettingsResponse.ReturnCode = returnCode;
            updateGroupSettingsResponse.ReturnDesc = returnDesc;
            updateGroupSettingsResponse.MyID = myId;

            updateGroupSettingsResponse.GroupAccount = new GroupAccount();
            updateGroupSettingsResponse.GroupAccount.Id = groupId;
            updateGroupSettingsResponse.GroupAccount.DisplayName = groupName;
            updateGroupSettingsResponse.GroupAccount.OwnerID = null;
            updateGroupSettingsResponse.GroupAccount.Settings = new Properties();
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap = new TagValueMsg();
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues = new System.Collections.Hashtable();
            //updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.CREDIT_LIMIT_DBL, creditLimit);
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.CLIENT_CREDIT_LIMIT_DBL, creditLimit);
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_DUPES_CSV, maxDuplicate);
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_NOTIONAL_INT, maxAmount);
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_PRICE_DIFF_PCT, maxpriceDiff);
            return updateGroupSettingsResponse;
        }
    }
}
