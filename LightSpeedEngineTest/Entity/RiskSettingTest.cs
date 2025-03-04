using EZX.LightspeedEngine.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using EZXLib;

namespace LightSpeedEngineTest
{


    [TestClass()]
    public class RiskSettingTest
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

        //[TestMethod()]
        //public void RiskSettingConstructorTest()
        //{
        //    Hashtable clientCompanySetting = null; // TODO: Initialize to an appropriate value
        //    RiskSetting target = new RiskSetting(clientCompanySetting);
        //    int expectedInt = int.MaxValue;
        //    bool? expectedBool = null;

        //    Assert.AreEqual(expectedInt, target.CreditLimit);
        //    Assert.AreEqual(expectedInt, target.AggregateExecutedAmount);
        //    Assert.AreEqual(expectedInt, target.DuplicateOrderTimeInterval);
        //    Assert.AreEqual(expectedInt, target.MaxDuplicateOrder);
        //    Assert.AreEqual(expectedInt, target.MaxNetTraded);
        //    Assert.AreEqual(expectedInt, target.MaxNotionalPerOrder);
        //    Assert.AreEqual(expectedInt, target.MaxSharesPerOrder);
        //    Assert.AreEqual(expectedBool, target.MocLocAllowed);
        //    Assert.AreEqual(null, target.LatestTime);
        //    Assert.AreEqual(expectedBool, target.EnableSellShort);
        //    Assert.AreEqual(expectedBool, target.EnableSellShortExempt);
        //    Assert.AreEqual(expectedInt, target.MaxPriceDiff);
        //}


        [TestMethod()]
        public void RiskSettingConstructorTestWithValidCompanySetting()
        {
            Hashtable clientCompanySetting = new Hashtable();
            //clientCompanySetting.Add(CompanySetting.ALLOW_SELL_SHORT_BOOL, true);
            //clientCompanySetting.Add(CompanySetting.ALLOW_SELL_SHORT_EXEMP_BOOL, true);
            //clientCompanySetting.Add(CompanySetting.CREDIT_LIMIT_DBL, 100000);
            clientCompanySetting.Add(CompanySetting.CLIENT_CREDIT_LIMIT_DBL, 100000);
            //clientCompanySetting.Add(CompanySetting.LOCATES_CSV, "1,45,45,665");
            //clientCompanySetting.Add(CompanySetting.MAX_AGGREGATED_EXECUTE_INT, 50000);
            clientCompanySetting.Add(CompanySetting.MAX_DUPES_CSV, "25,25000");
            //clientCompanySetting.Add(CompanySetting.MAX_NET_TRADE_INT, 25000);
            clientCompanySetting.Add(CompanySetting.MAX_NOTIONAL_INT, 55000);
            //clientCompanySetting.Add(CompanySetting.MAX_SHARES_LIMIT_INT, 10000);
            //clientCompanySetting.Add(CompanySetting.ON_CLOSE_CSV, " true, 15:25 ");
            //clientCompanySetting.Add(CompanySetting.POSITION_CSV, " A, 15, 22");
            clientCompanySetting.Add(CompanySetting.MAX_PRICE_DIFF_PCT, "65");

            RiskSetting target = new RiskSetting(clientCompanySetting);

            Assert.AreEqual(100000, target.CreditLimit);
            //Assert.AreEqual(50000, target.AggregateExecutedAmount);
            Assert.AreEqual(25000, target.DuplicateOrderTimeInterval);
            Assert.AreEqual(25, target.MaxDuplicateOrder);
            //Assert.AreEqual(25000, target.MaxNetTraded);
            Assert.AreEqual(55000, target.MaxNotionalPerOrder);
            //Assert.AreEqual(10000, target.MaxSharesPerOrder);
            //Assert.AreEqual(true, target.MocLocAllowed);
            //Assert.AreEqual("15:25", target.LatestTime);
            //Assert.AreEqual(true, target.EnableSellShort);
            //Assert.AreEqual(true, target.EnableSellShortExempt);
            Assert.AreEqual(65, target.MaxPriceDiff);

            bool expectedIsSetValue = true;
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInAggregateExecutedAmount);
            Assert.AreEqual(expectedIsSetValue, target.IsValueSetInCreditLimit);
            Assert.AreEqual(expectedIsSetValue, target.IsValueSetInDuplicateOrderTimeInterval);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInEnableSellShort);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInEnableSellShortExempt);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInLatestTime);
            Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMaxDuplicateOrder);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMaxNetTraded);
            Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMaxNotionalPerOrder);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMaxSharesPerOrder);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMocLocAllowed);


        }

        //[TestMethod()]
        //public void RiskSettingConstructorTest2WithValidCompanySetting()
        //{
        //    Hashtable clientCompanySetting = new Hashtable();
        //    clientCompanySetting.Add(CompanySetting.ALLOW_SELL_SHORT_BOOL, false);
        //    clientCompanySetting.Add(CompanySetting.ALLOW_SELL_SHORT_EXEMP_BOOL, false);
        //    clientCompanySetting.Add(CompanySetting.CREDIT_LIMIT_INT, "$ 10,000,0.00");
        //    clientCompanySetting.Add(CompanySetting.LOCATES_CSV, "1,45,45,665");
        //    clientCompanySetting.Add(CompanySetting.MAX_AGGREGATED_EXECUTE_INT, 50000);
        //    clientCompanySetting.Add(CompanySetting.MAX_DUPES_CSV, "25,25000");
        //    clientCompanySetting.Add(CompanySetting.MAX_NET_TRADE_INT, 25000);
        //    clientCompanySetting.Add(CompanySetting.MAX_NOTIONAL_INT, 55000);
        //    clientCompanySetting.Add(CompanySetting.MAX_SHARES_LIMIT_INT, 10000);
        //    clientCompanySetting.Add(CompanySetting.ON_CLOSE_CSV, " false, 15:25 ");
        //    clientCompanySetting.Add(CompanySetting.POSITION_CSV, " A, 15, 22");
        //    clientCompanySetting.Add(CompanySetting.MAX_PRICE_DIFF_PCT, "75");

        //    RiskSetting target = new RiskSetting(clientCompanySetting);

        //    Assert.AreEqual(100000, target.CreditLimit);
        //    Assert.AreEqual(50000, target.AggregateExecutedAmount);
        //    Assert.AreEqual(25000, target.DuplicateOrderTimeInterval);
        //    Assert.AreEqual(25, target.MaxDuplicateOrder);
        //    Assert.AreEqual(25000, target.MaxNetTraded);
        //    Assert.AreEqual(55000, target.MaxNotionalPerOrder);
        //    Assert.AreEqual(10000, target.MaxSharesPerOrder);
        //    Assert.AreEqual(false, target.MocLocAllowed);
        //    Assert.AreEqual(string.Empty, target.LatestTime);
        //    Assert.AreEqual(false, target.EnableSellShort);
        //    Assert.AreEqual(false, target.EnableSellShortExempt);
        //    Assert.AreEqual(75, target.MaxPriceDiff);
        //}

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetBoolValueFromEmptyTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = string.Empty;
            bool expected = false;
            bool? actual;
            actual = target.GetBoolValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetBoolValueFromNullTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = null;
            bool? expected = null;
            bool? actual;
            actual = target.GetBoolValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetBoolValueFromFALSETextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "FALSE";
            bool expected = false;
            bool? actual;
            actual = target.GetBoolValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetBoolValueFromLowerFalseTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "false";
            bool expected = false;
            bool? actual = true;
            actual = target.GetBoolValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetBoolValueFromTRUETextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "TRUE";
            bool expected = true;
            bool? actual;
            actual = target.GetBoolValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetBoolValueFrom1TextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "1";
            bool expected = true;
            bool? actual;
            actual = target.GetBoolValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetBoolValueFrom0TextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "0";
            bool expected = false;
            bool? actual = true;
            actual = target.GetBoolValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetBoolValueFromNULLTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "NULL";
            bool? expected = null;
            bool? actual = true;
            actual = target.GetBoolValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetBoolValueFromLowerTrueTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "true";
            bool expected = true;
            bool? actual;
            actual = target.GetBoolValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetINTValueFromEmptyTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "";
            int expected = int.MaxValue;
            int actual;
            actual = target.GetINTValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetINTValueFromNullTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = null;
            int expected = int.MaxValue;
            int actual;
            actual = target.GetINTValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetINTValueFromInvalidTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "absfef";
            int expected = int.MaxValue;
            int actual;
            actual = target.GetINTValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetINTValueFromValidTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "25000";
            int expected = 25000;
            int actual;
            actual = target.GetINTValueFromText(keyValue);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetINTValueFromValidFormatedTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValue = "25,000.00";
            int expected = 25000;
            int actual;
            actual = target.GetINTValueFromText(keyValue);
            Assert.AreEqual(expected, actual);

            keyValue = "$ 25,000.00";
            expected = 25000;
            actual = target.GetINTValueFromText(keyValue);
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetTextFromListForNullListTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            List<string> csvTextList = null;
            int index = 0;
            string expected = string.Empty;
            string actual;
            actual = target.GetTextFromList(csvTextList, index);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetTextFromListForEmptyListTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            List<string> csvTextList = new List<string>();
            int index = 0;
            string expected = string.Empty;
            string actual;
            actual = target.GetTextFromList(csvTextList, index);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetTextFromListForValidListTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            List<string> csvTextList = new List<string>();
            string expected1 = "Text1";
            string expected2 = "Text2";
            csvTextList.Add(expected1);
            csvTextList.Add(expected2);
            int index = 0;
            string actual;
            actual = target.GetTextFromList(csvTextList, index);
            Assert.AreEqual(expected1, actual);

            actual = target.GetTextFromList(csvTextList, ++index);
            Assert.AreEqual(expected2, actual);
        }


        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetTextFromListForInvalidListTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            List<string> csvTextList = new List<string>();
            string expected1 = "Text1";
            string expected = "";
            csvTextList.Add(expected1);
            csvTextList.Add(expected);
            int index = 0;
            string actual;
            actual = target.GetTextFromList(csvTextList, index);
            Assert.AreEqual(expected1, actual);

            actual = target.GetTextFromList(csvTextList, ++index);
            Assert.AreEqual(expected, actual);

            actual = target.GetTextFromList(csvTextList, ++index);
            Assert.AreEqual(expected, actual);

            actual = target.GetTextFromList(csvTextList, ++index);
            Assert.AreEqual(expected, actual);

            actual = target.GetTextFromList(csvTextList, ++index);
            Assert.AreEqual(expected, actual);

            csvTextList.Clear();
            actual = target.GetTextFromList(csvTextList, 0);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetTextListFromCVSTextTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValueCSV = "10,100,250";
            List<string> duplicatePropertyList = new List<string>();
            int numberOfPropertyToGetFromCSV = 3;
            string expected1 = "10";
            string expected2 = "100";
            string expected3 = "250";
            int expectedListCount = 3;

            List<string> actual;
            actual = target.GetTextListFromCVSText(keyValueCSV, duplicatePropertyList, numberOfPropertyToGetFromCSV);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
            Assert.AreEqual(expected3, actual[2]);
            Assert.AreEqual(expectedListCount, actual.Count);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetTextListFromCVSTextHavingSpaceTest()
        {
            Hashtable companySetting = new Hashtable();
            RiskSetting_Accessor target = new RiskSetting_Accessor(companySetting);
            string keyValueCSV = "10  ,  100  ,250  ";
            List<string> duplicatePropertyList = new List<string>();
            int numberOfPropertyToGetFromCSV = 3;
            string expected1 = "10";
            string expected2 = "100";
            string expected3 = "250";
            int expectedListCount = 3;

            List<string> actual;
            actual = target.GetTextListFromCVSText(keyValueCSV, duplicatePropertyList, numberOfPropertyToGetFromCSV);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
            Assert.AreEqual(expected3, actual[2]);
            Assert.AreEqual(expectedListCount, actual.Count);


            keyValueCSV = ",100,";
            duplicatePropertyList = new List<string>();
            numberOfPropertyToGetFromCSV = 3;
            expected1 = string.Empty;
            expected2 = "100";
            expected3 = "";
            expectedListCount = 3;

            actual = target.GetTextListFromCVSText(keyValueCSV, duplicatePropertyList, numberOfPropertyToGetFromCSV);
            Assert.AreEqual(expected1, actual[0]);
            Assert.AreEqual(expected2, actual[1]);
            Assert.AreEqual(expected3, actual[2]);
            Assert.AreEqual(expectedListCount, actual.Count);

        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void SetDefaultTest()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
            target.SetDefault();
            int expectedInt = 0;
            bool? expectedBool = null;
            bool expectedIsSetValue = true;
            Assert.AreEqual(expectedInt, target.CreditLimit);
            //Assert.AreEqual(expectedInt, target.AggregateExecutedAmount);
            Assert.AreEqual(expectedInt, target.DuplicateOrderTimeInterval);
            Assert.AreEqual(expectedInt, target.MaxDuplicateOrder);
            //Assert.AreEqual(expectedInt, target.MaxNetTraded);
            Assert.AreEqual(expectedInt, target.MaxNotionalPerOrder);
            //Assert.AreEqual(expectedInt, target.MaxSharesPerOrder);
            //Assert.AreEqual(expectedBool, target.MocLocAllowed);
            //Assert.AreEqual(null, target.LatestTime);
            //Assert.AreEqual(expectedBool, target.EnableSellShort);
            //Assert.AreEqual(expectedBool, target.EnableSellShortExempt);

            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInAggregateExecutedAmount);
            Assert.AreEqual(expectedIsSetValue, target.IsValueSetInCreditLimit);
            Assert.AreEqual(expectedIsSetValue, target.IsValueSetInDuplicateOrderTimeInterval);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInEnableSellShort);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInEnableSellShortExempt);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInLatestTime);
            Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMaxDuplicateOrder);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMaxNetTraded);
            Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMaxNotionalPerOrder);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMaxSharesPerOrder);
            //Assert.AreEqual(expectedIsSetValue, target.IsValueSetInMocLocAllowed);

        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void SetValueIntoDuplicatePropertiesFromTextTest()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
            string keyValueCSV = "10,10000";
            int expectedDuplicateOrder = 10;
            int expectedDupInterval = 10000;

            target.SetValueIntoDuplicatePropertiesFromText(keyValueCSV);

            int actaulDuplicateOrder = target.MaxDuplicateOrder;
            int actaulDupInterval = target.DuplicateOrderTimeInterval;

            Assert.AreEqual(expectedDuplicateOrder, actaulDuplicateOrder);
            Assert.AreEqual(expectedDupInterval, actaulDupInterval);


        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void SetValueIntoDuplicatePropertiesFrom1PropertTextTest()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
            string keyValueCSV = "10";
            int expectedDuplicateOrder = 10;
            int expectedDupInterval = int.MaxValue;

            target.SetValueIntoDuplicatePropertiesFromText(keyValueCSV);

            int actaulDuplicateOrder = target.MaxDuplicateOrder;
            int actaulDupInterval = target.DuplicateOrderTimeInterval;

            Assert.AreEqual(expectedDuplicateOrder, actaulDuplicateOrder);
            Assert.AreEqual(expectedDupInterval, actaulDupInterval);

            keyValueCSV = ",1000";
            expectedDuplicateOrder = int.MaxValue;
            expectedDupInterval = 1000;

            target.SetValueIntoDuplicatePropertiesFromText(keyValueCSV);

            actaulDuplicateOrder = target.MaxDuplicateOrder;
            actaulDupInterval = target.DuplicateOrderTimeInterval;

            Assert.AreEqual(expectedDuplicateOrder, actaulDuplicateOrder);
            Assert.AreEqual(expectedDupInterval, actaulDupInterval);

        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void SetValueIntoDuplicatePropertiesHavingSpacesTest()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
            string keyValueCSV = "10   ,   1000";
            int expectedDuplicateOrder = 10;
            int expectedDupInterval = 1000;

            target.SetValueIntoDuplicatePropertiesFromText(keyValueCSV);

            int actaulDuplicateOrder = target.MaxDuplicateOrder;
            int actaulDupInterval = target.DuplicateOrderTimeInterval;

            Assert.AreEqual(expectedDuplicateOrder, actaulDuplicateOrder);
            Assert.AreEqual(expectedDupInterval, actaulDupInterval);

            keyValueCSV = "   ,1000   ";
            expectedDuplicateOrder = int.MaxValue;
            expectedDupInterval = 1000;

            target.SetValueIntoDuplicatePropertiesFromText(keyValueCSV);

            actaulDuplicateOrder = target.MaxDuplicateOrder;
            actaulDupInterval = target.DuplicateOrderTimeInterval;

            Assert.AreEqual(expectedDuplicateOrder, actaulDuplicateOrder);
            Assert.AreEqual(expectedDupInterval, actaulDupInterval);

        }


        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetValueIntoMocLocPropertiesFromTextTest()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string keyValueCSV = " true , 16:30 ";
        //    bool expectedMocLocAllowed = true;
        //    string expectedLatestTime = "16:30";

        //    target.SetValueIntoMocLocPropertiesFromText(keyValueCSV);

        //    bool? actaulMocLocAllowed = target.MocLocAllowed;
        //    string actaulLatestTime = target.LatestTime;
        //    Assert.AreEqual(expectedMocLocAllowed, actaulMocLocAllowed);
        //    Assert.AreEqual(expectedLatestTime, actaulLatestTime);
        //}

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetValueIntoMocLocPropertiesFrom1PropertyTextTest()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string keyValueCSV = " true , ";
        //    bool expectedMocLocAllowed = true;
        //    string expectedLatestTime = string.Empty;

        //    target.SetValueIntoMocLocPropertiesFromText(keyValueCSV);

        //    bool? actaulMocLocAllowed = target.MocLocAllowed;
        //    string actaulLatestTime = target.LatestTime;
        //    Assert.AreEqual(expectedMocLocAllowed, actaulMocLocAllowed);
        //    Assert.AreEqual(expectedLatestTime, actaulLatestTime);
        //}

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetValueIntoMocLocPropertiesFromFalseMocLocAllowedPropertyTextTest()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string keyValueCSV = " false , 16:45";
        //    bool expectedMocLocAllowed = false;
        //    string expectedLatestTime = string.Empty;

        //    target.SetValueIntoMocLocPropertiesFromText(keyValueCSV);

        //    bool? actaulMocLocAllowed = target.MocLocAllowed;
        //    string actaulLatestTime = target.LatestTime;
        //    Assert.AreEqual(expectedMocLocAllowed, actaulMocLocAllowed);
        //    Assert.AreEqual(expectedLatestTime, actaulLatestTime);
        //}

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetValueIntoMocLocPropertiesFromOnlyLatestTimePropertyTextTest()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string keyValueCSV = " , 16:45";
        //    bool? expectedMocLocAllowed = null;
        //    string expectedLatestTime = string.Empty;

        //    target.SetValueIntoMocLocPropertiesFromText(keyValueCSV);

        //    bool? actaulMocLocAllowed = target.MocLocAllowed;
        //    string actaulLatestTime = target.LatestTime;
        //    Assert.AreEqual(expectedMocLocAllowed, actaulMocLocAllowed);
        //    Assert.AreEqual(expectedLatestTime, actaulLatestTime);
        //}

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void SetClientCompanySettingTest_CREDIT_LIMIT_INT()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
            string key = CompanySetting.CLIENT_CREDIT_LIMIT_DBL;
            string keyValue = "$ 1,000,000.00  ";
            target.SetClientCompanySetting(key, keyValue);
            int expected = 1000000;
            double actual = target.CreditLimit;
            Assert.AreEqual(expected, actual);
        }

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetClientCompanySettingTest_ALLOW_SELL_SHORT_BOOL()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string key = CompanySetting.ALLOW_SELL_SHORT_BOOL;
        //    string keyValue = " true  ";
        //    target.SetClientCompanySetting(key, keyValue);
        //    bool? expected = true;
        //    bool? actual = target.EnableSellShort;
        //    Assert.AreEqual(expected, actual);

        //    keyValue = " Null  ";
        //    target.SetClientCompanySetting(key, keyValue);
        //    expected = null;
        //    actual = target.EnableSellShort;
        //    Assert.AreEqual(expected, actual);

        //}

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetClientCompanySettingTest_ALLOW_SELL_SHORT_EXEMP_BOOL()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string key = CompanySetting.ALLOW_SELL_SHORT_EXEMP_BOOL;
        //    string keyValue = " true  ";
        //    target.SetClientCompanySetting(key, keyValue);
        //    bool? expected = true;
        //    bool? actual = target.EnableSellShortExempt;
        //    Assert.AreEqual(expected, actual);

        //    keyValue = " Null  ";
        //    target.SetClientCompanySetting(key, keyValue);
        //    expected = null;
        //    actual = target.EnableSellShortExempt;
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetClientCompanySettingTest_MAX_AGGREGATED_EXECUTE_INT()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string key = CompanySetting.MAX_AGGREGATED_EXECUTE_INT;
        //    string keyValue = "$ 1,000,000.00  ";
        //    target.SetClientCompanySetting(key, keyValue);
        //    int expected = 1000000;
        //    int? actual = target.AggregateExecutedAmount;
        //    Assert.AreEqual(expected, actual);
        //}

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetClientCompanySettingTest_MAX_NET_TRADE_INT()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string key = CompanySetting.MAX_NET_TRADE_INT;
        //    string keyValue = "$ 1,000,000.00  ";
        //    target.SetClientCompanySetting(key, keyValue);
        //    int expected = 1000000;
        //    int? actual = target.MaxNetTraded;
        //    Assert.AreEqual(expected, actual);
        //}

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void SetClientCompanySettingTest_MAX_NOTIONAL_INT()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
            string key = CompanySetting.MAX_NOTIONAL_INT;
            string keyValue = "$ 1,000,000.00  ";
            target.SetClientCompanySetting(key, keyValue);
            int expected = 1000000;
            int actual = target.MaxNotionalPerOrder;
            Assert.AreEqual(expected, actual);
        }

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetClientCompanySettingTest_MAX_SHARES_LIMIT_INT()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string key = CompanySetting.MAX_SHARES_LIMIT_INT;
        //    string keyValue = "$ 1,000,000.00  ";
        //    target.SetClientCompanySetting(key, keyValue);
        //    int expected = 1000000;
        //    int actual = target.MaxSharesPerOrder;
        //    Assert.AreEqual(expected, actual);
        //}

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void SetClientCompanySettingTest_MAX_PRICE_DIFF_PCT()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
            string key = CompanySetting.MAX_PRICE_DIFF_PCT;
            string keyValue = "95";
            target.SetClientCompanySetting(key, keyValue);
            int expected = 95;
            int actual = target.MaxPriceDiff;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void SetClientCompanySettingTest_MAX_DUPES_CSV()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
            string key = CompanySetting.MAX_DUPES_CSV;
            string keyValue = " 25 , 25000 ";
            target.SetClientCompanySetting(key, keyValue);
            int expectedDupOrder = 25;
            int expectedDupOrderInterval = 25000;
            int actualDupOrder = target.MaxDuplicateOrder;
            int actualDupOrderInterval = target.DuplicateOrderTimeInterval;
            Assert.AreEqual(expectedDupOrder, actualDupOrder);
            Assert.AreEqual(expectedDupOrderInterval, actualDupOrderInterval);
        }


        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetClientCompanySettingTest_ON_CLOSE_CSV()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string key = CompanySetting.ON_CLOSE_CSV;
        //    string keyValue = " true , 15:25";
        //    target.SetClientCompanySetting(key, keyValue);
        //    bool expectedMocLocAllowed = true;
        //    string expectedlatestTime = "15:25";
        //    bool? actualMocLocAllowed = target.MocLocAllowed;
        //    string actuallatestTime = target.LatestTime;
        //    Assert.AreEqual(expectedMocLocAllowed, actualMocLocAllowed);
        //    Assert.AreEqual(expectedlatestTime, actuallatestTime);
        //}

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetClientCompanySettingTest_POSITION_CSV()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string key = CompanySetting.POSITION_CSV;
        //    string keyValue = " 1000 , 2000";
        //    target.SetClientCompanySetting(key, keyValue);

        //    int expectedInt = int.MaxValue;
        //    bool? expectedBool = null;

        //    Assert.AreEqual(expectedInt, target.CreditLimit);
        //    Assert.AreEqual(expectedInt, target.AggregateExecutedAmount);
        //    Assert.AreEqual(expectedInt, target.DuplicateOrderTimeInterval);
        //    Assert.AreEqual(expectedInt, target.MaxDuplicateOrder);
        //    Assert.AreEqual(expectedInt, target.MaxNetTraded);
        //    Assert.AreEqual(expectedInt, target.MaxNotionalPerOrder);
        //    Assert.AreEqual(expectedInt, target.MaxSharesPerOrder);
        //    Assert.AreEqual(expectedBool, target.MocLocAllowed);
        //    Assert.AreEqual(null, target.LatestTime);
        //    Assert.AreEqual(expectedBool, target.EnableSellShort);
        //    Assert.AreEqual(expectedBool, target.EnableSellShortExempt);
        //}

        //[TestMethod()]
        //[DeploymentItem("RiskSettingEngine.dll")]
        //public void SetClientCompanySettingTest_LOCATES_CSV()
        //{
        //    Hashtable clientCompanySetting = null;
        //    RiskSetting_Accessor target = new RiskSetting_Accessor(clientCompanySetting);
        //    string key = CompanySetting.LOCATES_CSV;
        //    string keyValue = " 1000 , 2000";
        //    target.SetClientCompanySetting(key, keyValue);

        //    int expectedInt = int.MaxValue;
        //    bool? expectedBool = null;

        //    Assert.AreEqual(expectedInt, target.CreditLimit);
        //    Assert.AreEqual(expectedInt, target.AggregateExecutedAmount);
        //    Assert.AreEqual(expectedInt, target.DuplicateOrderTimeInterval);
        //    Assert.AreEqual(expectedInt, target.MaxDuplicateOrder);
        //    Assert.AreEqual(expectedInt, target.MaxNetTraded);
        //    Assert.AreEqual(expectedInt, target.MaxNotionalPerOrder);
        //    Assert.AreEqual(expectedInt, target.MaxSharesPerOrder);
        //    Assert.AreEqual(expectedBool, target.MocLocAllowed);
        //    Assert.AreEqual(null, target.LatestTime);
        //    Assert.AreEqual(expectedBool, target.EnableSellShort);
        //    Assert.AreEqual(expectedBool, target.EnableSellShortExempt);
        //}

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void UpdateClientCompanySettingTest_LOCATES_CSV()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target1 = new RiskSetting_Accessor(clientCompanySetting)
            {
                //CreditLimit = 100000,
                ClientCreditLimit = 100000,
                EnableSellShort = true,
                EnableSellShortExempt = false,
                DuplicateOrderTimeInterval = 10000,
                MaxDuplicateOrder = 100,
                MocLocAllowed = false,
                LatestTime = "15:25"
            };
            Hashtable modifiedRiskSetting = target1.GetUpdatedRiskSetting();

            RiskSetting_Accessor target2 = new RiskSetting_Accessor(modifiedRiskSetting);

            //Assert.AreEqual(target1.AggregateExecutedAmount, target2.AggregateExecutedAmount);
            //Assert.AreEqual(target1.MaxNetTraded, target2.MaxNetTraded);
            Assert.AreEqual(target1.MaxNotionalPerOrder, target2.MaxNotionalPerOrder);
            //Assert.AreEqual(target1.MaxSharesPerOrder, target2.MaxSharesPerOrder);
            Assert.AreEqual(target1.CreditLimit, target2.CreditLimit);

            //Assert.AreEqual(target1.EnableSellShort, target2.EnableSellShort);
            //Assert.AreEqual(target1.EnableSellShortExempt, target2.EnableSellShortExempt);

            Assert.AreEqual(target1.DuplicateOrderTimeInterval, target2.DuplicateOrderTimeInterval);
            Assert.AreEqual(target1.MaxDuplicateOrder, target2.MaxDuplicateOrder);

            //Assert.AreEqual(target1.MocLocAllowed, target2.MocLocAllowed);
            //Assert.AreNotEqual(target1.LatestTime, target2.LatestTime);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void IsDirtyIntegerTest()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target1 = new RiskSetting_Accessor(clientCompanySetting);
            int val = int.MaxValue;
            bool expected = true;
            bool actual = target1.IsDirtyValue(val);
            Assert.AreEqual(expected, actual);

            val = 500;
            expected = false;
            actual = target1.IsDirtyValue(val);
            Assert.AreEqual(expected, actual);

            val = 0;
            expected = false;
            actual = target1.IsDirtyValue(val);
            Assert.AreEqual(expected, actual);

            int? val2 = int.MaxValue;
            bool expected2 = true;
            bool actual2 = target1.IsDirtyValue(val2);
            Assert.AreEqual(expected2, actual2);

            val2 = null;
            expected2 = false;
            actual2 = target1.IsDirtyValue(val2);
            Assert.AreEqual(expected2, actual2);

            val2 = 5000;
            expected2 = false;
            actual2 = target1.IsDirtyValue(val2);
            Assert.AreEqual(expected2, actual2);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void IsDirtyBooleanTest()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target1 = new RiskSetting_Accessor(clientCompanySetting);
            bool? val = null;
            bool expected = true;
            bool actual = target1.IsDirtyValue(val);
            Assert.AreEqual(expected, actual);

            val = false;
            expected = false;
            actual = target1.IsDirtyValue(val);
            Assert.AreEqual(expected, actual);

            val = false;
            expected = false;
            actual = target1.IsDirtyValue(val);
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void IsDirtyStringTest()
        {
            Hashtable clientCompanySetting = null;
            RiskSetting_Accessor target1 = new RiskSetting_Accessor(clientCompanySetting);
            string val = null;
            bool expected = true;
            bool actual = target1.IsDirtyValue(val);
            Assert.AreEqual(expected, actual);

            val = "15:25:00";
            expected = false;
            actual = target1.IsDirtyValue(val);
            Assert.AreEqual(expected, actual);

            val = "";
            expected = false;
            actual = target1.IsDirtyValue(val);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetUpdatedRiskSettingTest()
        {
            RiskSetting setting = new RiskSetting(null);
            Hashtable clientCompanySetting = new Hashtable();
            clientCompanySetting = setting.GetUpdatedRiskSetting();
            Assert.AreEqual(9, clientCompanySetting.Count);
        }


        #region Tests for WASH Check Fields

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void SetDefaultTest_WASHCheck()
        {
            RiskSetting setting = new RiskSetting(null);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.EquitiesWashCheck);
        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void CheckCompanySettingTest_WASHCheck()
        {
            Hashtable clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, "CS=NPC,OPT=NPC");
            RiskSetting setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.NO_PRICE_CHECK, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.NO_PRICE_CHECK, setting.EquitiesWashCheck);

            clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, "OPT=POC,CS=POC");
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.EquitiesWashCheck);

            clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, "CS=PDC,OPT=PDC");
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_PLUS_DESTINATION, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_PLUS_DESTINATION, setting.EquitiesWashCheck);

            clientCompanySetting = new Hashtable();
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.EquitiesWashCheck);


            clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, "");
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.EquitiesWashCheck);

            clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, "CS=ABC,OPT=XYZ");
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.EquitiesWashCheck);

            clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, "CS=ABC,OPT=PDC");
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_PLUS_DESTINATION, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.EquitiesWashCheck);

            clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, "CS=PDC,,,");
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_PLUS_DESTINATION, setting.EquitiesWashCheck);

            clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, "OPT=PDC,,,");
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_PLUS_DESTINATION, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.EquitiesWashCheck);

            clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, " , OPT=PDC,,,");
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_PLUS_DESTINATION, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.EquitiesWashCheck);

            clientCompanySetting = new Hashtable();
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, " , CS=PDC,,,");
            setting = new RiskSetting(clientCompanySetting);
            Assert.AreEqual(WashTradeCheck.PRICE_ONLY_CHECK, setting.OptionsWashCheck);
            Assert.AreEqual(WashTradeCheck.PRICE_PLUS_DESTINATION, setting.EquitiesWashCheck);

        }

        [TestMethod()]
        [DeploymentItem("RiskSettingEngine.dll")]
        public void GetUpdatedRiskSettingTest_WASHCheck()
        {
            RiskSetting setting = new RiskSetting(null);

            setting.OptionsWashCheck = WashTradeCheck.NO_PRICE_CHECK;
            setting.EquitiesWashCheck = WashTradeCheck.NO_PRICE_CHECK;
            Hashtable prop = setting.GetUpdatedRiskSetting();
            Assert.IsTrue(prop.ContainsKey(CompanySetting.WASH_TRADE_CHECK_CSV));
            string actual = "CS="+setting.EquitiesWashCheck +",OPT=" + setting.OptionsWashCheck;
            Assert.AreEqual(prop[CompanySetting.WASH_TRADE_CHECK_CSV], actual);

            setting.OptionsWashCheck = WashTradeCheck.PRICE_ONLY_CHECK;
            setting.EquitiesWashCheck = WashTradeCheck.PRICE_ONLY_CHECK;
            prop = setting.GetUpdatedRiskSetting();
            actual = actual = "CS=" + setting.EquitiesWashCheck + ",OPT=" + setting.OptionsWashCheck;
            Assert.AreEqual(prop[CompanySetting.WASH_TRADE_CHECK_CSV], actual);

            setting.OptionsWashCheck = WashTradeCheck.PRICE_PLUS_DESTINATION;
            setting.EquitiesWashCheck = WashTradeCheck.PRICE_PLUS_DESTINATION;
            prop = setting.GetUpdatedRiskSetting();
            actual = actual = "CS=" + setting.EquitiesWashCheck + ",OPT=" + setting.OptionsWashCheck;
            Assert.AreEqual(prop[CompanySetting.WASH_TRADE_CHECK_CSV], actual);

        }
        
        #endregion

    
    }
}
