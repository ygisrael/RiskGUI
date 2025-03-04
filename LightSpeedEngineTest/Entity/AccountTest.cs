using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EZX.LightspeedEngine.Entity;
using EZXLib;

namespace LightSpeedEngineTest.Entity
{
    [TestClass()]
    public class AccountTest
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

        [TestMethod()]
        public void AddRiskSettingTest()
        {
            Account account = new Account();
            EZXLib.Properties properties = new EZXLib.Properties();
            properties.PropertyMap = new EZXLib.TagValueMsg();
            //properties.PropertyMap.tagValues.Add(CompanySetting.CREDIT_LIMIT_DBL, "100000");
            properties.PropertyMap.tagValues.Add(CompanySetting.CLIENT_CREDIT_LIMIT_DBL, "10000");
            properties.PropertyMap.tagValues.Add(CompanySetting.MAX_DUPES_CSV,"10,10000");
            properties.PropertyMap.tagValues.Add(CompanySetting.MAX_PRICE_DIFF_PCT,"10");
            properties.PropertyMap.tagValues.Add(CompanySetting.MAX_NOTIONAL_INT,"10000");
            account.AddRiskSetting(properties);

            Assert.AreEqual(account.RiskSetting.CreditLimit, 10000);
            Assert.AreEqual(account.RiskSetting.MaxDuplicateOrder, 10);
            Assert.AreEqual(account.RiskSetting.DuplicateOrderTimeInterval, 10000);
            Assert.AreEqual(account.RiskSetting.MaxNotionalPerOrder, 10000);
            Assert.AreEqual(account.RiskSetting.MaxPriceDiff, 10);

        }


        [TestMethod()]
        public void AddRiskSettingWhenProperrtiesIsNullTest()
        {
            Account account = new Account();
            account.AddRiskSetting(null);

            Assert.AreEqual(account.RiskSetting.CreditLimit, 0);
            Assert.AreEqual(account.RiskSetting.MaxDuplicateOrder, 0);
            Assert.AreEqual(account.RiskSetting.DuplicateOrderTimeInterval, 0);
            Assert.AreEqual(account.RiskSetting.MaxNotionalPerOrder, 0);
            Assert.AreEqual(account.RiskSetting.MaxPriceDiff, 1);

        }

    }
}
