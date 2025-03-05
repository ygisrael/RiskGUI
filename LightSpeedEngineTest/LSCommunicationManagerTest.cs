using EZX.LightSpeedEngine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EZX.LightSpeedEngine.Config;
using EZXLib;
using System.Collections.Generic;
using LightSpeedEngineTest.Utils;
using EZX.LightspeedEngine.Entity;

namespace LightSpeedEngineTest
{


    [TestClass()]
    public class LSCommunicationManagerTest
    {
        LSEngine engine = new LSEngine();
        ConfigInfo config = new ConfigInfo();
        LSConnectionInfo connectionInfo = new LSConnectionInfo();
        LSCommunicationManager lsComMgr;

        [TestInitialize]
        public void Setup()
        {
            connectionInfo.Company = "TESTCOMPANY";
            connectionInfo.Host = "testhost.abc.awqx";
            connectionInfo.Port = 1100;
            connectionInfo.IsSSL = false;
            config.LSConnectionInfo = connectionInfo;

            lsComMgr = new LSCommunicationManager(engine);            
            engine.Init(config, lsComMgr);

        }

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

        [TestMethod()]
        public void onReadGroupAccountsTest()
        {
            ReadGroupAccounts readGroupAccounts  = new ReadGroupAccounts();
            
            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            groupAccountList.Add(defaultGroup);

            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount group2 = GroupAccountUtil.CreateNewGroupAccount("g2", "Group B", 0, new Properties(), null);
            GroupAccount group3 = GroupAccountUtil.CreateNewGroupAccount("g3", "Group C", 0, new Properties(), null);
            groupAccountList.Add(group1);
            groupAccountList.Add(group2);
            groupAccountList.Add(group3);

            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");
            GroupAccount accontB1 = GroupAccountUtil.CreateNewGroupAccount("gb1", "Account B1", 0, null, "g2");
            GroupAccount accontB2 = GroupAccountUtil.CreateNewGroupAccount("gb2", "Account B2", 0, null, "g2");
            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(accontA3);
            groupAccountList.Add(accontB1);
            groupAccountList.Add(accontB2);

            GroupAccount standAloneAccontS1 = GroupAccountUtil.CreateNewGroupAccount("sa1", "Account S1", 0, new Properties(), "sa1");
            GroupAccount standAloneAccontS2 = GroupAccountUtil.CreateNewGroupAccount("sa2", "Account S2", 0, new Properties(), "sa2");
            groupAccountList.Add(standAloneAccontS1);
            groupAccountList.Add(standAloneAccontS2);


            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            Assert.AreEqual(groupAccountList, readGroupAccounts.GroupAccounts,"Failed to set private property");
            string encodedReadGroupAccountsText = readGroupAccounts.encode();
            encodedReadGroupAccountsText = "00217702136000" + StringConstants.DELIM_VALUE_CTRLA +""+encodedReadGroupAccountsText + StringConstants.DELIM_VALUE_CTRLC;
            PrivateMemberUtil.CallPrivateMethod<CommunicationManager>(lsComMgr.ComMgr, "ProcessResponse", encodedReadGroupAccountsText);

            Assert.IsNotNull(engine.DataManager.AccountGroupList, "engine.DataManager.AccountGroupList is null!");
            
            int expected = 6;
            int actual = engine.DataManager.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "Group count is not matching corectly");

            expected = 0;
            actual = (engine.DataManager).GetDefaultGroupNode().AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Defailt-Group is not matching corectly");


            expected = 3;
            actual = engine.DataManager.FindGroupNode("g1").AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Group A is not matching corectly");

            expected = 2;
            actual = engine.DataManager.FindGroupNode("g2").AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Group B is not matching corectly");

            expected = 0;
            actual = engine.DataManager.FindGroupNode("g3").AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Group C is not matching corectly");

            Assert.IsNull(engine.DataManager.FindGroupNode("sa1").AccountList, "Account count of Stand-Alone-1 is not matching corectly");

            Assert.IsNull(engine.DataManager.FindGroupNode("sa2").AccountList, "Account count of Stand-Alone-2 is not matching corectly");

        }

        [TestMethod()]
        public void onReadGroupAccounts_AccountLoadedFirstTest()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();

            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            groupAccountList.Add(defaultGroup);

            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount group2 = GroupAccountUtil.CreateNewGroupAccount("g2", "Group B", 0, new Properties(), null);

            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");
            GroupAccount accontB1 = GroupAccountUtil.CreateNewGroupAccount("gb1", "Account B1", 0, null, "g2");
            GroupAccount accontB2 = GroupAccountUtil.CreateNewGroupAccount("gb2", "Account B2", 0, null, "g2");

            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(accontB1);

            groupAccountList.Add(group1);
            groupAccountList.Add(group2);

            groupAccountList.Add(accontB2);
            groupAccountList.Add(accontA3);



            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            Assert.AreEqual(groupAccountList, readGroupAccounts.GroupAccounts, "Failed to set private property");
            string encodedReadGroupAccountsText = readGroupAccounts.encode();
            encodedReadGroupAccountsText = "00217702136000" + StringConstants.DELIM_VALUE_CTRLA + "" + encodedReadGroupAccountsText + StringConstants.DELIM_VALUE_CTRLC;
            PrivateMemberUtil.CallPrivateMethod<CommunicationManager>(lsComMgr.ComMgr, "ProcessResponse", encodedReadGroupAccountsText);

            Assert.IsNotNull(engine.DataManager.AccountGroupList, "engine.DataManager.AccountGroupList is null!");

            int expected = 3;
            int actual = engine.DataManager.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "Group count is not matching corectly");

            expected = 0;
            actual = engine.DataManager.GetDefaultGroupNode().AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Defailt-Group is not matching corectly");


            expected = 2;
            actual = engine.DataManager.FindGroupNode("g2").AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Group B is not matching corectly");

            expected = 3;
            actual = engine.DataManager.FindGroupNode("g1").AccountList.Count; 
            Assert.AreEqual(expected, actual, "Account count of Group A is not matching corectly");

        }


        [TestMethod()]
        public void onReadGroupAccounts_HavingAccountInDefaultGroupTest()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();

            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            groupAccountList.Add(defaultGroup);

            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount group2 = GroupAccountUtil.CreateNewGroupAccount("g2", "Group B", 0, new Properties(), null);
            GroupAccount group3 = GroupAccountUtil.CreateNewGroupAccount("g3", "Group C", 0, new Properties(), null);
            groupAccountList.Add(group1);
            groupAccountList.Add(group2);
            groupAccountList.Add(group3);

            GroupAccount accontDA1 = GroupAccountUtil.CreateNewGroupAccount("gda1", "Default Account A1", 0, null, "d1");
            GroupAccount accontDA2 = GroupAccountUtil.CreateNewGroupAccount("gda2", "Default Account A2", 0, null, "d1");
            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");
            GroupAccount accontB1 = GroupAccountUtil.CreateNewGroupAccount("gb1", "Account B1", 0, null, "g2");
            GroupAccount accontB2 = GroupAccountUtil.CreateNewGroupAccount("gb2", "Account B2", 0, null, "g2");
            groupAccountList.Add(accontDA1);
            groupAccountList.Add(accontDA2);
            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(accontA3);
            groupAccountList.Add(accontB1);
            groupAccountList.Add(accontB2);

            GroupAccount standAloneAccontS1 = GroupAccountUtil.CreateNewGroupAccount("sa1", "Account S1", 0, new Properties(), "sa1");
            GroupAccount standAloneAccontS2 = GroupAccountUtil.CreateNewGroupAccount("sa2", "Account S2", 0, new Properties(), "sa2");
            groupAccountList.Add(standAloneAccontS1);
            groupAccountList.Add(standAloneAccontS2);


            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            Assert.AreEqual(groupAccountList, readGroupAccounts.GroupAccounts, "Failed to set private property");
            string encodedReadGroupAccountsText = readGroupAccounts.encode();
            encodedReadGroupAccountsText = "00217702136000" + StringConstants.DELIM_VALUE_CTRLA + "" + encodedReadGroupAccountsText + StringConstants.DELIM_VALUE_CTRLC;
            PrivateMemberUtil.CallPrivateMethod<CommunicationManager>(lsComMgr.ComMgr, "ProcessResponse", encodedReadGroupAccountsText);

            Assert.IsNotNull(engine.DataManager.AccountGroupList, "engine.DataManager.AccountGroupList is null!");

            int expected = 6;
            int actual = engine.DataManager.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "Group count is not matching corectly");

            expected = 2;
            actual = engine.DataManager.GetDefaultGroupNode().AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Defailt-Group is not matching corectly");


            expected = 3;
            actual = engine.DataManager.FindGroupNode("g1").AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Group A is not matching corectly");

            expected = 2;
            actual = engine.DataManager.FindGroupNode("g2").AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Group B is not matching corectly");

            expected = 0;
            actual = engine.DataManager.FindGroupNode("g3").AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Group C is not matching corectly");

            Assert.IsNull(engine.DataManager.FindGroupNode("sa1").AccountList, "Account count of Stand-Alone-1 is not matching corectly");

            Assert.IsNull(engine.DataManager.FindGroupNode("sa2").AccountList, "Account count of Stand-Alone-2 is not matching corectly");

        }

        [TestMethod()]
        public void onReadGroupAccounts_AccountLoadedFirst_HavingAccountInDefaultGroupTest()
        {
            ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();

            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount accontDA1 = GroupAccountUtil.CreateNewGroupAccount("gda1", "Default Account A1", 0, null, "d1");
            groupAccountList.Add(accontDA1);
            GroupAccount accontDA11 = GroupAccountUtil.CreateNewGroupAccount("gda11", "Default Account A11", 0, null, "d1");
            groupAccountList.Add(accontDA11);


            GroupAccount defaultGroup = GroupAccountUtil.CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            groupAccountList.Add(defaultGroup);



            GroupAccount group1 = GroupAccountUtil.CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount group2 = GroupAccountUtil.CreateNewGroupAccount("g2", "Group B", 0, new Properties(), null);

            GroupAccount accontA1 = GroupAccountUtil.CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = GroupAccountUtil.CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = GroupAccountUtil.CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");
            GroupAccount accontB1 = GroupAccountUtil.CreateNewGroupAccount("gb1", "Account B1", 0, null, "g2");
            GroupAccount accontB2 = GroupAccountUtil.CreateNewGroupAccount("gb2", "Account B2", 0, null, "g2");
            GroupAccount accontDA2 = GroupAccountUtil.CreateNewGroupAccount("gda2", "Default Account A2", 0, null, "d1");

            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(accontB1);

            groupAccountList.Add(group1);
            groupAccountList.Add(group2);

            groupAccountList.Add(accontB2);
            groupAccountList.Add(accontA3);
            groupAccountList.Add(accontDA2);



            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);
            Assert.AreEqual(groupAccountList, readGroupAccounts.GroupAccounts, "Failed to set private property");
            string encodedReadGroupAccountsText = readGroupAccounts.encode();
            encodedReadGroupAccountsText = "00217702136000" + StringConstants.DELIM_VALUE_CTRLA + "" + encodedReadGroupAccountsText + StringConstants.DELIM_VALUE_CTRLC;
            PrivateMemberUtil.CallPrivateMethod<CommunicationManager>(lsComMgr.ComMgr, "ProcessResponse", encodedReadGroupAccountsText);

            Assert.IsNotNull(engine.DataManager.AccountGroupList, "engine.DataManager.AccountGroupList is null!");

            int expected = 3;
            int actual = engine.DataManager.AccountGroupList.Count;
            Assert.AreEqual(expected, actual, "Group count is not matching corectly");

            expected = 3;
            actual = engine.DataManager.GetDefaultGroupNode().AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Defailt-Group is not matching corectly");


            expected = 2;
            actual = (engine.DataManager.FindGroupNode("g2") as Group).AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Group B is not matching corectly");

            expected = 3;
            actual = (engine.DataManager.FindGroupNode("g1") as Group).AccountList.Count;
            Assert.AreEqual(expected, actual, "Account count of Group A is not matching corectly");

        }



    }
}
