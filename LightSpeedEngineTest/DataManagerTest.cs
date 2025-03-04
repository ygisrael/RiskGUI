using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EZX.LightSpeedEngine;
using EZX.LightspeedMockup.Mock;
using EZX.LightSpeedEngine.Config;
using EZXLib;
using LightSpeedEngineTest.Utils;
using EZX.LightspeedEngine.Entity;
using EZXWPFLibrary.Utils;

namespace LightSpeedEngineTest
{
    [TestClass]
    public class DataManagerTest
    {
        ConfigInfo config;
        LSEngine lsEngine;
        MockLSCommunicationManager lsComMgr;
        System.Collections.Hashtable riskSetting = new System.Collections.Hashtable();

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
            DataManager mgr1 = new DataManager(this.lsEngine);
            Assert.IsNotNull(mgr1.LSEngine);
        }

        [TestMethod]
        public void LoadAllGroupAndAccount_DefaultGroupTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            ReadGroupAccounts readGroupAccounts = AddGroupAccountDataInOrder();

            mgr1.LoadAllGroupAndAccount(readGroupAccounts);

            Assert.AreEqual("Default Group", mgr1.GetDefaultGroupNode().Name);
            Assert.AreEqual(true, mgr1.GetDefaultGroupNode().IsDefaultGroup);
        }

        [TestMethod]
        public void LoadAllGroupAndAccount_GroupAccountOrderedDataTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            ReadGroupAccounts readGroupAccounts = AddGroupAccountDataInOrder();

            mgr1.LoadAllGroupAndAccount(readGroupAccounts);
            
            int expectedGroupCount = 2;
            int actualGroupCount = mgr1.AccountGroupList.Count;


            Assert.AreEqual(expectedGroupCount, actualGroupCount, "Group count is mismatched");
            Assert.AreEqual(false, mgr1.FindGroupNode("g1").IsDefaultGroup);

            int expectedAccountCountInGroupA = 3;
            int actualAccountCountInGroupA = mgr1.FindGroupNode("g1").AccountList.Count;
            Assert.AreEqual(expectedAccountCountInGroupA, actualAccountCountInGroupA, "Account count in GroupA is mismatched");

            string groupAId = "g1";
            string actualAccount1ParentGroupId = mgr1.FindGroupNode(groupAId).AccountList[0].ParentGroup.Id;
            Assert.AreEqual(groupAId, actualAccount1ParentGroupId, "Account 1 parent Group Id is mismatched");

            string actualAccount2ParentGroupId = mgr1.FindGroupNode(groupAId).AccountList[1].ParentGroup.Id;
            Assert.AreEqual(groupAId, actualAccount2ParentGroupId, "Account 2 parent Group Id is mismatched");

            string actualAccount3ParentGroupId = mgr1.FindGroupNode(groupAId).AccountList[2].ParentGroup.Id;
            Assert.AreEqual(groupAId, actualAccount3ParentGroupId, "Account 3 parent Group Id is mismatched");

            int groupAAccountCount = 3;
            int actualAccount1ParentGroupAccountCount = mgr1.FindGroupNode(groupAId).AccountList[0].ParentGroup.AccountList.Count;
            int actualAccount2ParentGroupAccountCount = mgr1.FindGroupNode(groupAId).AccountList[1].ParentGroup.AccountList.Count;
            int actualAccount3ParentGroupAccountCount = mgr1.FindGroupNode(groupAId).AccountList[2].ParentGroup.AccountList.Count;

            Assert.AreEqual(groupAAccountCount, actualAccount1ParentGroupAccountCount, "Account 1 parentGroup accountList count is mismatched");
            Assert.AreEqual(groupAAccountCount, actualAccount2ParentGroupAccountCount, "Account 2 parentGroup accountList count is mismatched");
            Assert.AreEqual(groupAAccountCount, actualAccount3ParentGroupAccountCount, "Account 3 parentGroup accountList count is mismatched");
        }


        [TestMethod]
        public void LoadAllGroupAndAccount_GroupAccountNotOrderedDataTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            ReadGroupAccounts readGroupAccounts = AddGroupAccountDataNotInOrder();

            mgr1.LoadAllGroupAndAccount(readGroupAccounts);

            int expectedGroupCount = 2;
            int actualGroupCount = mgr1.AccountGroupList.Count;


            Assert.AreEqual(expectedGroupCount, actualGroupCount, "Group count is mismatched");
            Assert.AreEqual(false, mgr1.FindGroupNode("g1").IsDefaultGroup);

            int expectedAccountCountInGroupA = 3;
            int actualAccountCountInGroupA = mgr1.FindGroupNode("g1").AccountList.Count;
            Assert.AreEqual(expectedAccountCountInGroupA, actualAccountCountInGroupA, "Account count in GroupA is mismatched");

            string groupAId = "g1";
            string actualAccount1ParentGroupId = mgr1.FindGroupNode(groupAId).AccountList[0].ParentGroup.Id;
            Assert.AreEqual(groupAId, actualAccount1ParentGroupId, "Account 1 parent Group Id is mismatched");

            string actualAccount2ParentGroupId = mgr1.FindGroupNode(groupAId).AccountList[1].ParentGroup.Id;
            Assert.AreEqual(groupAId, actualAccount2ParentGroupId, "Account 2 parent Group Id is mismatched");

            string actualAccount3ParentGroupId = mgr1.FindGroupNode(groupAId).AccountList[2].ParentGroup.Id;
            Assert.AreEqual(groupAId, actualAccount3ParentGroupId, "Account 3 parent Group Id is mismatched");

            int groupAAccountCount = 3;
            int actualAccount1ParentGroupAccountCount = mgr1.FindGroupNode(groupAId).AccountList[0].ParentGroup.AccountList.Count;
            int actualAccount2ParentGroupAccountCount = mgr1.FindGroupNode(groupAId).AccountList[1].ParentGroup.AccountList.Count;
            int actualAccount3ParentGroupAccountCount = mgr1.FindGroupNode(groupAId).AccountList[2].ParentGroup.AccountList.Count;

            Assert.AreEqual(groupAAccountCount, actualAccount1ParentGroupAccountCount, "Account 1 parentGroup accountList count is mismatched");
            Assert.AreEqual(groupAAccountCount, actualAccount2ParentGroupAccountCount, "Account 2 parentGroup accountList count is mismatched");
            Assert.AreEqual(groupAAccountCount, actualAccount3ParentGroupAccountCount, "Account 3 parentGroup accountList count is mismatched");

        }

        [TestMethod]
        public void CreateNewGroup_WhenAccountAlreadyExistTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            mgr1.AccountGroupList = new EZXWPFLibrary.Helpers.MTObservableCollection<INodeEntity>();
            mgr1.TempAccountList = new List<Account>();
            Account account1 = GroupAccountUtil.CreateAccountWithoutParentGroup("ga1", "Account 1", "g1");
            Account account2 = GroupAccountUtil.CreateAccountWithoutParentGroup("ga1", "Account 1", "g1");
            mgr1.TempAccountList.Add(account1);
            mgr1.TempAccountList.Add(account2);

            GroupAccount groupAccountObj = new GroupAccount();
            groupAccountObj.OwnerID = null;
            groupAccountObj.Id = "g1";
            groupAccountObj.IsDefault = 0;
            groupAccountObj.DisplayName = "Group A";
            groupAccountObj.Settings = new Properties();

            mgr1.CreateNewGroupNode("d1", groupAccountObj, false);

            Assert.AreEqual(1, mgr1.AccountGroupList.Count,"Group Count is not matched");
            
            Group groupA = mgr1.AccountGroupList[0] as Group;
            
            int expectedAccountCount = 2;
            int actualAccountCount = groupA.AccountList.Count;
            Assert.AreEqual(expectedAccountCount, actualAccountCount, "GroupA AccountList Count is not matched");

            Account account1GroupA = groupA.AccountList[0];
            string expectedAccount1ParentId = groupA.Id;
            string actualAccount1ParentId = account1GroupA.ParentGroup.Id;
            Assert.AreEqual(expectedAccount1ParentId, actualAccount1ParentId, "Account 1 ParentId is not matched");

        }

        [TestMethod]
        public void CreateNewGroupAccount_AddingNewGroupTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);

            CreateGroupAccount newGroup = new CreateGroupAccount();
            newGroup.ReturnCode = 0;
            newGroup.ReturnDesc = string.Empty;
            newGroup.MyID = Guid.NewGuid().ToString();
            newGroup.GroupAccount = new GroupAccount();
            newGroup.GroupAccount.Id = "g100";
            newGroup.GroupAccount.DisplayName = "Group 100";
            newGroup.GroupAccount.OwnerID = null;
            newGroup.GroupAccount.Settings = new Properties();

            Assert.IsNotNull(mgr1.AccountGroupList);
            mgr1.AddNewGroupAccount(newGroup);
            Assert.AreEqual(1, mgr1.AccountGroupList.Count, "New Group not added succesfully!");
        }

        [TestMethod]
        public void CreateNewGroupAccount_AddingExistingGroupTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);

            CreateGroupAccount newGroup = new CreateGroupAccount();
            newGroup.ReturnCode = 0;
            newGroup.ReturnDesc = string.Empty;
            newGroup.MyID = Guid.NewGuid().ToString();
            newGroup.GroupAccount = new GroupAccount();
            newGroup.GroupAccount.Id = "g100";
            newGroup.GroupAccount.DisplayName = "Group 100";
            newGroup.GroupAccount.OwnerID = null;
            newGroup.GroupAccount.Settings = new Properties();

            Assert.IsNotNull(mgr1.AccountGroupList);
            mgr1.AccountGroupList.Add(new Group()
            { 
                Id = newGroup.GroupAccount.Id,
                Name = newGroup.GroupAccount.DisplayName,
                RiskSetting = new RiskSetting(),
                AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>()
            }); 
            mgr1.AddNewGroupAccount(newGroup);
            Assert.AreEqual(1, mgr1.AccountGroupList.Count, "Existing Group not added succesfully!");
        }

        [TestMethod]
        public void CreateNewGroupAccount_AddingNewAccountTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            mgr1.AccountGroupList.Add(new Group()
            {
                Id = "ga1",
                Name = "Group A",
                RiskSetting = new RiskSetting(),
                AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>()
            }); 

            CreateGroupAccount newAccount = new CreateGroupAccount();
            newAccount.ReturnCode = 0;
            newAccount.ReturnDesc = string.Empty;
            newAccount.MyID = Guid.NewGuid().ToString();
            newAccount.GroupAccount = new GroupAccount();
            newAccount.GroupAccount.Id = "ga100";
            newAccount.GroupAccount.DisplayName = "Account 100";
            newAccount.GroupAccount.OwnerID = "ga1";
            newAccount.GroupAccount.Settings = null;

            Assert.IsNotNull(mgr1.AccountGroupList);
            mgr1.AddNewGroupAccount(newAccount);
            Assert.AreEqual(1, mgr1.AccountGroupList.Count, "Group count is incorrect!");
            Assert.AreEqual(1, (mgr1.AccountGroupList[0] as Group).AccountList.Count, "New Account is not added successfully!");
        }

        [TestMethod]
        public void CreateNewGroupAccount_AddingNewAccountWhichGroupIsAddingAfterAccountTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);

            CreateGroupAccount newAccount = new CreateGroupAccount();
            newAccount.ReturnCode = 0;
            newAccount.ReturnDesc = string.Empty;
            newAccount.MyID = Guid.NewGuid().ToString();
            newAccount.GroupAccount = new GroupAccount();
            newAccount.GroupAccount.Id = "ga100";
            newAccount.GroupAccount.DisplayName = "Account 100";
            newAccount.GroupAccount.OwnerID = "ga1";
            newAccount.GroupAccount.Settings = null;

            Assert.IsNotNull(mgr1.AccountGroupList);
            mgr1.AddNewGroupAccount(newAccount);
            Assert.AreEqual(0, mgr1.AccountGroupList.Count, "Group count is incorrect!");

            CreateGroupAccount newGroup = new CreateGroupAccount();
            newGroup.ReturnCode = 0;
            newGroup.ReturnDesc = string.Empty;
            newGroup.MyID = Guid.NewGuid().ToString();
            newGroup.GroupAccount = new GroupAccount();
            newGroup.GroupAccount.Id = "ga1";
            newGroup.GroupAccount.OwnerID = null;
            newGroup.GroupAccount.DisplayName = "Group A";
            mgr1.AddNewGroupAccount(newGroup);

            Assert.AreEqual(1, mgr1.AccountGroupList.Count, "Group count is incorrect!");
            Assert.AreEqual(1, (mgr1.AccountGroupList[0] as Group).AccountList.Count, "New Account is not added successfully!");
        }

        [TestMethod]
        public void CreateNewGroupAccount_AddingNewStandAloneAccountTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            mgr1.AccountGroupList.Add(new Group()
            {
                Id = "ga1",
                Name = "Group A",
                RiskSetting = new RiskSetting(),
                AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>()
            });

            CreateGroupAccount newAccount = new CreateGroupAccount();
            newAccount.ReturnCode = 0;
            newAccount.ReturnDesc = string.Empty;
            newAccount.MyID = Guid.NewGuid().ToString();
            newAccount.GroupAccount = new GroupAccount();
            newAccount.GroupAccount.Id = "ga100";
            newAccount.GroupAccount.DisplayName = "Account 100";
            newAccount.GroupAccount.OwnerID = "ga100";
            newAccount.GroupAccount.Settings = new Properties();

            Assert.IsNotNull(mgr1.AccountGroupList);
            mgr1.AddNewGroupAccount(newAccount);
            Assert.AreEqual(2, mgr1.AccountGroupList.Count, "Group count is incorrect!");
            Assert.AreEqual("ga100", (mgr1.AccountGroupList[1] as Group).GroupAccount.Id, "New Stand-Alone Account is not added successfully!");
        }

        [TestMethod]
        public void CreateNewGroupAccount_AddingExistingStandAloneAccountTest()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            mgr1.AccountGroupList.Add(new Group()
            {
                Id = "ga1",
                Name = "Group A",
                RiskSetting = new RiskSetting(),
                AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>()
            });

            Group accountGroup =  new Group()
            {
                Id = "ga100",
                Name = "Account 100",
                RiskSetting = new RiskSetting(),
                AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>()
            };

            Account standAloneAccount = new Account();
            standAloneAccount.IsOwnGroup = true;
            standAloneAccount.Id = "ga100";
            standAloneAccount.Name = "Account 100";
            standAloneAccount.ParentGroup = accountGroup;
            accountGroup.AccountList.Add(standAloneAccount);
            accountGroup.IsAccountGroup = true;
            mgr1.AccountGroupList.Add(accountGroup);


            CreateGroupAccount newAccount = new CreateGroupAccount();
            newAccount.ReturnCode = 0;
            newAccount.ReturnDesc = string.Empty;
            newAccount.MyID = Guid.NewGuid().ToString();
            newAccount.GroupAccount = new GroupAccount();
            newAccount.GroupAccount.Id = "ga100";
            newAccount.GroupAccount.DisplayName = "Account 100";
            newAccount.GroupAccount.OwnerID = "ga100";
            newAccount.GroupAccount.Settings = new Properties();

            Assert.IsNotNull(mgr1.AccountGroupList);
            mgr1.AddNewGroupAccount(newAccount);
            Assert.AreEqual(2, mgr1.AccountGroupList.Count, "Group count is incorrect!");
            Assert.AreEqual("ga100", (mgr1.AccountGroupList[1] as Group).GroupAccount.Id, "New Stand-Alone Account is not added successfully!");
        }

        [TestMethod]
        public void CreateNewGroupAccount_AddingNewGroupHasError_WhenMyIdFormDifferentSession_Test()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            mgr1.LSEngine.SessionId = "SessionId_A";

            CreateGroupAccount newGroup = new CreateGroupAccount();
            newGroup.ReturnCode = 1;
            newGroup.ReturnDesc = "Group already exists with this name!";
            newGroup.MyID = "SessionId_B";
            newGroup.GroupAccount = new GroupAccount();
            newGroup.GroupAccount.Id = "g100";
            newGroup.GroupAccount.DisplayName = "Group 100";
            newGroup.GroupAccount.OwnerID = null;
            newGroup.GroupAccount.Settings = new Properties();

            Assert.IsNotNull(mgr1.AccountGroupList);
            mgr1.AddNewGroupAccount(newGroup);
            Assert.AreEqual(0, mgr1.AccountGroupList.Count, "New Group not added succesfully!");
        }

        [TestMethod]
        public void LoadAllGroupAndAccount_CheckReadGroupAccountsObject_Test()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            Assert.IsNotNull(mgr1.ReadGroupAccountsInstance, "ReadGroupAccountsInstance is null!");
            ReadGroupAccounts readGroupAccounts = AddGroupAccountDataNotInOrder();
            mgr1.LoadAllGroupAndAccount(readGroupAccounts);
            Assert.AreEqual(readGroupAccounts, mgr1.ReadGroupAccountsInstance, "readGroupAccounts in not correctly loaded!");
            Assert.AreEqual(readGroupAccounts.GroupAccounts.Count, mgr1.ReadGroupAccountsInstance.GroupAccounts.Count, "readGroupAccounts in not correctly loaded!");
        }

        [TestMethod]
        public void CreateNewGroupAccount_AddingNewGroupHasError_WhenMyIdFormSameSession_Test()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            mgr1.LSEngine.SessionId = "SessionId_A";

            ReadGroupAccounts readGroupAccounts1 = AddGroupAccountSampleData();
            mgr1.LoadAllGroupAndAccount(readGroupAccounts1);


            //Group groupModified = (mgr1.AccountGroupList[1] as Group);
            //groupModified.Name = "RenameGroup A";
            //groupModified.Id = "ga1";
            //groupModified.RiskSetting = new RiskSetting();
            //groupModified.IsWaitingForServerResponse = true;

            Group groupModified = new Group();
            groupModified.Name = "RenameGroup A";
            groupModified.Id = "ga100";
            groupModified.RiskSetting = new RiskSetting();
            groupModified.IsInEditMode = true;
            groupModified.IsWaitingForServerResponse = true;

            string expectedName = "RenameGroup A"+"";
            Assert.AreEqual(expectedName, groupModified.Name, "When waiting for response, Group name is not correctly updated!");

            CreateGroupAccount newGroup = new CreateGroupAccount();
            newGroup.ReturnCode = 1;
            newGroup.ReturnDesc = "Group already exists with this name!";
            newGroup.MyID = "SessionId_A";
            newGroup.GroupAccount = new GroupAccount();
            newGroup.GroupAccount.Id = "g100";
            newGroup.GroupAccount.DisplayName = "RenameGroup A";
            newGroup.GroupAccount.OwnerID = null;
            newGroup.GroupAccount.Settings = new Properties();

            Assert.IsNotNull(mgr1.AccountGroupList);
            mgr1.AddNewGroupAccount(newGroup);
            Assert.AreEqual(5, mgr1.AccountGroupList.Count, "New Group not added succesfully!");
        }


        [TestMethod]
        public void LoadAllGroupAndAccount_MultpleTimeLoading_Test()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            Assert.IsNotNull(mgr1.ReadGroupAccountsInstance, "ReadGroupAccountsInstance is null!");
            ReadGroupAccounts readGroupAccounts1 = AddGroupAccountDataNotInOrder();
            mgr1.LoadAllGroupAndAccount(readGroupAccounts1);
            Assert.AreEqual(readGroupAccounts1, mgr1.ReadGroupAccountsInstance, "readGroupAccounts in not correctly loaded!");
            Assert.AreEqual(readGroupAccounts1.GroupAccounts.Count, mgr1.ReadGroupAccountsInstance.GroupAccounts.Count, "readGroupAccounts in not correctly loaded!");

            int expectedGroupCount = mgr1.AccountGroupList.Count+1;

            ReadGroupAccounts readGroupAccounts2 = AddGroupAccountDataNotInOrderWithMoreGroupAndAccount();
            mgr1.LoadAllGroupAndAccount(readGroupAccounts2);
            Assert.AreEqual(readGroupAccounts2, mgr1.ReadGroupAccountsInstance, "readGroupAccounts in not correctly loaded!");
            Assert.AreEqual(readGroupAccounts2.GroupAccounts.Count, mgr1.ReadGroupAccountsInstance.GroupAccounts.Count, "readGroupAccounts in not correctly loaded!");

            int actualGroupCount = mgr1.AccountGroupList.Count;
            Assert.AreEqual(expectedGroupCount, actualGroupCount,"Group count is not correct!");

            Assert.AreEqual(0, mgr1.GetDefaultGroupNode().AccountList.Count, "Account count in default group is not correct!");
            Assert.AreEqual(4, mgr1.FindGroupNode("g1").AccountList.Count, "Account count in Group A is not correct!");
            Assert.AreEqual(1, mgr1.FindGroupNode("g2").AccountList.Count, "Account count in Group B group is not correct!");
        }

        [TestMethod]
        public void LoadAllGroupAndAccount_MultpleTimeLoadingAndAccountIsMoved_Test()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            Assert.IsNotNull(mgr1.ReadGroupAccountsInstance, "ReadGroupAccountsInstance is null!");
            ReadGroupAccounts readGroupAccounts1 = AddGroupAccountDataNotInOrder();
            mgr1.LoadAllGroupAndAccount(readGroupAccounts1);
            Assert.AreEqual(readGroupAccounts1, mgr1.ReadGroupAccountsInstance, "readGroupAccounts in not correctly loaded!");
            Assert.AreEqual(readGroupAccounts1.GroupAccounts.Count, mgr1.ReadGroupAccountsInstance.GroupAccounts.Count, "readGroupAccounts in not correctly loaded!");

            int expectedGroupCount = mgr1.AccountGroupList.Count + 1;

            ReadGroupAccounts readGroupAccounts2 = AddGroupAccountDataNotInOrderAndAccountMovedAndGroupDeleted();
            mgr1.LoadAllGroupAndAccount(readGroupAccounts2);
            Assert.AreEqual(readGroupAccounts2, mgr1.ReadGroupAccountsInstance, "readGroupAccounts in not correctly loaded!");
            Assert.AreEqual(readGroupAccounts2.GroupAccounts.Count, mgr1.ReadGroupAccountsInstance.GroupAccounts.Count, "readGroupAccounts in not correctly loaded!");

            int actualGroupCount = mgr1.AccountGroupList.Count;
            Assert.AreEqual(expectedGroupCount-1, actualGroupCount, "Group count is not correct!");

            Assert.AreEqual(2, mgr1.FindGroupNode("g1").AccountList.Count, "Account count in Group A is not correct!");
            Assert.AreEqual(3, mgr1.FindGroupNode("g2").AccountList.Count, "Account count in Group B is not correct!");

        }

        [TestMethod]
        public void LoadAllGroupAndAccount_MultpleTimeLoadingAndStandAlone_Test()
        {
            DataManager mgr1 = new DataManager(this.lsEngine);
            Assert.IsNotNull(mgr1.ReadGroupAccountsInstance, "ReadGroupAccountsInstance is null!");
            ReadGroupAccounts readGroupAccounts1 = AddGroupAccountDataNotInOrderWithStandAloneAccount();
            mgr1.LoadAllGroupAndAccount(readGroupAccounts1);
            Assert.AreEqual(readGroupAccounts1, mgr1.ReadGroupAccountsInstance, "readGroupAccounts in not correctly loaded!");
            Assert.AreEqual(readGroupAccounts1.GroupAccounts.Count, mgr1.ReadGroupAccountsInstance.GroupAccounts.Count, "readGroupAccounts in not correctly loaded!");

            int expectedGroupCount = mgr1.AccountGroupList.Count;
            expectedGroupCount = expectedGroupCount - 2;

            ReadGroupAccounts readGroupAccounts2 = AddGroupAccountDataNotInOrderWithStandAloneAccountMoveDelete();
            mgr1.LoadAllGroupAndAccount(readGroupAccounts2);
            Assert.AreEqual(readGroupAccounts2, mgr1.ReadGroupAccountsInstance, "readGroupAccounts in not correctly loaded!");
            Assert.AreEqual(readGroupAccounts2.GroupAccounts.Count, mgr1.ReadGroupAccountsInstance.GroupAccounts.Count, "readGroupAccounts in not correctly loaded!");

            int actualGroupCount = mgr1.AccountGroupList.Count;
            Assert.AreEqual(expectedGroupCount, actualGroupCount, "Group count is not correct!");

            Assert.AreEqual(true, mgr1.FindGroupNode("sa2").IsAccountGroup , "Stand Alone Account in not correctly loaded!");
            Assert.AreEqual(1, mgr1.FindGroupNode("g1").AccountList.Count, "Account count in Group A is not correct!");
            Assert.AreEqual(1, mgr1.FindGroupNode("d1").AccountList.Count, "Account count in Default group is not correct!");

        }

        [TestMethod]
        public void AddNewGroupAccountTest_AddingNewGroup()
        {
            DataManager dMgr = new DataManager(lsEngine);
            int expected = 0;
            int actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            CreateGroupAccount createGroupAccount = new CreateGroupAccount();
            createGroupAccount.MyID = "11112121212";
            createGroupAccount.ReturnCode = 0;
            GroupAccount group = GroupAccountUtil.CreateNewGroupAccount("g4","Group4", 0, new Properties(), null);
            createGroupAccount.GroupAccount = group;


            dMgr.AddNewGroupAccount(createGroupAccount);

            expected = 1;
            actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");
        }

        [TestMethod]
        public void AddNewGroupAccountTest_AddingNewDefaultGroup()
        {
            DataManager dMgr = new DataManager(lsEngine);
            int expected = 0;
            int actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            CreateGroupAccount createGroupAccount = new CreateGroupAccount();
            createGroupAccount.MyID = "11112121212";
            createGroupAccount.ReturnCode = 0;
            GroupAccount group = GroupAccountUtil.CreateNewGroupAccount("d4", "Default", 1, new Properties(), null);
            createGroupAccount.GroupAccount = group;


            dMgr.AddNewGroupAccount(createGroupAccount);

            expected = 1;
            actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            Assert.IsTrue(dMgr.FindGroupNode("d4").IsDefaultGroup);
        }


        [TestMethod]
        public void AddNewGroupAccountTest_AddingNewWhenMyIDIsSame()
        {
            DataManager dMgr = new DataManager(lsEngine);
            int expected = 0;
            int actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            dMgr.ReadGroupAccountsInstance = new ReadGroupAccounts();

            Group newSaAccount1 = dMgr.CreateNewStandAloneAccountNode("TestAccountSA1");
            dMgr.AccountGroupList.Add(newSaAccount1);

            newSaAccount1.IsInEditMode = false;

            lsEngine.SessionId = "11111111";
            CreateGroupAccount createGroupAccount = new CreateGroupAccount();
            createGroupAccount.MyID = lsEngine.SessionId;
            createGroupAccount.ReturnCode = 1;
            GroupAccount group = GroupAccountUtil.CreateNewGroupAccount(newSaAccount1.Id, newSaAccount1.Name, 0, new Properties(), newSaAccount1.Id);
            createGroupAccount.GroupAccount = group;

            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(group);

            dMgr.AddNewGroupAccount(createGroupAccount);

            expected = 1;
            actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            //Since there is an error therefore the IsInEditMode = true
            Assert.IsTrue(newSaAccount1.IsInEditMode);

        }


        [TestMethod]
        public void AddNewGroupAccountTest_AddingNewWhenMyIDIsDifferent()
        {
            DataManager dMgr = new DataManager(lsEngine);
            int expected = 0;
            int actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            Group newSaAccount1 = dMgr.CreateNewStandAloneAccountNode("TestAccountSA1");
            dMgr.AccountGroupList.Add(newSaAccount1);

            newSaAccount1.IsInEditMode = false;

            lsEngine.SessionId = "11111111";
            CreateGroupAccount createGroupAccount = new CreateGroupAccount();
            createGroupAccount.MyID = "121212121";
            createGroupAccount.ReturnCode = 1;
            GroupAccount group = GroupAccountUtil.CreateNewGroupAccount("g4", "Group 4", 0, new Properties(), null);
            createGroupAccount.GroupAccount = group;

            dMgr.AddNewGroupAccount(createGroupAccount);

            expected = 1;
            actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

        }


        [TestMethod]
        public void AddNewGroupAccountTest_AddingNewStandAloneAccount()
        {
            DataManager dMgr = new DataManager(lsEngine);
            int expected = 0;
            int actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            CreateGroupAccount createGroupAccount = new CreateGroupAccount();
            createGroupAccount.MyID = "11112121212";
            createGroupAccount.ReturnCode = 0;
            GroupAccount group = GroupAccountUtil.CreateNewGroupAccount("sa4", "SA Account 4a", 0, new Properties(), "sa4");
            createGroupAccount.GroupAccount = group;


            dMgr.AddNewGroupAccount(createGroupAccount);

            expected = 1;
            actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            Assert.IsTrue(dMgr.FindGroupNode("sa4").IsAccountGroup);

        }

        [TestMethod]
        public void AddNewGroupAccountTest_AddingAccountWhenParentGroupIsNotExists()
        {
            DataManager dMgr = new DataManager(lsEngine);
            int expected = 0;
            int actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            CreateGroupAccount createGroupAccount = new CreateGroupAccount();
            createGroupAccount.MyID = "11112121212";
            createGroupAccount.ReturnCode = 0;
            GroupAccount account = GroupAccountUtil.CreateNewGroupAccount("a1", "Account a1", 0, null, "g1");
            createGroupAccount.GroupAccount = account;

            dMgr.AddNewGroupAccount(createGroupAccount);

            expected = 0;
            actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");
        }


        [TestMethod]
        public void AddNewGroupAccountTest_AddingAccountWhenParentGroupIsExists()
        {
            DataManager dMgr = new DataManager(lsEngine);
            int expected = 0;
            int actual = dMgr.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            Group g1 = new Group() { Id = "g1", Name = "Group A", IsAccountGroup = false, AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            dMgr.AccountGroupList.Add(g1);

            CreateGroupAccount createGroupAccount = new CreateGroupAccount();
            createGroupAccount.MyID = "11112121212";
            createGroupAccount.ReturnCode = 0;
            GroupAccount account = GroupAccountUtil.CreateNewGroupAccount("a1", "Account a1", 0, null, "g1");
            createGroupAccount.GroupAccount = account;

            expected = 0;
            actual = dMgr.FindGroupNode("g1").AccountList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");

            dMgr.AddNewGroupAccount(createGroupAccount);

            expected = 1;
            actual = dMgr.FindGroupNode("g1").AccountList.Count;
            Assert.AreEqual(expected, actual, "AccountGroupList is not matched!");
        }

        [TestMethod]
        public void CreateNewGroupNodeTest()
        {
            DataManager dMgr = new DataManager(lsEngine);
            Group newGroup = dMgr.CreateNewGroupNode();
            Assert.AreEqual("New Group", newGroup.Name);
            Assert.AreEqual(false, newGroup.IsAccountGroup);
            Assert.AreEqual(false, newGroup.IsDefaultGroup);
            Assert.AreEqual(true, newGroup.IsInEditMode);
            Assert.IsNotNull(newGroup.AccountList);
            Assert.IsNotNull(newGroup.RiskSetting);

        }


        [TestMethod]
        public void MoveAccountNode_FromGroupToGroupTest()
        {
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account(){ Name="AccountA", ParentGroup=group1, Id="AccountA", OwnerId = "g1"});
            Group group2 = dMgr.CreateNewGroupNode();
            group2.Id = "g2";
            group2.Name = "GroupB";

            dMgr.AccountGroupList.Add(group1);
            dMgr.AccountGroupList.Add(group2);

            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupB", Id = "g2", OwnerID = null, IsDefault = 0 });

            Assert.AreEqual(1, group1.AccountList.Count);
            Assert.AreEqual(0, group2.AccountList.Count);

            dMgr.MoveAccountNode(null, "AccountA", "g2");

            Assert.AreEqual(0, group1.AccountList.Count);
            Assert.AreEqual(1, group2.AccountList.Count);


        }

        [TestMethod]
        public void MoveAccountNode_FromStandAloneAccountToGroupTest()
        {
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";

            Group group2 = dMgr.CreateNewGroupNode();
            group2.Id = "g2";
            group2.Name = "GroupB";


            Group saAccount1 = dMgr.CreateNewStandAloneAccountNode("AccountA");


            dMgr.AccountGroupList.Add(group1);
            dMgr.AccountGroupList.Add(group2);
            dMgr.AccountGroupList.Add(saAccount1);
            
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupB", Id = "g2", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "AccountA", Id = "AccountA", OwnerID = "AccountA", IsDefault = 0 });

            Assert.AreEqual(0, group1.AccountList.Count);
            Assert.AreEqual(0, group2.AccountList.Count);
            Assert.AreEqual(3, dMgr.AccountGroupList.Count);

            dMgr.MoveAccountNode(new Properties(), "AccountA", "g1");

            Assert.AreEqual(1, group1.AccountList.Count);
            Assert.AreEqual(0, group2.AccountList.Count);
            Assert.AreEqual(2, dMgr.AccountGroupList.Count);

        }

        [TestMethod]
        public void MoveAccountNode_FromGroupToStandAloneAccountTest()
        {
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Name = "AccountA", ParentGroup = group1, Id = "AccountA", OwnerId = "g1" });
            Group group2 = dMgr.CreateNewGroupNode();
            group2.Id = "g2";
            group2.Name = "GroupB";

            dMgr.AccountGroupList.Add(group1);
            dMgr.AccountGroupList.Add(group2);

            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupB", Id = "g2", OwnerID = null, IsDefault = 0 });

            Assert.AreEqual(1, group1.AccountList.Count);
            Assert.AreEqual(0, group2.AccountList.Count);
            Assert.AreEqual(2, dMgr.AccountGroupList.Count);

            dMgr.MoveAccountNode(new Properties(), "AccountA", "AccountA");

            Assert.AreEqual(0, group1.AccountList.Count);
            Assert.AreEqual(0, group2.AccountList.Count);
            Assert.AreEqual(3, dMgr.AccountGroupList.Count);

        }

        [TestMethod]
        public void SaveGroupTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";

            Assert.AreEqual(0, dMgr.AccountGroupList.Count);

            dMgr.SaveGroup(group1);
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, lsEngine.DataManager.AccountGroupList.Count);

        }

        [TestMethod]
        public void SaveStandAloneAccountTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewStandAloneAccountNode("SA Account 2");

            Assert.AreEqual(0, lsEngine.DataManager.AccountGroupList.Count);

            dMgr.SaveGroup(group1);
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, lsEngine.DataManager.AccountGroupList.Count);

        }

        [TestMethod]
        public void SaveExistingGroupTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupARen";

            lsEngine.DataManager.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });

            Assert.AreEqual(1, lsEngine.DataManager.AccountGroupList.Count);

            dMgr.SaveGroup(group1);
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(1, lsEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(group1.Name, lsEngine.DataManager.GetGroupAccount("g1").DisplayName);
        }

        [TestMethod]
        public void RemoveGroupAccountNodeTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupARen";

            dMgr.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });

            Assert.AreEqual(1, dMgr.AccountGroupList.Count);

            GroupAccount groupAcc = GroupAccountUtil.CreateNewGroupAccount(group1.Id, group1.Name, 0, new Properties(), null);
            dMgr.RemoveGroupAccountNode(groupAcc);
            Assert.AreEqual(0, dMgr.AccountGroupList.Count);
        }


        [TestMethod]
        public void RemoveGroupAccountNode_AccountRemoveTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupARen";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });

            dMgr.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A1", Id = "A1", OwnerID = "g1", IsDefault = 0 });

            Assert.AreEqual(1, dMgr.AccountGroupList.Count);
            Assert.AreEqual(1, dMgr.FindGroupNode("g1").AccountList.Count);


            GroupAccount groupAcc = GroupAccountUtil.CreateNewGroupAccount("A1", "A1", 0, new Properties(), "g1");
            dMgr.RemoveGroupAccountNode(groupAcc);
            Assert.AreEqual(1, dMgr.AccountGroupList.Count);
            Assert.AreEqual(0, dMgr.FindGroupNode("g1").AccountList.Count);
        }

        [TestMethod]
        public void SaveAccountTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);

            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupARen";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });

            dMgr.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A1", Id = "A1", OwnerID = "g1", IsDefault = 0 });
            Assert.AreEqual(1, dMgr.FindGroupNode("g1").AccountList.Count);

            lsEngine.DataManager.AccountGroupList = dMgr.AccountGroupList;

            Account accountNode = new Account() { Id = "A2", Name = "A2", IsOwnGroup = false, ParentGroup = group1 };

            dMgr.SaveAccount(accountNode);
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(2, lsEngine.DataManager.FindGroupNode("g1").AccountList.Count);

        }

        [TestMethod]
        public void MoveAccountTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });
            group1.AccountList.Add(new Account() { Id = "A2", Name = "A2", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });

            Group group2 = dMgr.CreateNewGroupNode();
            group2.Id = "g2";
            group2.Name = "GroupB";

            dMgr.AccountGroupList.Add(group1);
            dMgr.AccountGroupList.Add(group2);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupB", Id = "g2", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A1", Id = "A1", OwnerID = "g1", IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A2", Id = "A2", OwnerID = "g1", IsDefault = 0 });

            Assert.AreEqual(2, dMgr.AccountGroupList.Count);
            Assert.AreEqual(2, dMgr.FindGroupNode("g1").AccountList.Count);
            Assert.AreEqual(0, dMgr.FindGroupNode("g2").AccountList.Count);

            lsEngine.DataManager.AccountGroupList = dMgr.AccountGroupList;
            lsEngine.DataManager.ReadGroupAccountsInstance = dMgr.ReadGroupAccountsInstance;

            dMgr.MoveAccount("A2", "g2");

            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(2, dMgr.AccountGroupList.Count);
            Assert.AreEqual(1, dMgr.FindGroupNode("g1").AccountList.Count);
            Assert.AreEqual(1, dMgr.FindGroupNode("g2").AccountList.Count);


        }

        [TestMethod]
        public void RemoveAccountNodeTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });

            dMgr.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A1", Id = "A1", OwnerID = "g1", IsDefault = 0 });

            Assert.AreEqual(1, dMgr.AccountGroupList.Count);
            Assert.AreEqual(1, dMgr.FindGroupNode("g1").AccountList.Count);

            lsEngine.DataManager.AccountGroupList = dMgr.AccountGroupList;

            Account accountNode = dMgr.FindNodeEntity("A1") as Account;
            dMgr.RemoveAccountNode(accountNode);

            Assert.AreEqual(1, dMgr.AccountGroupList.Count);
            Assert.AreEqual(0, dMgr.FindGroupNode("g1").AccountList.Count);

        }

        [TestMethod]
        public void GetGroupAccountByNameTest()
        {
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });

            dMgr.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });

            GroupAccount g1 = dMgr.GetGroupAccountByName(group1.Name);
            Assert.AreEqual("g1", g1.Id);

            GroupAccount gNull = dMgr.GetGroupAccountByName("null");
            Assert.IsNull(gNull);
        }

        [TestMethod]
        public void RemoveStandAloneAccountNodeTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group saAccount = dMgr.CreateNewStandAloneAccountNode("SA1");
            Group saAccount2 = dMgr.CreateNewStandAloneAccountNode("SA2");

            dMgr.AccountGroupList.Add(saAccount);
            dMgr.AccountGroupList.Add(saAccount2);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "SA1", Id = "SA1", OwnerID = "SA1", IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "SA2", Id = "SA2", OwnerID = "SA2", IsDefault = 0 });

            Assert.AreEqual(2, dMgr.AccountGroupList.Count);

            lsEngine.DataManager.AccountGroupList = dMgr.AccountGroupList;

            Group accountNode = dMgr.FindNodeEntity("SA1") as Group;
            dMgr.RemoveAccountNode(accountNode.GroupAccount);

            Assert.AreEqual(1, lsEngine.DataManager.AccountGroupList.Count);

        }


        [TestMethod]
        public void RemoveGroupNodeTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });

            dMgr.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });

            Assert.AreEqual(1, dMgr.AccountGroupList.Count);

            lsEngine.DataManager.AccountGroupList = dMgr.AccountGroupList;

            Group groupNode = dMgr.FindNodeEntity("g1") as Group;
            dMgr.RemoveGroupNode(groupNode);

            Assert.AreEqual(0, dMgr.AccountGroupList.Count);

        }

        [TestMethod]
        public void UpdateRiskSettingTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });
            //group1.RiskSetting.CreditLimit = 100000;
            group1.RiskSetting.ClientCreditLimit = 100000;
            group1.RiskSetting.DuplicateOrderTimeInterval = 1000;
            group1.RiskSetting.MaxDuplicateOrder = 10;
            group1.RiskSetting.MaxNotionalPerOrder = 10000;
            group1.RiskSetting.MaxPriceDiff = 10;

            dMgr.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });

            Assert.AreEqual(1, dMgr.AccountGroupList.Count);

            lsEngine.DataManager.AccountGroupList = dMgr.AccountGroupList;

            dMgr.UpdateRiskSetting(group1);
            GroupAccount g1 = lsEngine.DataManager.GetGroupAccount("g1");

            RiskSetting setting = new RiskSetting(g1.Settings.PropertyMap.tagValues);

            Assert.AreEqual(group1.RiskSetting.CreditLimit, setting.CreditLimit);
            Assert.AreEqual(group1.RiskSetting.MaxDuplicateOrder, setting.MaxDuplicateOrder);
            Assert.AreEqual(group1.RiskSetting.DuplicateOrderTimeInterval, setting.DuplicateOrderTimeInterval);
            Assert.AreEqual(group1.RiskSetting.MaxNotionalPerOrder, setting.MaxNotionalPerOrder);
            Assert.AreEqual(group1.RiskSetting.MaxPriceDiff, setting.MaxPriceDiff);
        }

        [TestMethod]
        public void UpdateGroupNodeRiskSettingTest()
        {
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });
            group1.RiskSetting.CreditLimit = 100000;
            group1.RiskSetting.DuplicateOrderTimeInterval = 1000;
            group1.RiskSetting.MaxDuplicateOrder = 10;
            group1.RiskSetting.MaxNotionalPerOrder = 10000;
            group1.RiskSetting.MaxPriceDiff = 10;
            dMgr.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });

            Assert.AreEqual(1, dMgr.AccountGroupList.Count);

            GroupAccount groupAccount = GroupAccountUtil.CreateNewGroupAccount("g1", "GroupA", 0, new Properties(), null);
            groupAccount.Settings = new Properties();
            groupAccount.Settings.PropertyMap = new TagValueMsg();
            groupAccount.Settings.PropertyMap.tagValues = new System.Collections.Hashtable();
            //groupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.CREDIT_LIMIT_DBL, "200000");
            groupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.CLIENT_CREDIT_LIMIT_DBL, "200000");
            groupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_DUPES_CSV, "30, 30000");
            groupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_NOTIONAL_INT, "400000");
            groupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_PRICE_DIFF_PCT, "50");

            dMgr.UpdateGroupNodeRiskSetting(groupAccount);

            Group groupA = dMgr.FindGroupNode("g1");

            Assert.AreEqual(200000,group1.RiskSetting.CreditLimit);
            Assert.AreEqual(30, group1.RiskSetting.MaxDuplicateOrder);
            Assert.AreEqual(30000, group1.RiskSetting.DuplicateOrderTimeInterval);
            Assert.AreEqual(400000, group1.RiskSetting.MaxNotionalPerOrder);
            Assert.AreEqual(50, group1.RiskSetting.MaxPriceDiff);

        }


        [TestMethod]
        public void UpdateGroupNodeNameTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";

            dMgr.AccountGroupList.Add(group1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });

            GroupAccount groupAccount = GroupAccountUtil.CreateNewGroupAccount("g1", "GroupA-Ren", 0, new Properties(), null);

            dMgr.UpdateGroupNodeName(groupAccount);

            Assert.AreEqual("GroupA-Ren", dMgr.FindGroupNode("g1").Name);
        }

        [TestMethod]
        public void GetAccountListTest()
        {
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });
            group1.AccountList.Add(new Account() { Id = "A2", Name = "A2", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });
            group1.AccountList.Add(new Account() { Id = "A3", Name = "A3", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });
            group1.AccountList.Add(new Account() { Id = "A4", Name = "A4", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });



            Group group2 = dMgr.CreateNewGroupNode();
            group2.Id = "g2";
            group2.Name = "GroupB";

            dMgr.AccountGroupList.Add(group1);
            dMgr.AccountGroupList.Add(group2);


            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupB", Id = "g2", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A1", Id = "A1", OwnerID = "g1", IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A2", Id = "A2", OwnerID = "g1", IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A3", Id = "A3", OwnerID = "g1", IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A4", Id = "A4", OwnerID = "g1", IsDefault = 0 });


            List<GroupAccount> group1AccountList = dMgr.GetAccountList(group1.Id);
            List<GroupAccount> group2AccountList = dMgr.GetAccountList(group2.Id);

            Assert.AreEqual(4, group1AccountList.Count);
            Assert.AreEqual(0, group2AccountList.Count);
        }

        [TestMethod]
        public void SaveGroupAccountConfigTest()
        {
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Name = "AccountA", ParentGroup = group1, Id = "AccountA", OwnerId = "g1" });

            Group group2 = dMgr.CreateNewGroupNode();
            group2.Id = "g2";
            group2.Name = "GroupB";

            Group saAccount1 = dMgr.CreateNewStandAloneAccountNode("AccountA");

            dMgr.AccountGroupList.Add(group1);
            dMgr.AccountGroupList.Add(group2);
            dMgr.AccountGroupList.Add(saAccount1);

            Assert.AreEqual(3, dMgr.AccountGroupList.Count);
            dMgr.SaveGroupAccountConfig("TempTest.xml");
            List<Group> saveGroupOrder = new List<Group>();
            saveGroupOrder = SerializerUtil.LoadFromFile<List<Group>>("TempTest.xml");

            Assert.AreEqual(3, saveGroupOrder.Count);
        }

        [TestMethod]
        public void MoveAccountNodeTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group group1 = dMgr.CreateNewGroupNode();
            group1.Id = "g1";
            group1.Name = "GroupA";
            group1.AccountList.Add(new Account() { Id = "A1", Name = "A1", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });
            group1.AccountList.Add(new Account() { Id = "A2", Name = "A2", OwnerId = "g1", ParentGroup = group1, IsOwnGroup = false });

            Group group2 = dMgr.CreateNewGroupNode();
            group2.Id = "g2";
            group2.Name = "GroupB";

            dMgr.AccountGroupList.Add(group1);
            dMgr.AccountGroupList.Add(group2);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupA", Id = "g1", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "GroupB", Id = "g2", OwnerID = null, IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A1", Id = "A1", OwnerID = "g1", IsDefault = 0 });
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(new GroupAccount() { DisplayName = "A2", Id = "A2", OwnerID = "g1", IsDefault = 0 });

            Assert.AreEqual(2, dMgr.AccountGroupList.Count);
            Assert.AreEqual(2, dMgr.FindGroupNode("g1").AccountList.Count);
            Assert.AreEqual(0, dMgr.FindGroupNode("g2").AccountList.Count);

            lsEngine.DataManager.AccountGroupList = dMgr.AccountGroupList;
            lsEngine.DataManager.ReadGroupAccountsInstance = dMgr.ReadGroupAccountsInstance;

            Account a2 = dMgr.FindNodeEntity("A2") as Account;
            dMgr.MoveAccountNode(a2, "g2");

            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(2, dMgr.AccountGroupList.Count);
            Assert.AreEqual(1, dMgr.FindGroupNode("g1").AccountList.Count);
            Assert.AreEqual(1, dMgr.FindGroupNode("g2").AccountList.Count);
        }

        [TestMethod]
        public void EvaluateIsGroupExpended_WhenSaveGroupAreEmptyTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);

            List<Group> saveGroupOrder = new List<Group>();
            GroupAccount groupAccountObj = new GroupAccount() { Id = "G1" };
            bool expected = false;
            bool actual = dMgr.EvaluateIsGroupExpended(saveGroupOrder, groupAccountObj);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EvaluateIsGroupExpended_WhenGroupInNotInSaveGroupListTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group g2 = new Group() { Id = "G2", Name = "G2" };
            Group g3 = new Group() { Id = "G3", Name = "G3" };
            Group g4 = new Group() { Id = "G4", Name = "G4" };
            Group g5 = new Group() { Id = "G5", Name = "G5" };

            List<Group> saveGroupOrder = new List<Group>();
            saveGroupOrder.Add(g2);
            saveGroupOrder.Add(g3);
            saveGroupOrder.Add(g4);
            saveGroupOrder.Add(g5);
            GroupAccount groupAccountObj = new GroupAccount() { Id = "G1" };
            bool expected = false;
            bool actual = dMgr.EvaluateIsGroupExpended(saveGroupOrder, groupAccountObj);

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void EvaluateIsGroupExpended_WhenGroupInInSaveGroupListButNotExpendedTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group g1 = new Group() { Id = "G1", Name = "G1" };
            Group g2 = new Group() { Id = "G2", Name = "G2" };
            Group g3 = new Group() { Id = "G3", Name = "G3" };
            Group g4 = new Group() { Id = "G4", Name = "G4" };
            Group g5 = new Group() { Id = "G5", Name = "G5" };

            List<Group> saveGroupOrder = new List<Group>();
            saveGroupOrder.Add(g1);
            saveGroupOrder.Add(g2);
            saveGroupOrder.Add(g3);
            saveGroupOrder.Add(g4);
            saveGroupOrder.Add(g5);
            GroupAccount groupAccountObj = new GroupAccount() { Id = "G1" };
            bool expected = false;
            bool actual = dMgr.EvaluateIsGroupExpended(saveGroupOrder, groupAccountObj);

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void EvaluateIsGroupExpended_WhenGroupInInSaveGroupListAndExpendedTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);
            Group g1 = new Group() { Id = "G1", Name = "G1", IsExpanded=true };
            Group g2 = new Group() { Id = "G2", Name = "G2", IsExpanded = true };
            Group g3 = new Group() { Id = "G3", Name = "G3" };
            Group g4 = new Group() { Id = "G4", Name = "G4", IsExpanded = true };
            Group g5 = new Group() { Id = "G5", Name = "G5", IsExpanded = true };

            List<Group> saveGroupOrder = new List<Group>();
            saveGroupOrder.Add(g1);
            saveGroupOrder.Add(g2);
            saveGroupOrder.Add(g3);
            saveGroupOrder.Add(g4);
            saveGroupOrder.Add(g5);

            GroupAccount groupAccountObj1 = new GroupAccount() { Id = "G1" };
            bool expected = true;
            bool actual = dMgr.EvaluateIsGroupExpended(saveGroupOrder, groupAccountObj1);
            Assert.AreEqual(expected, actual);


            GroupAccount groupAccountObj2 = new GroupAccount() { Id = "G2" };
            expected = true;
            actual = dMgr.EvaluateIsGroupExpended(saveGroupOrder, groupAccountObj2);
            Assert.AreEqual(expected, actual);

            GroupAccount groupAccountObj3 = new GroupAccount() { Id = "G3" };
            expected = false;
            actual = dMgr.EvaluateIsGroupExpended(saveGroupOrder, groupAccountObj3);
            Assert.AreEqual(expected, actual);

            GroupAccount groupAccountObj4 = new GroupAccount() { Id = "G4" };
            expected = true;
            actual = dMgr.EvaluateIsGroupExpended(saveGroupOrder, groupAccountObj4);
            Assert.AreEqual(expected, actual);

            GroupAccount groupAccountObj5 = new GroupAccount() { Id = "G5" };
            expected = true;
            actual = dMgr.EvaluateIsGroupExpended(saveGroupOrder, groupAccountObj5);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CreateNewGroupNode_WhenIsGroupNodeExpendedIsTrueTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);

            GroupAccount groupAccountObj1 = new GroupAccount() { Id = "G1", DisplayName="G1", OwnerID=null, Settings = new Properties() };

            dMgr.AccountGroupList.Clear();
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj1, true);

            bool expected = true;
            bool actual = dMgr.AccountGroupList[0].IsExpanded;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void ValidateExistingGroupAccountTest()
        {
            LSEngine.TEST_MODE = true;
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);

            dMgr.AccountGroupList.Clear();

            GroupAccount groupAccountObjDefault = new GroupAccount() { Id = "Default", DisplayName = "Default", IsDefault = 1, OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj1 = new GroupAccount() { Id = "G1", DisplayName = "G1", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj2 = new GroupAccount() { Id = "G2", DisplayName = "G2", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj3 = new GroupAccount() { Id = "G3", DisplayName = "G3", OwnerID = null, Settings = new Properties() };

            dMgr.CreateNewGroupNode(string.Empty, groupAccountObjDefault, false);
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj1, false);
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj2, false);
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj3, false);

            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObjDefault);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj2);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj3);

            Group gTest = new Group() { Id = "G4", Name = "G4", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            bool expected = true;
            bool actual = dMgr.ValidateExistingGroupAccount(gTest);
            Assert.AreEqual(expected, actual);

            Group gTest1 = new Group() { Id = "G1", Name = "G1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            expected = false;
            actual = dMgr.ValidateExistingGroupAccount(gTest1);
            Assert.AreEqual(expected, actual);

            Group gTest2 = new Group() { Id = "G2", Name = "", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            expected = false;
            actual = dMgr.ValidateExistingGroupAccount(gTest2);
            Assert.AreEqual(expected, actual);


            Group gTest3 = new Group() { Id = "G2", Name = "", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            gTest3.Name = "G-0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";
            expected = false;
            actual = dMgr.ValidateExistingGroupAccount(gTest3);
            Assert.AreEqual(expected, actual);     
        }

        [TestMethod]
        public void ValidateNewGroupTest()
        {
            LSEngine.TEST_MODE = true;
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);

            dMgr.AccountGroupList.Clear();

            GroupAccount groupAccountObjDefault = new GroupAccount() { Id = "Default", DisplayName = "Default", IsDefault = 1, OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj1 = new GroupAccount() { Id = "G1", DisplayName = "G1", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj2 = new GroupAccount() { Id = "G2", DisplayName = "G2", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj3 = new GroupAccount() { Id = "G3", DisplayName = "G3", OwnerID = null, Settings = new Properties() };

            dMgr.CreateNewGroupNode(string.Empty, groupAccountObjDefault, false);
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj1, false);
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj2, false);
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj3, false);

            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObjDefault);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj1);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj2);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj3);

            Group gTest = new Group() { Id = "G4", Name = "G4", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            bool expected = true;
            bool actual = dMgr.ValidateExistingGroupAccount(gTest);
            Assert.AreEqual(expected, actual);

            Group gTest1 = new Group() { Id = "G1", Name = "G1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            expected = false;
            actual = dMgr.ValidateExistingGroupAccount(gTest1);
            Assert.AreEqual(expected, actual);

            Group gTest2 = new Group() { Id = "G2", Name = "", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            expected = false;
            actual = dMgr.ValidateExistingGroupAccount(gTest2);
            Assert.AreEqual(expected, actual);


            Group gTest3 = new Group() { Id = "G2", Name = "", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            gTest3.Name = "G-0123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";
            expected = false;
            actual = dMgr.ValidateExistingGroupAccount(gTest3);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void MoveAccountNodeForStandaloneAccountTest()
        {
            LSEngine.TEST_MODE = true;
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);

            dMgr.AccountGroupList.Clear();

            GroupAccount groupAccountObjDefault = new GroupAccount() { Id = "Default", DisplayName = "Default", IsDefault = 1, OwnerID = null, Settings = new Properties() };

            dMgr.CreateNewGroupNode(string.Empty, groupAccountObjDefault, false);
            dMgr.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObjDefault);

            
            dMgr.MoveAccountNode(null, "SA1", "SA1");
            
            string expected = "SA1";
            string actual = dMgr.AccountGroupList[0].Name;

            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void SelectedGroupNodeTest()
        {
            lsEngine.SessionId = "11111111";
            DataManager dMgr = new DataManager(lsEngine);

            dMgr.AccountGroupList.Clear();

            GroupAccount groupAccountObjDefault = new GroupAccount() { Id = "Default", DisplayName = "Default", IsDefault=1, OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj1 = new GroupAccount() { Id = "G1", DisplayName = "G1", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj2 = new GroupAccount() { Id = "G2", DisplayName = "G2", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj3 = new GroupAccount() { Id = "G3", DisplayName = "G3", OwnerID = null, Settings = new Properties() };
      
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObjDefault, false);
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj1, false);
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj2, false);
            dMgr.CreateNewGroupNode(string.Empty, groupAccountObj3, false);

            bool expected = true;
            bool actual = dMgr.SelectedGroupNode().IsDefaultGroup;
            Assert.AreEqual(expected, actual);

            dMgr.AccountGroupList[0].IsSelected = false;
            dMgr.AccountGroupList[1].IsSelected = true;

            string expectedName = dMgr.AccountGroupList[1].Name; 
            string actualName = dMgr.SelectedGroupNode().Name;
            Assert.AreEqual(expectedName, actualName);

        }


        private static ReadGroupAccounts AddGroupAccountDataNotInOrderWithStandAloneAccount()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();
            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");

            GroupAccount accontSA1 = GroupAccountUtil.CreateNewGroupAccount("sa1", "S-Account A1", 0, new Properties(), "sa1");
            GroupAccount accontSA2 = GroupAccountUtil.CreateNewGroupAccount("sa2", "S-Account A2", 0, new Properties(), "sa2");
            GroupAccount accontSA3 = GroupAccountUtil.CreateNewGroupAccount("sa3", "S-Account A3", 0, new Properties(), "sa3");

            groupAccountList.Add(accontSA1);
            groupAccountList.Add(accontSA2);
            groupAccountList.Add(accontSA3);

            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(defaultGroup);
            groupAccountList.Add(group1);
            groupAccountList.Add(accontA3);
            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            return readGroupAccounts;
        }
        private static ReadGroupAccounts AddGroupAccountDataNotInOrderWithStandAloneAccountMoveDelete()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();
            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");

            GroupAccount accontSA1 = GroupAccountUtil.CreateNewGroupAccount("sa3", "S-Account A1", 0, new Properties(), "d1");
            GroupAccount accontSA2 = GroupAccountUtil.CreateNewGroupAccount("sa2", "S-Account A2", 0, new Properties(), "sa2");

            groupAccountList.Add(accontSA1);
            groupAccountList.Add(accontSA2);

            groupAccountList.Add(accontA1);
            groupAccountList.Add(defaultGroup);
            groupAccountList.Add(group1);

            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            return readGroupAccounts;
        }


        private static ReadGroupAccounts AddGroupAccountDataNotInOrder()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();
            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");

            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(defaultGroup);
            groupAccountList.Add(group1);
            groupAccountList.Add(accontA3);
            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            return readGroupAccounts;
        }

        private static ReadGroupAccounts AddGroupAccountSampleData()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();
            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount group2 = GroupAccountUtil.CreateNewGroupAccount("g2", "Group B", 0, new Properties(), null);

            GroupAccount accontDefault1 = GroupAccountUtil.CreateNewGroupAccount("dfa1", "Account A1", 0, null, "d1");
            GroupAccount accontDefault2 = GroupAccountUtil.CreateNewGroupAccount("dfa2", "Account A2", 0, null, "d1");

            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");

            GroupAccount accontB1 = GroupAccountUtil.CreateNewGroupAccount("gb1", "Account B1", 0, null, "g2");
            GroupAccount accontB2 = GroupAccountUtil.CreateNewGroupAccount("gb2", "Account B2", 0, null, "g2");
            GroupAccount accontB3 = GroupAccountUtil.CreateNewGroupAccount("gb3", "Account B3", 0, null, "g2");

            GroupAccount standAloneAccont1 = GroupAccountUtil.CreateNewGroupAccount("sa1", "Account SA1", 0, null, "sa1");
            GroupAccount standAloneAccont2 = GroupAccountUtil.CreateNewGroupAccount("sa2", "Account SA2", 0, null, "sa2");


            groupAccountList.Add(defaultGroup);
            groupAccountList.Add(accontDefault1);
            groupAccountList.Add(accontDefault2);


            groupAccountList.Add(group1);
            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(accontA3);

            groupAccountList.Add(group2);
            groupAccountList.Add(accontB1);
            groupAccountList.Add(accontB2);
            groupAccountList.Add(accontB3);

            groupAccountList.Add(standAloneAccont1);
            groupAccountList.Add(standAloneAccont2);


            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            return readGroupAccounts;
        }

        private static ReadGroupAccounts AddGroupAccountDataInOrder()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();
            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");

            groupAccountList.Add(defaultGroup);
            groupAccountList.Add(group1);
            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(accontA3);
            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            return readGroupAccounts;
        }

        private static ReadGroupAccounts AddGroupAccountDataNotInOrderWithMoreGroupAndAccount()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();
            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount group2 = GroupAccountUtil.CreateNewGroupAccount("g2", "Group B", 0, new Properties(), null);
            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");
            GroupAccount accontA4 = GroupAccountUtil.CreateNewGroupAccount("ga4", "Account A4", 0, null, "g1");
            GroupAccount accontB1 = GroupAccountUtil.CreateNewGroupAccount("gb1", "Account B1", 0, null, "g2");

            groupAccountList.Add(accontB1); 

            groupAccountList.Add(accontA4); 
            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(defaultGroup);
            groupAccountList.Add(group1);
            groupAccountList.Add(group2);
            groupAccountList.Add(accontA3);
            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            return readGroupAccounts;
        }


        private ReadGroupAccounts AddGroupAccountDataNotInOrderAndAccountMovedAndGroupDeleted()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();
            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount group2 = GroupAccountUtil.CreateNewGroupAccount("g2", "Group B", 0, new Properties(), null);
            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g2");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g2");
            GroupAccount accontA4 = GroupAccountUtil.CreateNewGroupAccount("ga4", "Account A4", 0, null, "g1");
            GroupAccount accontB1 = GroupAccountUtil.CreateNewGroupAccount("gb1", "Account B1", 0, null, "g2");

            groupAccountList.Add(accontB1);

            groupAccountList.Add(accontA4);
            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(group1);
            groupAccountList.Add(group2);
            groupAccountList.Add(accontA3);
            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            return readGroupAccounts;
        }




    }
}
