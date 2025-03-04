using EZX.LightspeedEngine.Entity;
using EZX.LightSpeedEngine.Config;
using EZXLib;
using EZXWPFLibrary.Helpers;
using EZXWPFLibrary.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EZX.LightSpeedEngine
{
    public class DataManager : ObservableBase
    {
        //public delegate void ClientOrderProcessedHandler(object sender, EventArgs e);
        //public event ClientOrderProcessedHandler ClientOrderProcessed;
        //private MTObservableCollection<ClientOrderStatusData> clientOrderList;
        //public MTObservableCollection<ClientOrderStatusData> ClientOrderList
        //{
        //    get { return clientOrderList; }
        //    set
        //    {
        //        clientOrderList = value;
        //        this.RaisePropertyChanged(p => p.ClientOrderList);
        //    }
        //}

        public string PRICE_CHECK_KEY = CompanySetting.MARKET_DATA_SOURCE_ENABLED_BOOL;

        private string groupConfigFile = "GroupConfig.xml";

        private LSEngine lsEngine;
        private MTObservableCollection<INodeEntity> accountGroupList;
        private ReadGroupAccounts readGroupAccountsInstance;

        private bool? isPriceCheckON;
        private bool isPriceCheckSpecified;

        private bool isAlphabetize;

        public LSEngine LSEngine
        {
            get { return lsEngine; }
            set
            {
                lsEngine = value;
                this.RaisePropertyChanged(p => p.LSEngine);
            }
        }
        public MTObservableCollection<INodeEntity> AccountGroupList
        {
            get { return accountGroupList; }
            set
            {
                accountGroupList = value;
                this.RaisePropertyChanged(p => p.AccountGroupList);
            }
        }
        public ReadGroupAccounts ReadGroupAccountsInstance
        {
            get { return readGroupAccountsInstance; }
            set
            {
                readGroupAccountsInstance = value;
                this.RaisePropertyChanged(p => p.ReadGroupAccountsInstance);
            }
        }
        public bool? IsPriceCheckON
        {
            get { return isPriceCheckON; }
            set
            {
                isPriceCheckON = value;
                this.RaisePropertyChanged(p => p.IsPriceCheckON);
            }
        }
        public bool IsPriceCheckSpecified
        {
            get { return isPriceCheckSpecified; }
            set
            {
                isPriceCheckSpecified = value;
                this.RaisePropertyChanged(p => p.IsPriceCheckSpecified);
            }
        }

        public bool IsAlphabetize
        {
            get { return isAlphabetize; }
            set
            {
                isAlphabetize = value;
                this.RaisePropertyChanged(p => p.IsAlphabetize);
            }
        }

        public DataManager(LSEngine lsEngine)
        {
            LSEngine = lsEngine;
            AccountGroupList = new MTObservableCollection<INodeEntity>();
            ReadGroupAccountsInstance = new ReadGroupAccounts();
            //this.ClientOrderList = new MTObservableCollection<ClientOrderStatusData>();
        }

        private List<Account> tempAccountList = new List<Account>();

        public List<Account> TempAccountList
        {
            get { return tempAccountList; }
            set { tempAccountList = value; }
        }

        public void LoadAllGroupAndAccount(ReadGroupAccounts readGroupAccountsObj)
        {
            string defaultGroupId = string.Empty;
            ReadGroupAccountsInstance = readGroupAccountsObj;
            List<Group> saveGroupOrder = GetGroupFromSavedConfig();
            if (saveGroupOrder == null)
            {
                saveGroupOrder = new List<Group>();
            }

            List<GroupAccount> orderedGroupAccountOrderList = ReOrderReadGroupAccount(readGroupAccountsObj.GroupAccounts, saveGroupOrder);
            AccountGroupList.Clear();
            foreach (GroupAccount groupAccountObj in orderedGroupAccountOrderList)
            {
                if (string.IsNullOrEmpty(groupAccountObj.OwnerID))
                {
                    //Group
                    bool isGroupNodeExpended = EvaluateIsGroupExpended(saveGroupOrder, groupAccountObj);

                    defaultGroupId = CreateNewGroupNode(defaultGroupId, groupAccountObj, isGroupNodeExpended);
                }
                else if (groupAccountObj.OwnerID.Equals(groupAccountObj.Id))
                {
                    //Stand-alone Accounts
                    CreateStandAloneAccountNode(groupAccountObj);
                }
                else
                {
                    //Account within Group
                    CreateAccountNode(tempAccountList, groupAccountObj);
                }
            }
        }

        public bool EvaluateIsGroupExpended(List<Group> saveGroupOrder, GroupAccount groupAccountObj)
        {
            Group grp = saveGroupOrder.FirstOrDefault(g => g.Id.Equals(groupAccountObj.Id));
            if (grp != null)
            {
                return grp.IsExpanded;
            }
            return false;
        }

        private List<GroupAccount> ReOrderReadGroupAccount(List<GroupAccount> sourceGroupAccountOrderList, List<Group> saveGroupOrder)
        {
            List<GroupAccount> groupAccountOrderList = new List<GroupAccount>();
            foreach (Group group in saveGroupOrder)
            {
                GroupAccount groupAccount = sourceGroupAccountOrderList.FirstOrDefault(g => g.Id.Equals(@group.Id));
                if (groupAccount != null)
                {
                    groupAccountOrderList.Add(groupAccount);
                    if (group.AccountList != null)
                    {
                        foreach (Account orderedAccount in group.AccountList)
                        {
                            GroupAccount account = sourceGroupAccountOrderList.FirstOrDefault(g => g.Id.Equals(orderedAccount.Id));
                            if (account != null)
                            {
                                groupAccountOrderList.Add(account);
                            }
                        }
                    }
                }
            }

            foreach (GroupAccount groupAccount in sourceGroupAccountOrderList)
            {
                GroupAccount addedOrderGroupAccount = groupAccountOrderList.FirstOrDefault(g => g.Id.Equals(groupAccount.Id));
                if (addedOrderGroupAccount == null)
                {
                    groupAccountOrderList.Insert(0, groupAccount);
                }
            }

            return groupAccountOrderList;
        }

        private List<Group> GetGroupFromSavedConfig()
        {
            string fileFullPath = ConfigInfo.BaseDirectory + "/" + groupConfigFile;
            try
            {
                List<Group> saveGroupOrder = SerializerUtil.LoadFromFile<List<Group>>(fileFullPath);
                return saveGroupOrder;
            }
            catch (Exception ex)
            {
                Logger.WARN("Unable to load from config-file: " + fileFullPath + ",as exception occurred: " + ex.StackTrace);
                return null;
            }
        }

        private void CreateAccountNode(List<Account> tmpAccountList, GroupAccount groupAccountObj)
        {
            Account accountNode = new Account { ParentGroup = FindGroupNode(groupAccountObj.OwnerID) };
            // If account is loading before loading group, then parent group would not find through FindGroup(groupAccountObj.OwnerID),
            // So here the accounts will load through tmpAccountList, when group is loading.
            if (accountNode.ParentGroup.Id == null)
            {
                accountNode.OwnerId = groupAccountObj.OwnerID;
                tmpAccountList.Add(accountNode);
            }

            accountNode.Name = groupAccountObj.DisplayName;
            accountNode.Id = groupAccountObj.Id;
            SetAccountRiskSetting(groupAccountObj.Settings, accountNode);

            Account existingAccountInSameGroup = accountNode.ParentGroup.AccountList.FirstOrDefault(a => a.Id != null && a.Id.Equals(accountNode.Id));
            if (existingAccountInSameGroup != null)
            {
                existingAccountInSameGroup = accountNode;
            }
            else
            {
                accountNode.ParentGroup.AccountList.Add(accountNode);
            }
        }

        private void CreateStandAloneAccountNode(GroupAccount groupAccountObj)
        {
            INodeEntity existingGroup = AccountGroupList.FirstOrDefault(a => a.Id.Equals(groupAccountObj.Id));

            Account accountNode = new Account
            {
                IsOwnGroup = true,
                Name = groupAccountObj.DisplayName,
                Id = groupAccountObj.Id
            };

            SetAccountRiskSetting(groupAccountObj.Settings, accountNode);

            Group standAloneAccountGroup = new Group { Name = groupAccountObj.DisplayName };
            if (existingGroup != null)
            {
                standAloneAccountGroup = existingGroup as Group;
                if (standAloneAccountGroup != null) standAloneAccountGroup.GroupAccount.Name = accountNode.Name;
            }
            else
            {
                AccountGroupList.Add(standAloneAccountGroup);
                standAloneAccountGroup.Id = groupAccountObj.Id;
                standAloneAccountGroup.IsAccountGroup = false;
                standAloneAccountGroup.AccountList = new MTObservableCollection<Account> { accountNode };
                standAloneAccountGroup.IsAccountGroup = true;
            }
            accountNode.ParentGroup = standAloneAccountGroup;

            standAloneAccountGroup?.AddRiskSetting(groupAccountObj.Settings);
        }

        public string CreateNewGroupNode(string defaultGroupId, GroupAccount groupAccountObj, bool isGroupNodeExpended)
        {
            INodeEntity existingGroup = AccountGroupList.FirstOrDefault(a => a.Id.ToLower().Equals(groupAccountObj.Id.ToLower()));
            Group group = new Group();
            if (existingGroup != null)
            {
                group = existingGroup as Group;
            }
            else
            {
                group.Id = groupAccountObj.Id;
                group.AccountList = new MTObservableCollection<Account>();
                AccountGroupList.Add(group);
            }

            if (group != null)
            {
                group.IsExpanded = isGroupNodeExpended;
                group.Name = groupAccountObj.DisplayName;
                group.AddRiskSetting(groupAccountObj.Settings);
                ObservableCollection<Account> accountList = FindAllAccountList(group.Id, tempAccountList);

                foreach (Account account in accountList)
                {
                    //Setting Parent Group of all AccountList
                    account.ParentGroup = group;

                    //Adding Account, which are added before group, and removing from tempAccount
                    group.AccountList.Add(account);
                    tempAccountList.Remove(account);
                }

                if (groupAccountObj.IsDefault == 1)
                {
                    group.IsDefaultGroup = true;
                    defaultGroupId = groupAccountObj.Id;
                    group.IsSelected = true;
                }
            }

            return defaultGroupId;
        }

        private ObservableCollection<Account> FindAllAccountList(string groupId, List<Account> tmpAccountList)
        {
            ObservableCollection<Account> accountList = new ObservableCollection<Account>();

            foreach (Account account in tmpAccountList)
            {
                if (account.OwnerId.Equals(groupId))
                {
                    accountList.Add(account);
                }
            }

            return accountList;
        }

        public Group FindGroupNode(string groupId)
        {
            Group existingGroup = new Group();
            foreach (var accGroup in AccountGroupList)
            {
                if (accGroup.Id.Equals(groupId))
                {
                    if (accGroup is Group)
                    {
                        existingGroup = accGroup as Group;
                        break;
                    }
                }
            }


            if (existingGroup.AccountList == null)
            {
                existingGroup.AccountList = new MTObservableCollection<Account>();
            }
            return existingGroup;

        }

        //ClientOrderResponse received in following cases:
        //1 - When Logon, bulk of responses receives.
        //2 - When new order added, then server send response.
        //3 - When logout and login back, then only the orders added meanwhile the user was disconnected.
        //public void ProcessClientOrderResponse(ClientOrderResponse clientOrderResponse)
        //{
        //    ClientOrderStatusData currentCosData = new ClientOrderStatusData();
        //    currentCosData.Import(this.LSEngine.ChoiceManager, clientOrderResponse);

        //    ClientOrderStatusData prevCosData = this.ClientOrderList.Where(c => c.OrderID == clientOrderResponse.OrderID).FirstOrDefault();
        //    if (prevCosData != null)
        //    {
        //        prevCosData = currentCosData;
        //    }
        //    else
        //    {
        //        this.ClientOrderList.Add(currentCosData);
        //    }

        //    CheckAndAddAccountIntoDefaultGroup(currentCosData.Account);

        //}

        public Group GetDefaultGroupNode()
        {
            foreach (Group group in AccountGroupList)
            {
                if (group.IsDefaultGroup)
                {
                    return group;
                }
            }
            return null;
        }

        public Group CreateNewStandAloneAccountNode(string accountName)
        {
            if (string.IsNullOrEmpty(accountName))
            {
                accountName = "New Account";
            }

            Group newGroup = new Group
            {
                Name = accountName,
                Id = accountName,
                IsDefaultGroup = false,
                IsAccountGroup = false,
                RiskSetting = new RiskSetting(),
                AccountList = new MTObservableCollection<Account>()
            };

            Account newAccount = new Account
            {
                Name = accountName,
                Id = accountName,
                IsInEditMode = true,
                IsOwnGroup = true,
                ParentGroup = newGroup,
            };

            newGroup.Id = newAccount.Id;
            newGroup.AccountList.Add(newAccount);
            newGroup.IsAccountGroup = true;

            return newGroup;
        }

        public Group CreateNewGroupNode()
        {
            Group newGroup = new Group
            {
                Name = "New Group",
                IsDefaultGroup = false,
                IsAccountGroup = false,
                RiskSetting = new RiskSetting(),
                Id = Group.newId(),
                IsInEditMode = true,
                AccountList = new MTObservableCollection<Account>()
            };
            return newGroup;
        }

        public void AddNewGroupAccount(CreateGroupAccount createGroupAccount)
        {
            //bool isErrorInResponse = false;
            INodeEntity existingNode = FindNodeEntity(createGroupAccount.GroupAccount.Id);
            if (createGroupAccount.ReturnCode != 0)
            {
                //Error in ReturnCode
                //isErrorInResponse = true;
                if (!LSEngine.SessionId.Equals(createGroupAccount.MyID))
                {
                    //No need to process respose the the error in response message 
                    //and process not run for different GUI Session (this.LSEngine.SessionId != createGroupAccount.MyID)
                    return;
                }

                if (existingNode == null)
                {
                    return;
                    //throw new Exception("ExistingNode is failed to find in AccountGroupList,  for Id" + createGroupAccount.GroupAccount.Id);
                }

                existingNode.IsWaitingForServerResponse = false;

                GroupAccount existingObjectInReadGroupAccountInstance = FindGroupAccount(createGroupAccount.GroupAccount.Id);
                if (existingObjectInReadGroupAccountInstance == null)
                {
                    // Means the ReadGroupAccountInstance does not had this instance when sending request
                    // Therefore the existingNode was send for Create New operation, and here it requires to remove as ERROR occurred.
                    existingNode.IsInEditMode = true;
                    if (existingNode is Group && (existingNode as Group).IsAccountGroup)
                    {
                        (existingNode as Group).GroupAccount.IsInEditMode = true;
                    }
                }

            }
            else
            {
                GroupAccount groupAccount = GetGroupAccount(createGroupAccount.GroupAccount.Id);
                if (groupAccount != null)
                {
                    groupAccount = createGroupAccount.GroupAccount;
                }
                else
                {
                    ReadGroupAccountsInstance.GroupAccounts.Add(createGroupAccount.GroupAccount);
                }

                if (existingNode != null)
                {
                    if (existingNode is Group && (existingNode as Group).IsAccountGroup)
                    {
                        (existingNode as Group).GroupAccount.IsWaitingForServerResponse = false;
                        (existingNode as Group).GroupAccount.IsInEditMode = false;
                    }
                    else
                    {
                        existingNode.IsWaitingForServerResponse = false;
                        existingNode.IsInEditMode = false;
                    }
                }

                if (string.IsNullOrEmpty(createGroupAccount.GroupAccount.OwnerID))
                {
                    //Group Object
                    if (createGroupAccount.GroupAccount.IsDefault == 1)
                    {
                        //Default Group
                        CreateNewGroupNode(createGroupAccount.GroupAccount.Id, createGroupAccount.GroupAccount, false);
                    }
                    else
                    {
                        //Non Default Group
                        CreateNewGroupNode("", createGroupAccount.GroupAccount, false);
                    }
                }
                else
                {
                    //Account Object
                    if (createGroupAccount.GroupAccount.Id.Equals(createGroupAccount.GroupAccount.OwnerID))
                    {
                        CreateStandAloneAccountNode(createGroupAccount.GroupAccount);
                    }
                    else
                    {
                        //Account
                        CreateAccountNode(createGroupAccount.GroupAccount);
                    }
                }
            }
        }

        private GroupAccount FindGroupAccount(string groupAccountId)
        {
            foreach (GroupAccount groupAccount in ReadGroupAccountsInstance.GroupAccounts)
            {
                if (groupAccount.Id.Equals(groupAccountId))
                {
                    return groupAccount;
                }
            }
            return null;
        }

        public GroupAccount GetGroupAccount(string groupAccountId)
        {
            GroupAccount groupAccountObj = ReadGroupAccountsInstance.GroupAccounts.FirstOrDefault(g => g.Id.Trim().Equals(groupAccountId.Trim()));
            return groupAccountObj;
        }

        public GroupAccount GetGroupAccountByName(string groupAccountName)
        {
            GroupAccount groupAccountObj = ReadGroupAccountsInstance.GroupAccounts.FirstOrDefault(g => g.DisplayName.ToLower().Trim().Equals(groupAccountName.ToLower().Trim()));
            return groupAccountObj;
        }


        public List<GroupAccount> GetFilteredAccountList()
        {
            ReadGroupAccounts filteredGroupAccounts = new ReadGroupAccounts();
            foreach (GroupAccount groupAccount in ReadGroupAccountsInstance.GroupAccounts)
            {
                foreach (string s in LSEngine.ApexFilterList)
                {
                    string compare = s.Split('%')[0];
                    if (groupAccount.Id.Contains(compare))
                    {
                        filteredGroupAccounts.GroupAccounts.Add(groupAccount);
                    }
                }
            }

            return SortAccountsAlphabetically(filteredGroupAccounts.GroupAccounts);
        }

        public List<GroupAccount> GetAccountList(string groupId)
        {
            return ReadGroupAccountsInstance.GroupAccounts.Where(g => g.OwnerID != null && g.OwnerID.Equals(groupId)).ToList();
        }


        public INodeEntity FindNodeEntity(string nodeId)
        {
            foreach (INodeEntity group in AccountGroupList)
            {
                if (group.Id.Equals(nodeId))
                {
                    return group;
                }

                if ((@group as Group).IsAccountGroup)
                {
                    if ((@group as Group).GroupAccount.Id.Equals(nodeId))
                    {
                        return (@group as Group).GroupAccount;
                    }
                }
                else
                {
                    foreach (INodeEntity account in (@group as Group).AccountList)
                    {
                        if (!account.IsInEditMode && account.Id.Equals(nodeId))
                        {
                            return account;
                        }
                    }
                }
            }
            return null;
        }

        private void CreateAccountNode(GroupAccount groupAccount)
        {
            CreateAccountNode(tempAccountList, groupAccount);
        }

        public void MoveAccountNode(Account accountNode, string newGroupId)
        {
            MoveAccount(accountNode.Id, newGroupId);
        }

        public void RemoveGroupAccountNode(GroupAccount groupAccount)
        {
            for (int i = 0; i < AccountGroupList.Count; i++)
            {
                INodeEntity group = AccountGroupList[i];
                if (group.Id.Equals(groupAccount.Id))
                {
                    AccountGroupList.Remove(group);
                    return;
                }

                if (@group is Group && !(@group as Group).IsAccountGroup)
                {
                    for (int j = 0; j < (@group as Group).AccountList.Count; j++)
                    {
                        Account account = (@group as Group).AccountList[j];
                        if (account.Id != null && account.Id.Equals(groupAccount.Id))
                        {
                            (@group as Group).AccountList.Remove(account);
                            return;
                        }
                    }
                }
            }
        }


        //public void MoveDeletingGroupAccountAsStandAlone(string removingGroupId)
        //{
        //    List<GroupAccount> accountList = this.GetAccountList(removingGroupId);
        //    if (accountList != null)
        //    {
        //        foreach (GroupAccount account in accountList)
        //        {
        //            this.MoveAccount(account.Id, account.Id);
        //        }
        //    }
        //}

        //public void MoveAccountAsStandAloneAccount(DeleteGroup deleteGroupResposne)
        //{
        //    string groupId = deleteGroupResposne.GroupAccount.Id;
        //    List<GroupAccount> accountList = this.GetAccountList(groupId);
        //    if (accountList != null)
        //    {
        //        foreach (GroupAccount account in accountList)
        //        {
        //            this.MoveAccount(account.Id, account.Id); 
        //        }
        //    }
        //}

        public void MoveAccountNode(Properties riskSetting, string accountId, string parentId)
        {
            if (riskSetting == null)
            {
                riskSetting = new Properties();
            }
            if (riskSetting.PropertyMap == null)
            {
                riskSetting.PropertyMap = new TagValueMsg();
            }

            INodeEntity nodeAccount = FindNodeEntity(accountId);
            if (nodeAccount != null)
            {
                //Check if not stand-alone account
                if (nodeAccount is Group && (nodeAccount as Group).IsAccountGroup)
                {
                    nodeAccount = (nodeAccount as Group).GroupAccount;
                }

                Account accountToMove = nodeAccount as Account;
                Group previousParentGroup = (nodeAccount as Account).ParentGroup;
                INodeEntity newParentGroup = FindNodeEntity(parentId);

                SetAccountRiskSetting(riskSetting, accountToMove);

                if (previousParentGroup.IsAccountGroup)
                {
                    //Remove stand-alone group, where the account was exists before moving
                    AccountGroupList.Remove(previousParentGroup);
                }
                else
                {
                    //Remove account from the old-parent-group, where the account was exists before moving
                    previousParentGroup.AccountList.Remove(accountToMove);
                }

                if (newParentGroup is Group)
                {
                    accountToMove.IsOwnGroup = false;
                    accountToMove.ParentGroup = newParentGroup as Group;
                    (newParentGroup as Group).AccountList.Add(accountToMove);
                }
                else
                {
                    if (newParentGroup is Account && newParentGroup == accountToMove)
                    {
                        //Move account as stand-alone account
                        Group standAloneAccountGroup = new Group();
                        standAloneAccountGroup.Id = accountToMove.Id;
                        standAloneAccountGroup.IsAccountGroup = false;
                        standAloneAccountGroup.AccountList = new MTObservableCollection<Account>();
                        standAloneAccountGroup.AccountList.Add(accountToMove);
                        standAloneAccountGroup.IsAccountGroup = true;
                        accountToMove.IsOwnGroup = true;
                        accountToMove.ParentGroup = standAloneAccountGroup;
                        standAloneAccountGroup.RiskSetting = new RiskSetting(riskSetting.PropertyMap.tagValues);
                        AccountGroupList.Add(standAloneAccountGroup);
                    }
                }
            }
            else
            {
                //Either new account or new Standalone Account
                if (accountId == parentId)
                {
                    //Stand alone Account
                    Group grp = CreateNewStandAloneAccountNode(accountId);
                    grp.GroupAccount.IsInEditMode = false;

                    grp.RiskSetting = new RiskSetting(riskSetting.PropertyMap.tagValues);
                    SetAccountRiskSetting(riskSetting, grp.GroupAccount);

                    AccountGroupList.Insert(0, grp);
                }
                else
                {
                    INodeEntity newParentGroup = FindNodeEntity(parentId);
                }
            }
        }

        private void SetAccountRiskSetting(Properties riskSetting, Account accountToMove)
        {
            if (LSEngine.IsAccountSettingON)
            {
                accountToMove.AddRiskSetting(riskSetting);
            }
            else
            {
                accountToMove.RiskSetting = new RiskSetting();
            }
        }


        public void SaveGroupAccountConfig(string _fullPath)
        {
            List<Group> groupList = new List<Group>();
            int groupIndex = 0;
            foreach (INodeEntity node in AccountGroupList.ToList())
            {
                Group group = new Group();
                group.Id = (node as Group).Id;
                group.IsDefaultGroup = (node as Group).IsDefaultGroup;
                group.Name = (node as Group).Name;
                group.IsExpanded = (node as Group).IsExpanded;
                group.IsAccountGroup = (node as Group).IsAccountGroup;
                group.RiskSetting = null;
                group.DisplayIndex = groupIndex;
                if (!group.IsAccountGroup)
                {
                    group.AccountList = new MTObservableCollection<Account>();
                    int accountIndex = 0;
                    foreach (Account account in (node as Group).AccountList.ToList())
                    {
                        Account newAccount = new Account();
                        newAccount.Id = account.Id;
                        newAccount.Name = account.Name;
                        newAccount.OwnerId = account.OwnerId;
                        newAccount.ParentGroup = null;
                        newAccount.DisplayIndex = accountIndex;
                        accountIndex++;
                        group.AccountList.Add(newAccount);
                    }
                }
                else
                {
                    group.IsAccountGroup = false;
                    group.AccountList = new MTObservableCollection<Account>();
                    group.IsAccountGroup = true;
                }
                groupList.Add(group);
                groupIndex++;
            }

            string fileFullPath = ConfigInfo.BaseDirectory + "/" + groupConfigFile;
            if (!string.IsNullOrEmpty(_fullPath))
            {
                fileFullPath = _fullPath;
            }

            SerializerUtil.SerializeToXML(groupList, fileFullPath);
        }

        public bool ValidateExistingGroupAccount(Group groupNode)
        {
            GroupAccount existingGroupByName = GetGroupAccountByName(groupNode.Name);
            try
            {
                if (existingGroupByName != null)
                {
                    throw new LightspeedException("Could not save Group/Account, as already exists with name: " + groupNode.Name, LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION);
                }

                if (groupNode.Name.Length > 155)
                {
                    throw new LightspeedException("Could not save, as Group/Account length is more than 155 characters!", LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION);
                }

                if (groupNode.Name.Trim().Length < 1)
                {
                    throw new LightspeedException("Could not save, as Group/Account length is too short!", LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION);
                }
            }
            catch (Exception ex)
            {
                if (!LSEngine.TEST_MODE)
                {
                    throw ex;
                }

                return false;
            }
            return true;
        }

        public bool ValidateNewGroup(Group groupNode)
        {
            try
            {
                GroupAccount existingGroupByName = GetGroupAccountByName(groupNode.Name);
                if (existingGroupByName != null)
                {
                    throw new LightspeedException("Could not save Group/Account, as already exists with name: " + groupNode.Name, LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION);
                }

                if (groupNode.Name.Length > 155)
                {
                    throw new LightspeedException("Could not save, as Group/Account length is more than 155 characters!", LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION);
                }

                if (groupNode.Name.Trim().Length < 1)
                {
                    throw new LightspeedException("Could not save, as Group/Account length is too short!", LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION);
                }
            }
            catch (Exception ex)
            {
                if (!LSEngine.TEST_MODE)
                {
                    throw ex;
                }

                return false;
            }
            return true;
        }

        public bool ValidateNewAccount(Account accountNode)
        {
            try
            {
                if (accountNode.Name.Length > 155)
                {
                    throw new LightspeedException("Could not save, as account length is more than 155 characters!", LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION);
                }

                if (accountNode.Name.Trim().Length < 1)
                {
                    throw new LightspeedException("Could not save, as account length is too short!", LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION);
                }
            }
            catch (Exception ex)
            {
                if (!LSEngine.TEST_MODE)
                {
                    throw ex;
                }

                return false;
            }
            return true;
        }


        //Send Request Methods
        #region Send Request Methods

        public void SaveAccount(Account accountNode)
        {
            ValidateNewAccount(accountNode);

            GroupAccount existingAccount = GetGroupAccountByName(accountNode.Id);

            if (existingAccount == null)
            {
                CreateGroupAccount newAccount = new CreateGroupAccount { MyID = LSEngine.SessionId };
                newAccount.GroupAccount = new GroupAccount
                {
                    DisplayName = accountNode.Name,
                    Id = accountNode.Name,
                    OwnerID = accountNode.ParentGroup.Id,
                    Settings = null,
                    Notes = LSEngine.ChangeSettingReason
                };
                LSEngine.SendMessage(newAccount);
            }
            else
            {
                throw new LightspeedException("Group / Account already exists: " + existingAccount.DisplayName, LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION);
            }
        }

        public void SaveGroup(Group group)
        {
            GroupAccount existingGroup = GetGroupAccount(group.Id);

            if (existingGroup == null)
            {
                CreateGroupAccount newGroup = new CreateGroupAccount
                {
                    MyID = LSEngine.SessionId,
                    GroupAccount = new GroupAccount()
                };
                if (group.IsAccountGroup)
                {
                    //Stand alone Account
                    newGroup.GroupAccount.DisplayName = group.GroupAccount.Name;
                    newGroup.GroupAccount.Id = group.GroupAccount.Name;
                    newGroup.GroupAccount.OwnerID = group.GroupAccount.Name;
                    newGroup.GroupAccount.Settings = group.RiskSetting.GetUpdatedRiskSettingProperties();
                }
                else
                {
                    //Group
                    newGroup.GroupAccount.DisplayName = group.Name;
                    newGroup.GroupAccount.Id = group.Id;
                    newGroup.GroupAccount.OwnerID = null;
                    newGroup.GroupAccount.Settings = group.RiskSetting.GetUpdatedRiskSettingProperties();
                }
                newGroup.GroupAccount.Notes = LSEngine.ChangeSettingReason;
                LSEngine.SendMessage(newGroup);
            }
            else
            {
                if (!group.IsAccountGroup)
                {
                    UpdateGroupName updateGroupNameRequest = new UpdateGroupName();
                    updateGroupNameRequest.MyID = LSEngine.SessionId;
                    updateGroupNameRequest.GroupAccount = existingGroup;
                    updateGroupNameRequest.GroupAccount.DisplayName = group.Name;
                    updateGroupNameRequest.GroupAccount.Notes = LSEngine.ChangeSettingReason;
                    LSEngine.SendMessage(updateGroupNameRequest);
                }
                else
                {
                    throw new Exception("Could not modify stand-alone account");
                }
            }
        }

        public void RemoveGroupNode(Group groupNode)
        {
            DeleteGroup deleteGroupRequest = new DeleteGroup();
            deleteGroupRequest.MyID = LSEngine.SessionId;
            deleteGroupRequest.GroupAccount = GetGroupAccount(groupNode.Id);
            if (deleteGroupRequest.GroupAccount == null)
            {
                throw new Exception("Not found Group/Account with Id: " + groupNode.Id);
            }
            deleteGroupRequest.GroupAccount.Notes = LSEngine.ChangeSettingReason;
            LSEngine.SendMessage(deleteGroupRequest);
        }

        public void RemoveAccountNode(Account accountNode)
        {
            DeleteAccount deleteAccountRequest = new DeleteAccount();
            deleteAccountRequest.MyID = LSEngine.SessionId;
            deleteAccountRequest.GroupAccount = GetGroupAccount(accountNode.Id);
            if (deleteAccountRequest.GroupAccount == null)
            {
                throw new Exception("Not found Group/Account with Id: " + accountNode.Id);
            }
            deleteAccountRequest.GroupAccount.Notes = LSEngine.ChangeSettingReason;
            LSEngine.SendMessage(deleteAccountRequest);
        }

        public void MoveAccount(string accountId, string newGroupId)
        {
            MoveAccount moveAccountRequest = new MoveAccount();
            moveAccountRequest.MyID = LSEngine.SessionId;
            moveAccountRequest.GroupAccount = GetGroupAccount(accountId);
            if (moveAccountRequest.GroupAccount == null)
            {
                throw new Exception("Not found Account with Id: " + accountId);
            }
            moveAccountRequest.GroupAccount.OwnerID = newGroupId;
            moveAccountRequest.GroupAccount.Notes = LSEngine.ChangeSettingReason;
            LSEngine.SendMessage(moveAccountRequest);
        }

        public void UpdateRiskSetting(Group group)
        {
            UpdateGroupSettings updatedRiskSettingRequest = new UpdateGroupSettings();
            updatedRiskSettingRequest.MyID = LSEngine.SessionId;
            updatedRiskSettingRequest.GroupAccount = GetGroupAccount(group.Id);
            updatedRiskSettingRequest.GroupAccount.Settings = group.RiskSetting.GetUpdatedRiskSettingProperties();
            updatedRiskSettingRequest.GroupAccount.Notes = LSEngine.ChangeSettingReason;
            LSEngine.SendMessage(updatedRiskSettingRequest);
        }

        public bool SaveGroupAccount(GroupAccount groupAccount, string tradeSetting)
        {
            groupAccount.Settings.PropertyMap.tagValues[CompanySetting.DAY_TRADING_ON_OFF_CSV] = tradeSetting;

            UpdateGroupSettings msg = new UpdateGroupSettings
            {
                MyID = LSEngine.SessionId,
                GroupAccount = groupAccount,
                OverRideGroupRestriction = 1 // need to set this to allow updating an account in a group separately from the group.
            };
            msg.GroupAccount.Settings = groupAccount.Settings;
            msg.GroupAccount.Notes = CompanySetting.DAY_TRADING_ON_OFF_CSV + "=" + tradeSetting;
            return LSEngine.SendMessage(msg);
        }

        #endregion

        public void UpdateGroupNodeRiskSetting(GroupAccount groupAccount)
        {
            Group group = FindNodeEntity(groupAccount.Id) as Group;
            if (group != null)
            {
                group.RiskSetting = new RiskSetting(groupAccount.Settings.PropertyMap.tagValues);
            }
        }

        public void UpdateGroupNodeName(GroupAccount groupAccount)
        {
            Group group = FindNodeEntity(groupAccount.Id) as Group;
            if (group != null)
            {
                group.Name = groupAccount.DisplayName;
            }
        }

        public void MoveAccountNodeUpdateSetting(Properties properties, string accountNodeId)
        {
            if (LSEngine.IsAccountSettingON)
            {
                INodeEntity node = FindNodeEntity(accountNodeId);
                if (node is Group)
                {
                    (node as Group).AddRiskSetting(properties);
                }
                else if (node is Account)
                {
                    (node as Account).AddRiskSetting(properties);
                }
            }
        }

        public Group SelectedGroupNode()
        {
            foreach (INodeEntity node in AccountGroupList)
            {
                if (node.IsSelected)
                {
                    return node as Group;
                }

                if (!((Group)node).IsAccountGroup)
                    foreach (Account acc in (node as Group).AccountList)
                    {
                        if (acc.IsSelected)
                        {
                            return node as Group;
                        }
                    }
            }
            return GetDefaultGroupNode();
        }

        public Group FindGroupNodeByName(string groupName)
        {
            INodeEntity node = AccountGroupList.Where(g => g.Name.Trim().ToLower().Equals(groupName.ToLower())).FirstOrDefault();
            if (node != null)
            {
                Group group = node as Group;
                return group;
            }
            return null;
        }

        public Group FindCaseSensitiveGroupNodeByName(string groupName)
        {
            INodeEntity node = AccountGroupList.Where(g => g.Name.Trim().Equals(groupName)).FirstOrDefault();
            if (node != null)
            {
                Group group = node as Group;
                return group;
            }
            return null;
        }


        public void UpdatePriceCheck(TagValueMsg tagValueMsg)
        {
            IsPriceCheckON = null;
            IsPriceCheckSpecified = false;
            if (tagValueMsg != null)
            {
                if (tagValueMsg.tagValues.ContainsKey(PRICE_CHECK_KEY))
                {
                    IsPriceCheckSpecified = true;
                    string priceCheckValue = tagValueMsg.tagValues[PRICE_CHECK_KEY].ToString();
                    if (priceCheckValue.Trim().ToUpper().Equals("TRUE"))
                    {
                        IsPriceCheckON = true;
                    }
                    else
                    {
                        IsPriceCheckON = false;
                    }
                }
            }
        }

        public void Alphabetize()
        {
            if (IsAlphabetize)
            {
                INodeEntity defaultGroup = null;
                List<INodeEntity> clonedGroupList = new List<INodeEntity>();
                List<INodeEntity> clonedAccountGroupList = new List<INodeEntity>();

                var clonedAllGroupList = AccountGroupList.OrderBy(g => g.Name).ToList();
                AccountGroupList.Clear();
                foreach (INodeEntity clonedAccountGroup in clonedAllGroupList)
                {
                    if (clonedAccountGroup is Group accountGroup && !accountGroup.IsAccountGroup)
                    {
                        MTObservableCollection<Account> cloneAccountList = new MTObservableCollection<Account>();
                        foreach (Account account in accountGroup.AccountList.OrderBy(a => a.Name).ToList())
                        {
                            cloneAccountList.Add(account);
                        }

                        accountGroup.AccountList.Clear();
                        accountGroup.AccountList = cloneAccountList;

                        if (accountGroup.IsDefaultGroup)
                        {
                            defaultGroup = accountGroup;
                        }
                        else
                        {
                            clonedGroupList.Add(accountGroup);
                        }
                    }
                    else if (clonedAccountGroup is Group && (clonedAccountGroup as Group).IsAccountGroup)
                    {
                        clonedAccountGroupList.Add(clonedAccountGroup);
                    }
                }

                if (defaultGroup != null)
                {
                    AccountGroupList.Add(defaultGroup);
                }

                foreach (INodeEntity group in clonedGroupList)
                {
                    AccountGroupList.Add(group);
                }

                foreach (INodeEntity accountGroup in clonedAccountGroupList)
                {
                    AccountGroupList.Add(accountGroup);
                }

                lsEngine.StatusMessage = "Alphabetize Group/Account Display-Order.";
            }
        }

        private ReadGroupAccounts accounts;
        private ReadGroupAccounts Accounts
        {
            get => accounts;
            set
            {
                accounts = value;
                this.RaisePropertyChanged(p => p.Accounts);
            }
        }

        public List<GroupAccount> SortAccountsAlphabetically(List<GroupAccount> input)
        {
            List<GroupAccount> clonedList = new List<GroupAccount>(input.Count);


            for (int i = 0; i < input.Count; i++)
            {
                var item = input[i];
                var currentIndex = i;

                while (currentIndex > 0 && string.CompareOrdinal(clonedList[currentIndex - 1].Id, item.Id) > 0)
                {
                    currentIndex--;
                }

                clonedList.Insert(currentIndex, item);
            }

            return clonedList;
        }

        public void ToggleAccountEnabled(string newMsg)
        {
            EZXMsg msg = new EZXMsg();
            msg.encode();

            LSEngine.SendMessage(msg);
        }
    }
}
