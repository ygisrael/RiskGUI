using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EZX.LightspeedEngine.Entity;

namespace LightSpeedEngineTest.Entity
{
    [TestClass()]
    public class GroupTest
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
        public void SetGroupNameForStandaloneGroupTest()
        {
            Group group = new Group()
                {
                    Id = "GROUP1",
                    Name = "GROUP1",
                    AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(),
                    RiskSetting = new RiskSetting()
                };
            Account account = new Account()
                {
                    Id = "ACCOUNT1",
                    Name = "ACCOUNT1",
                    OwnerId = "GROUP1",
                    ParentGroup = group,
                    IsOwnGroup = true
                };
            group.AccountList.Add(account);
            group.IsAccountGroup = true;

            Assert.AreEqual(account.Name, group.Name);
            Assert.AreEqual(account.IsWaitingForServerResponse, group.IsWaitingForServerResponse);
            Assert.AreEqual(account.Id, group.Id);
            Assert.AreEqual(account.IsInEditMode, group.IsInEditMode);
            Assert.AreEqual(account, group.GroupAccount);

            account.Name = "ACCOUNT1Ren";
            Assert.AreEqual(account.Name, group.Name);

            group.Name = "TEST";
            Assert.AreEqual(account.Name, group.Name);

            group.IsAccountGroup = false;
            group.AccountList.Clear();
            Assert.IsNull(group.GroupAccount);
        }
    }
}
