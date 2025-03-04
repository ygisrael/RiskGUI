using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXLib;
using EZX.LightspeedEngine.Entity;

namespace LightSpeedEngineTest.Utils
{
    public static class GroupAccountUtil
    {
        public static Account CreateAccountWithoutParentGroup(string id, string name, string ownerId)
        {
            Account account = new Account();
            account.Id = id;
            account.Name = name;
            account.OwnerId = ownerId;
            account.ParentGroup = new Group();
            return account;
        }
        public static GroupAccount CreateNewGroupAccount(string id, string name, int isDefault, Properties settings, string ownerId)
        {
            GroupAccount groupAccount = new GroupAccount()
            {
                Id = id,
                DisplayName = name,
                IsDefault = isDefault,
                Settings = settings,
                OwnerID = ownerId
            };
            return groupAccount;
        }
    }
}
