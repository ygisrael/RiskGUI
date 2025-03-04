using EZXLightspeedGUI.View;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using EZXLightspeedGUI.Model;
using System.Collections.Generic;
using EZX.LightspeedEngine.Entity;
using EZXLightspeedGUI;

namespace EZXLightspeedGUITest
{


    [TestClass()]
    public class MainMenuUserControlTest
    {

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //Create the application for resources.
            if (System.Windows.Application.Current == null)
            {
                ApplicationManager.MOCK_MODE = true;
                App application = new App();
                application.InitializeComponent();
                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Clear();
                App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Clear();
            }
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadAllDataTest_WhenThereAreNoLines()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string[] lines = new string[0];

            bool expected = false;
            bool actual;
            actual = target.LoadAllData(lines);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadAllDataTest_WhenHeaderFormatIsNotCorrect()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string[] lines = new string[5];
            lines[0] = "asasavsagcsavcsabv";
            lines[1] = "asdbavbnvvvvabsdvasdbnvas";

            bool expected = false;
            bool actual;
            actual = target.LoadAllData(lines);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadAllDataTest_WhenHeaderFormatIsCorrectButDataNotPreviouslyExists()
        {
        
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string[] lines = new string[5];
            lines[0] = "Date, 05/15/2014";
            lines[1] = "GROUP , ACCOUNT, BUYING POWER";
            lines[3] = "";
            lines[2] = "G1 , , 1000000";
            lines[3] = " , SA1 , 2000000";

            bool expected = false;
            bool actual;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Clear();
            actual = target.LoadAllData(lines);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadAllDataTest_WhenHeaderFormatIsCorrectAndDataPreviouslyExists()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string[] lines = new string[5];
            lines[0] = "Date, 05/15/2014";
            lines[1] = "GROUP , ACCOUNT, BUYING POWER";
            lines[3] = "";
            lines[2] = "G1 , , 1000000";
            lines[3] = " , SA1 , 2000000";


            Group g1 = new Group() { Id = "g1", Name = "g1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1", Name = "sa1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1", Name = "sa1", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);
            bool expected = true;
            bool actual;
            actual = target.LoadAllData(lines);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataTest_WhenGroupIsNotExistsBefore()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string dataLine = "G1,,250000";
            List<NodeImportEntity> nodeImportEntityList = new List<NodeImportEntity>();
            int lineNumber = 3;
            string errMsg = string.Empty;
            string errMsgExpected = "Unknown group: [G1]";
            bool expected = false;
            bool actual;
            actual = target.LoadData(dataLine, nodeImportEntityList, lineNumber, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataTest_WhenAccountIsNotExistsBefore()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string dataLine = ",A1,250000";
            List<NodeImportEntity> nodeImportEntityList = new List<NodeImportEntity>();
            int lineNumber = 3;
            string errMsg = string.Empty;
            string errMsgExpected = "Unknown account: [A1]";
            bool expected = false;
            bool actual;
            actual = target.LoadData(dataLine, nodeImportEntityList, lineNumber, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataTest_WhenGroupIsExistsBefore()
        {

            Group g1 = new Group() { Id = "g1", Name = "g1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1", Name = "sa1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1", Name = "sa1", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);
                       
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string dataLine = "g1,,250000";
            List<NodeImportEntity> nodeImportEntityList = new List<NodeImportEntity>();
            int lineNumber = 3;
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            bool expected = true;
            bool actual;
            actual = target.LoadData(dataLine, nodeImportEntityList, lineNumber, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataTest_WhenAccountIsExistsBefore()
        {
            Group g1 = new Group() { Id = "g1", Name = "g1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1", Name = "sa1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1", Name = "sa1", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);

            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string dataLine = ",sa1,250000";
            List<NodeImportEntity> nodeImportEntityList = new List<NodeImportEntity>();
            int lineNumber = 3;
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            bool expected = true;
            bool actual;
            actual = target.LoadData(dataLine, nodeImportEntityList, lineNumber, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataTest_WhenGroupNameHasSpaces()
        {
            Group g1 = new Group() { Id = "g1", Name = "g1 group", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g2 group", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3 group", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1", Name = "sa1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1", Name = "sa1", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);

            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string dataLine = " g1 group , ,250000";
            List<NodeImportEntity> nodeImportEntityList = new List<NodeImportEntity>();
            int lineNumber = 3;
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            bool expected = true;
            bool actual;
            actual = target.LoadData(dataLine, nodeImportEntityList, lineNumber, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataTest_WhenAccountNameHasSpaces()
        {
            Group g1 = new Group() { Id = "g1", Name = "g1 group", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g2 group", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3 group", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1 account", Name = "sa1 account", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1 account", Name = "sa1 account", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);

            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string dataLine = "  ,sa1 account ,3550000";
            List<NodeImportEntity> nodeImportEntityList = new List<NodeImportEntity>();
            int lineNumber = 3;
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            bool expected = true;
            bool actual;
            actual = target.LoadData(dataLine, nodeImportEntityList, lineNumber, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataInColumn_WhenLessThan3Line()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            bool expected = true;
            bool actual = target.LoadDataInColumn(null);
            Assert.AreEqual(expected, actual);

            string[] lines = new string[2];
            actual = target.LoadDataInColumn(lines);
            Assert.AreEqual(expected, actual);


        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataInColumn_WithEmptyDataLine()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            bool expected = true;
            string[] lines = new string[6];
            lines[0] = "";
            lines[1] = "";
            lines[2] = "";
            lines[3] = "";
            lines[4] = "";
            lines[5] = "";
            bool actual = target.LoadDataInColumn(lines);
            Assert.AreEqual(expected, actual);


        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataInColumn_WithInvalidDataLine()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            bool expected = false;
            string[] lines = new string[3];
            lines[0] = "";
            lines[1] = "";
            lines[2] = "GRP2, ACC2, 150000";
            bool actual = target.LoadDataInColumn(lines);
            Assert.AreEqual(expected, actual);

            lines[2] = "GRP2,, 150000, Test";
            actual = target.LoadDataInColumn(lines);
            Assert.AreEqual(expected, actual);

            lines[2] = "GRP2,,";
            actual = target.LoadDataInColumn(lines);
            Assert.AreEqual(expected, actual);



        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void LoadDataInColumn_WithValidDataLine()
        {
            INodeEntity node1 = new Group() { Id = "GRP1", Name = "GRP1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            INodeEntity node2 = new Group() { Id = "GRP2", Name = "GRP2", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            Group node3 = new Group() { Id = "ACC3", Name = "ACC3", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            Account acc3 = new Account() { Id = node3.Id, Name = node3.Id, IsOwnGroup = true, ParentGroup = node3, OwnerId = node3.Id };
            node3.AccountList.Add(acc3);
            node3.IsAccountGroup = true;


            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(node1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(node2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(node3);

            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            bool expected = true;
            string[] lines = new string[3];
            lines[0] = "";
            lines[1] = "";
            lines[2] = "GRP2,, 150000";
            bool actual = target.LoadDataInColumn(lines);
            Assert.AreEqual(expected, actual);

            lines[2] = " GRP2  ,   , 150000 , ";
            actual = target.LoadDataInColumn(lines);
            Assert.AreEqual(expected, actual);

            lines[2] = "   , ACC3  , 150000 , ";
            actual = target.LoadDataInColumn(lines);
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateAndParseExistingGroupAccountTest_WhenGroupNotExists()
        {
            Group g1 = new Group() { Id = "g1", Name = "g1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1", Name = "sa1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1", Name = "sa1", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Clear(); 
            
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);

            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string groupName = "g1";
            string accountId = string.Empty;
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            INodeEntity nodeEntity = null;
            INodeEntity nodeEntityExpected = g1;
            bool expected = true;
            bool actual;
            actual = target.ValidateAndParseExistingGroupAccount(groupName, accountId, out errMsg, out nodeEntity);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(nodeEntityExpected, nodeEntity);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateAndParseExistingGroupAccountTest_WhenGroupExists()
        {
            Group g1 = new Group() { Id = "g1", Name = "g1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1", Name = "sa1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1", Name = "sa1", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);

            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string groupName = "G2REN";
            string accountId = string.Empty;
            string errMsg = string.Empty;
            string errMsgExpected = "Unknown group: [G2REN]";
            INodeEntity nodeEntity = null;
            INodeEntity nodeEntityExpected = null;
            bool expected = false;
            bool actual;
            actual = target.ValidateAndParseExistingGroupAccount(groupName, accountId, out errMsg, out nodeEntity);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(nodeEntityExpected, nodeEntity);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateAndParseExistingGroupAccountTest_WhenAccountExists()
        {
            Group g1 = new Group() { Id = "g1", Name = "g1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1", Name = "sa1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1", Name = "sa1", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Clear();

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);

            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string groupName = string.Empty;
            string accountId = "sa1";
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            INodeEntity nodeEntity = null;
            INodeEntity nodeEntityExpected = sa1;
            bool expected = true;
            bool actual;
            actual = target.ValidateAndParseExistingGroupAccount(groupName, accountId, out errMsg, out nodeEntity);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(nodeEntityExpected, nodeEntity);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateAndParseExistingGroupAccountTest_WhenAccountNotExists()
        {
            Group g1 = new Group() { Id = "g1", Name = "g1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1", Name = "sa1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1", Name = "sa1", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);

            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string groupName = string.Empty;
            string accountId = "sa1ren";
            string errMsg = string.Empty;
            string errMsgExpected = "Unknown account: [sa1ren]";
            INodeEntity nodeEntity = null;
            INodeEntity nodeEntityExpected = null;
            bool expected = false;
            bool actual;
            actual = target.ValidateAndParseExistingGroupAccount(groupName, accountId, out errMsg, out nodeEntity);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(nodeEntityExpected, nodeEntity);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateAndParseExistingGroupAccountTest_WhenAccountParentIsGroup()
        {
            Group g1 = new Group() { Id = "g1", Name = "g1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g2 = new Group() { Id = "g2", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Group g3 = new Group() { Id = "g3", Name = "g3", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            Group sa1 = new Group() { Id = "sa1", Name = "sa1", RiskSetting = new RiskSetting(), AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account a1 = new Account() { Id = "sa1", Name = "sa1", ParentGroup = sa1, IsOwnGroup = true };
            sa1.AccountList.Add(a1);
            sa1.IsAccountGroup = true;

            Account a2 = new Account() { Id = "a2", Name = "a1", ParentGroup = g1};
            g1.AccountList.Add(a2);

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(g3);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(sa1);

            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string groupName = string.Empty;
            string accountId = "a2";
            string errMsg = string.Empty;
            string errMsgExpected = "Not allowed to specify settings for account [a2], which is part of group: [g1]";
            INodeEntity nodeEntity = null;
            INodeEntity nodeEntityExpected = null;
            bool expected = false;
            bool actual;
            actual = target.ValidateAndParseExistingGroupAccount(groupName, accountId, out errMsg, out nodeEntity);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(nodeEntityExpected, nodeEntity);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateColumnHeaderLineTest_WithCorrectText()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string columnHeaderLine = "Group,Account,Buying Power";
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            bool expected = true;
            bool actual;
            actual = target.ValidateColumnHeaderLine(columnHeaderLine, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateColumnHeaderLineTest_WithCorrectTextAndCommaAtTheEnd()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string columnHeaderLine = "Group,Account,Buying Power,";
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            bool expected = true;
            bool actual;
            actual = target.ValidateColumnHeaderLine(columnHeaderLine, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateColumnHeaderLineTest_WithInCorrectText()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string columnHeaderLine = "Group,Account,Buying Power,test";
            string errMsg = string.Empty;
            string errMsgExpected = "Invalid data [test], is exists at the end of column-header"; ;
            bool expected = false;
            bool actual;
            actual = target.ValidateColumnHeaderLine(columnHeaderLine, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);


            columnHeaderLine = "GroupTEST,Account,Buying Power";
            expected = false;
            actual = target.ValidateColumnHeaderLine(columnHeaderLine, out errMsg);
            Assert.AreEqual(expected, actual);

            columnHeaderLine = "GroupTEST,AccountTEST,Buying Power";
            expected = false;
            actual = target.ValidateColumnHeaderLine(columnHeaderLine, out errMsg);
            Assert.AreEqual(expected, actual);

            columnHeaderLine = "GroupTEST,Account,Buying Power TEST";
            expected = false;
            actual = target.ValidateColumnHeaderLine(columnHeaderLine, out errMsg);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateColumnNameTest_ForGroupColumn()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string columnHeader = "Group";
            string matchedColumn = "GROUP";
            bool expected = true;
            bool actual;
            actual = target.ValidateColumnName(columnHeader, matchedColumn);
            Assert.AreEqual(expected, actual);

            columnHeader = "Group";
            matchedColumn = "GR OUP";
            expected = true;
            actual = target.ValidateColumnName(columnHeader, matchedColumn);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateColumnNameTest_ForAccountColumn()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string columnHeader = "Account";
            string matchedColumn = "ACCOUNT";
            bool expected = true;
            bool actual;
            actual = target.ValidateColumnName(columnHeader, matchedColumn);
            Assert.AreEqual(expected, actual);

            columnHeader = " Acc ount ";
            matchedColumn = "  Account ";
            expected = true;
            actual = target.ValidateColumnName(columnHeader, matchedColumn);
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateColumnNameTest_ForBuyingPowerColumn()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string columnHeader = "Buying Power";
            string matchedColumn = " BuyingPOWER";
            bool expected = true;
            bool actual;
            actual = target.ValidateColumnName(columnHeader, matchedColumn);
            Assert.AreEqual(expected, actual);

            columnHeader = " Buying   POWER ";
            matchedColumn = "  BUYING     POWER ";
            expected = true;
            actual = target.ValidateColumnName(columnHeader, matchedColumn);
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateColumnNameTest_WithIncorrectColumn()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string columnHeader = "Buying Power";
            string matchedColumn = "100000";
            bool expected = false;
            bool actual;
            actual = target.ValidateColumnName(columnHeader, matchedColumn);
            Assert.AreEqual(expected, actual);

            columnHeader = " Group ";
            matchedColumn = "  Default Group";
            expected = false;
            actual = target.ValidateColumnName(columnHeader, matchedColumn);
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateDateLineTest()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string dateHeaderline = "Date , 05/15/2014";
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            bool expected = true;
            bool actual;
            actual = target.ValidateDateLine(dateHeaderline, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);

            dateHeaderline = "Date , 5/5/2014 15:05,";
            expected = true;
            actual = target.ValidateDateLine(dateHeaderline, out errMsg);
            Assert.AreEqual(expected, actual);

            
            dateHeaderline = "     Date , 05/15/2014 15:25,";
            expected = true;
            actual = target.ValidateDateLine(dateHeaderline, out errMsg);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateDateLineTest_IncorrectTest()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string dateHeaderline = "Date , ";
            string errMsg = string.Empty;
            bool expected = false;
            bool actual;
            actual = target.ValidateDateLine(dateHeaderline, out errMsg);
            Assert.AreEqual(expected, actual);

            dateHeaderline = "DAT , 5/5/2014 15:05,";
            expected = false;
            actual = target.ValidateDateLine(dateHeaderline, out errMsg);
            Assert.AreEqual(expected, actual);


            dateHeaderline = "     Dated , 05/15/2014 15:25,";
            expected = false;
            actual = target.ValidateDateLine(dateHeaderline, out errMsg);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateHeaderTest()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string dateHeaderline = "Date, 05/15/2014";
            string columnHeaderLine = "Group,Account, Buying Power";
            string errMsg = string.Empty;
            string errMsgExpected = string.Empty;
            bool expected = true;
            bool actual;
            actual = target.ValidateHeader(dateHeaderline, columnHeaderLine, out errMsg);
            Assert.AreEqual(errMsgExpected, errMsg);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ValidateHeaderTest_IncorrectHeaders()
        {
            MainMenuUserControl_Accessor target = new MainMenuUserControl_Accessor();
            string errMsg = string.Empty;

            string dateHeaderline = "Date, 05/15/2014";
            string columnHeaderLine = "Group,Account, 10000";
            bool expected = false;
            bool actual = target.ValidateHeader(dateHeaderline, columnHeaderLine, out errMsg);
            Assert.AreEqual(expected, actual);

            dateHeaderline = ", 05/15/2014";
            columnHeaderLine = "Group,Account, 10000";
            expected = false;
            actual = target.ValidateHeader(dateHeaderline, columnHeaderLine, out errMsg);
            Assert.AreEqual(expected, actual);

            dateHeaderline = ", 05/15/2014";
            columnHeaderLine = "Group,Account, Buying Power";
            expected = false;
            actual = target.ValidateHeader(dateHeaderline, columnHeaderLine, out errMsg);
            Assert.AreEqual(expected, actual);
        
        }
    
    }
}
