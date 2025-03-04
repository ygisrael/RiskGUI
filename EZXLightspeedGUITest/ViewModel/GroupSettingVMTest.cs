using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EZXLightspeedGUI;
using EZX.LightspeedEngine.Entity;
using EZXLib;
using EZXLightspeedGUI.ViewModel;

namespace EZXLightspeedGUITest.ViewModel
{
    [TestClass()]
    public class GroupSettingVMTest
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
        public void UpdateGroupAccountSettingTest()
        {
            string id = "g1";
            string name = "Group1";

            CreateGroupAccount groupAccount = new CreateGroupAccount();
            groupAccount.MyID = "1212121";
            groupAccount.ReturnCode = 0;
            groupAccount.ReturnDesc = string.Empty;
            groupAccount.GroupAccount = new GroupAccount();
            groupAccount.GroupAccount.Id = id;
            groupAccount.GroupAccount.DisplayName = name;
            groupAccount.GroupAccount.OwnerID = null;
            groupAccount.GroupAccount.Settings = new Properties();
            groupAccount.GroupAccount.Settings.PropertyMap = new TagValueMsg();
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues = new System.Collections.Hashtable();
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.CREDIT_LIMIT_DBL, "100000");
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_DUPES_CSV, "10,10000");
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_NOTIONAL_INT, "10000");
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_PRICE_DIFF_PCT, "10");
            App.AppManager.GUILSEngine.onCreateGroupAccount(groupAccount);
            Assert.IsNotNull(App.AppManager.GUILSEngine.DataManager.AccountGroupList);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);

            GroupSettingVM vm = new GroupSettingVM();
            vm.Init((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting, (App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group));
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.CreditLimit, vm.SelectedRiskSetting.CreditLimit);
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.MaxDuplicateOrder, vm.SelectedRiskSetting.MaxDuplicateOrder);
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.DuplicateOrderTimeInterval, vm.SelectedRiskSetting.DuplicateOrderTimeInterval);
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.MaxNotionalPerOrder, vm.SelectedRiskSetting.MaxNotionalPerOrder);
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.MaxPriceDiff, vm.SelectedRiskSetting.MaxPriceDiff);

            string creditLimit = "200000";
            string maxDuplicate = "30,30000";
            string maxAmount = "400000";
            string maxpriceDiff = "50";
            string myId = "1212121212";
            int returnCode = 0;
            string returnDesc = string.Empty;

            UpdateGroupSettings updateGroupSettingsResponse = CreateAndGetUpdateSettingObj(id, name, creditLimit, maxDuplicate, maxAmount, maxpriceDiff, myId, returnCode, returnDesc);
            App.AppManager.GUILSEngine.onUpdateGroupSettings(updateGroupSettingsResponse);
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.CreditLimit, vm.SelectedRiskSetting.CreditLimit);
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.MaxDuplicateOrder, vm.SelectedRiskSetting.MaxDuplicateOrder);
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.DuplicateOrderTimeInterval, vm.SelectedRiskSetting.DuplicateOrderTimeInterval);
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.MaxNotionalPerOrder, vm.SelectedRiskSetting.MaxNotionalPerOrder);
            Assert.AreEqual((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting.MaxPriceDiff, vm.SelectedRiskSetting.MaxPriceDiff);
        
        }

        [TestMethod]
        public void UpdateGroupNameTest()
        {
            string id = "g1";
            string name = "Group1";

            CreateGroupAccount groupAccount = new CreateGroupAccount();
            groupAccount.MyID = "1212121";
            groupAccount.ReturnCode = 0;
            groupAccount.ReturnDesc = string.Empty;
            groupAccount.GroupAccount = new GroupAccount();
            groupAccount.GroupAccount.Id = id;
            groupAccount.GroupAccount.DisplayName = name;
            groupAccount.GroupAccount.OwnerID = null;
            groupAccount.GroupAccount.Settings = new Properties();
            groupAccount.GroupAccount.Settings.PropertyMap = new TagValueMsg();
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues = new System.Collections.Hashtable();
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.CREDIT_LIMIT_DBL, "100000");
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_DUPES_CSV, "10,10000");
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_NOTIONAL_INT, "10000");
            groupAccount.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_PRICE_DIFF_PCT, "10");
            App.AppManager.GUILSEngine.onCreateGroupAccount(groupAccount);
            Assert.IsNotNull(App.AppManager.GUILSEngine.DataManager.AccountGroupList);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);

            GroupSettingVM vm = new GroupSettingVM();
            vm.Init((App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).RiskSetting, (App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group));
            Assert.AreEqual("GROUP: "+(App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).Name, vm.Heading);
            string myId = "1212121212";
            int returnCode = 0;
            string returnDesc = string.Empty;

            UpdateGroupName updateGroupNameResponse = CreateAndGetUpdateNameObj(id, "Group1Ren", myId, returnCode, returnDesc);
            App.AppManager.GUILSEngine.onUpdateGroupName(updateGroupNameResponse);
            Assert.AreEqual("GROUP: " + (App.AppManager.GUILSEngine.DataManager.AccountGroupList[0] as Group).Name, vm.Heading);

        }

        private UpdateGroupName CreateAndGetUpdateNameObj(string groupId, string groupName, string myId, int returnCode, string returnDesc)
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
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.CREDIT_LIMIT_DBL, creditLimit);
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_DUPES_CSV, maxDuplicate);
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_NOTIONAL_INT, maxAmount);
            updateGroupSettingsResponse.GroupAccount.Settings.PropertyMap.tagValues.Add(CompanySetting.MAX_PRICE_DIFF_PCT, maxpriceDiff);
            return updateGroupSettingsResponse;
        }

    }
}
