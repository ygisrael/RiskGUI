using EZX.LightspeedEngine.Entity;
using EZX.LightSpeedEngine.Config;
using EZXLib;
using EZXWPFLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Windows;

namespace EZX.LightSpeedEngine
{
    public delegate void LoginCompleteHandler(object sender, LSConnectionEventArgs e);
    public delegate void ConnectHandler(object sender, LSConnectionEventArgs e);
    public delegate void DisconnectHandler(object sender, LSConnectionEventArgs e);
    public delegate void ConnectErrorOccuredHandler(object sender, LSConnectionEventArgs e);
    public delegate void LoggedOutHandler(object sender, LSConnectionEventArgs e);
    public delegate void LoadAllGroupAndAccountHandler(object sender, LSConnectionEventArgs e);
    public delegate void UpdateGroupAccountSettingHandler(object sender, GroupAccountEventArgs e);
    public delegate void UpdateGroupNameHandler(object sender, LSConnectionEventArgs e);
    public delegate void MoveAccountHandler(object sender, GroupAccountEventArgs e);

    public class LSEngine : ObservableBase
    {
        public static bool TEST_MODE = false;

        private DataManager dataManager;
        private ILSCommunicationManager lsComMgr;
        private ConfigInfo configInfo;

        private string sessionId;
        private bool loggedIn;
        private string statusMessage = "Start Processing Lightspeed UI ...";
        private bool isAccountSettingON;
        private bool isConnected;
        private string hostandUserText = string.Format("Host: {0}, User: {1}", string.Empty, string.Empty);
        private string changeSettingReason;

        public string SessionId
        {
            get => sessionId;
            set
            {
                sessionId = value;
                this.RaisePropertyChanged(p => p.SessionId);
            }
        }
        public bool LoggedIn
        {
            get => loggedIn;
            set
            {
                loggedIn = value;
                this.RaisePropertyChanged(p => p.LoggedIn);
            }
        }
        public string StatusMessage
        {
            get => ("(" + DateTime.Now.ToString("HH:mm:ss") + ") " + statusMessage);
            set
            {
                statusMessage = value;
                this.RaisePropertyChanged(p => p.StatusMessage);
            }
        }
        public bool IsAccountSettingON
        {
            get => isAccountSettingON;
            set
            {
                isAccountSettingON = value;
                this.RaisePropertyChanged(p => p.IsAccountSettingON);
            }
        }
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                this.RaisePropertyChanged(p => p.IsConnected);
            }
        }
        public string HostandUserText
        {
            get => hostandUserText;
            set
            {
                hostandUserText = value;
                this.RaisePropertyChanged(p => p.HostandUserText);
            }
        }
        public string ChangeSettingReason
        {
            get => changeSettingReason;
            set
            {
                changeSettingReason = value;
                this.RaisePropertyChanged(p => p.ChangeSettingReason);
            }
        }

        public DataManager DataManager
        {
            get => dataManager;
            set
            {
                dataManager = value;
                this.RaisePropertyChanged(p => p.DataManager);
            }
        }
        public ILSCommunicationManager LSComMgr
        {
            get => lsComMgr;
            set
            {
                lsComMgr = value;
                this.RaisePropertyChanged(p => p.LSComMgr);
            }
        }
        public ConfigInfo ConfigInfo
        {
            get => configInfo;
            set
            {
                configInfo = value;
                this.RaisePropertyChanged(p => p.ConfigInfo);
            }
        }

        public event LoginCompleteHandler LoginCompleted;
        public event ConnectHandler Connected;
        public event DisconnectHandler Disconnected;
        public event ConnectErrorOccuredHandler ConnectErrorOccured;
        public event DisconnectHandler LoggedOut;
        public event LoadAllGroupAndAccountHandler LoadAllGroupAndAccountCompleted;
        public event UpdateGroupAccountSettingHandler UpdateGroupAccountSettingCompleted;
        public event UpdateGroupNameHandler UpdateGroupNameCompleted;
        public event MoveAccountHandler MoveAccountCompleted;

        //private OMSEngineHelper omsEngine;

        public virtual bool Init(ConfigInfo configInfo, ILSCommunicationManager lsComMgr)
        {
            Logger.DEBUG("virtual public bool Init(ConfigInfo configInfo) STARTED");

            StatusMessage = "Initializing LS Engine...";

            if (configInfo == null || configInfo.LSConnectionInfo == null)
            {
                Logger.DEBUG("virtual public bool Init(ConfigInfo configInfo) FINISHED as (configInfo == null || configInfo.LSConnectionInfo)!");
                return false;
            }

            DataManager = new DataManager(this);
            ConfigInfo = configInfo;
            LSComMgr = lsComMgr;
            LSComMgr.Init(configInfo);
            DataManager.IsAlphabetize = ConfigInfo.IsAlphabetize;

            Logger.DEBUG("virtual public bool Init(ConfigInfo configInfo) COMPLETED");
            return true;
        }

        public virtual bool Connect()
        {
            Logger.DEBUG("Connect() Started");
            int connectionId = LSComMgr.Connect();
            if (connectionId < 0)
            {
                StatusMessage = "Failed to connect!";
                Logger.DEBUG("Connect() unsuccessfully Finished!");
                return false;
            }

            StatusMessage = "Trying to connect...";

            Logger.DEBUG("Connect() Completed");
            return true;
        }

        public virtual void Disconnect()
        {
            Logger.DEBUG("Disconnect() Started");
            LSComMgr.Disconnect();
            LoggedIn = false;
            Logger.DEBUG("Disconnect() Completed");
        }


        public virtual void Logon()
        {
            Logger.DEBUG("Logon() Started");
            LogonRequest logonRequest = new LogonRequest();
            logonRequest.UserName = ConfigInfo.Username;
            logonRequest.Password = ConfigInfo.Password;
            logonRequest.CompanyName = ConfigInfo.LSConnectionInfo.Company;
            logonRequest.SendDestStrategies = 0;
            logonRequest.SendSettings = 1;
            logonRequest.OmsVersion = "1.0.0.1";
            logonRequest.SendDestinationState = 0;
            logonRequest.DateGMT = "<TODAY>";
            logonRequest.SeqNo = LSComMgr.LastSeqNo;
            logonRequest.LogonType = LogonType.ALL;
            if (!LSComMgr.SendMessage(logonRequest))
            {
                //Error to send Logon request
            }

            StatusMessage = "Sending logon request...";

            Logger.DEBUG("Logon() Completed");
        }


        public virtual void Logout()
        {
            Logger.DEBUG("Logout() Started");

            DataManager.SaveGroupAccountConfig(string.Empty);
            Disconnect();
            LoggedIn = false;

            StatusMessage = "Logout successful.";

            LoggedOut?.Invoke(this, null);

            Logger.DEBUG("Logout() Completed");
        }

        public virtual bool SendMessage(EZXMsg msg)
        {
            Logger.INFO("SendMessage(EZXMsg msg) Started");

            bool status = LSComMgr.SendMessage(msg);

            Logger.INFO("SendMessage(EZXMsg msg) Completed");
            return status;
        }

        public virtual void OnCommunicationManagerConnected(ILSCommunicationManager ICommunicationManager)
        {
            Logger.DEBUG("OnCommunicationManagerConnected(ILSCommunicationManager ICommunicationManager) started");

            Connected?.Invoke(this, new LSConnectionEventArgs(ICommunicationManager));

            StatusMessage = "Connected.";
            IsConnected = true;

            Logon();

            Logger.DEBUG("OnCommunicationManagerConnected(ILSCommunicationManager ICommunicationManager) finished");
        }

        public virtual void OnCommunicationManagerLoggedIn(ILSCommunicationManager ICommunicationManager)
        {
            Logger.DEBUG("OnCommunicationManagerLoggedIn(ILSCommunicationManager ICommunicationManager) Started");

            LoggedIn = true;

            if (LoginCompleted != null)
            {
                LoginCompleted(this, new LSConnectionEventArgs(ICommunicationManager));
            }
            else
            {
                Logger.ERROR("OnCommunicationManagerLoggedIn(ILSCommunicationManager ICommunicationManager) LoginCompleted = null!");
            }

            HostandUserText = $"Host: {LSComMgr.ConnectionInfo.Host}, User: {ConfigInfo.Username}";

            GetGroupAccountData();

            StatusMessage = "Logon successfully.";

            Logger.DEBUG("OnCommunicationManagerLoggedIn(ILSCommunicationManager ICommunicationManager) Started");

        }

        public bool GetGroupAccountData()
        {
           return LSComMgr.SendMessage(new ReadGroupAccounts());
        }

        public virtual void OnCommunicationManagerDisconnected(ILSCommunicationManager ICommunicationManager)
        {
            Logger.DEBUG("OnCommunicationManagerDisconnected(ILSCommunicationManager ICommunicationManager) started");

            StatusMessage = "Disconnected.";
            IsConnected = false;
            Disconnected?.Invoke(this, new LSConnectionEventArgs(ICommunicationManager));

            HostandUserText = string.Format("Host: {0}, User: {1}", LSComMgr.ConnectionInfo.Host, ConfigInfo.Username);
            Logger.DEBUG("OnCommunicationManagerDisconnected(ILSCommunicationManager ICommunicationManager) Completed");
        }

        public virtual void OnCommunicationManagerErrorOccurred(ILSCommunicationManager ICommunicationManager)
        {
            Logger.DEBUG("OnCommunicationManagerDisconnected(ILSCommunicationManager ICommunicationManager) Started");

            LoggedIn = false;

            ConnectErrorOccured?.Invoke(this, new LSConnectionEventArgs(ICommunicationManager));

            HostandUserText = string.Format("Host: {0}, User: {1}", LSComMgr.ConnectionInfo.Host, ConfigInfo.Username);
            StatusMessage = "Disconnected (Failed to connect!).";

            Logger.DEBUG("OnCommunicationManagerDisconnected(ILSCommunicationManager ICommunicationManager) Completed");
        }

        public virtual void OnOrderResponse(OrderResponse orderResponse, ILSCommunicationManager ICommunicationManager)
        {
            Logger.DEBUG("OnOrderResponse(OrderResponse orderResponse, ILSCommunicationManager ICommunicationManager) Started");

            Logger.DEBUG("OnOrderResponse(OrderResponse orderResponse, ILSCommunicationManager ICommunicationManager) Completed");
        }

        public virtual void OnClientOrderResponse(ClientOrderResponse clientOrderResponse, ILSCommunicationManager ICommunicationManager)
        {
            Logger.DEBUG("OnClientOrderResponse(ClientOrderResponse clientOrderResponse, ILSCommunicationManager ICommunicationManager) Started");

            //this.DataManager.ProcessClientOrderResponse(clientOrderResponse);

            Logger.DEBUG("OnClientOrderResponse(ClientOrderResponse clientOrderResponse, ILSCommunicationManager ICommunicationManager) Completed");
        }


        private void ReselectGroupNode(string selectedGroupName, bool isSelectedGroupIsExpended)
        {
            if (!string.IsNullOrEmpty(selectedGroupName))
            {
                foreach (INodeEntity node in DataManager.AccountGroupList)
                {
                    if (node.Name.Equals(selectedGroupName))
                    {
                        node.IsSelected = true;
                        node.IsExpanded = isSelectedGroupIsExpended;
                        break;
                    }

                    node.IsSelected = false;
                }
            }
        }

        public void onCreateGroupAccount(CreateGroupAccount createGroupAccount)
        {
            dataManager.AddNewGroupAccount(createGroupAccount);
            if (string.IsNullOrEmpty(createGroupAccount.GroupAccount.OwnerID))
            {
                DataManager.Alphabetize();
                StatusMessage = "Added new group: " + createGroupAccount.GroupAccount.DisplayName;
            }
            else
            {
                DataManager.Alphabetize();
                StatusMessage = "Added new account: " + createGroupAccount.GroupAccount.DisplayName;
            }

        }

        public void onDeleteGroup(DeleteGroup deleteGroupResposne)
        {
            if (deleteGroupResposne.ReturnCode == 0)
            {
                GroupAccount group = DataManager.GetGroupAccount(deleteGroupResposne.GroupAccount.Id);
                if (deleteGroupResposne != null)
                {
                    DataManager.ReadGroupAccountsInstance.GroupAccounts.Remove(group);
                }

                //Other sessions are required to modify when the account is deleted
                if (!deleteGroupResposne.MyID.Equals(SessionId))
                {
                    DataManager.RemoveGroupAccountNode(deleteGroupResposne.GroupAccount);
                }

                StatusMessage = "Deleted group: " + deleteGroupResposne.GroupAccount.DisplayName;

            }
            else
            {
                StatusMessage = "Failed to delete group: " + deleteGroupResposne.GroupAccount.DisplayName;
            }
        }

        public void onDeleteAccount(DeleteAccount deleteAccountResponse)
        {
            if (deleteAccountResponse.ReturnCode == 0)
            {
                GroupAccount account = DataManager.GetGroupAccount(deleteAccountResponse.GroupAccount.Id);
                if (account != null)
                {
                    DataManager.ReadGroupAccountsInstance.GroupAccounts.Remove(account);
                }

                //Other sessions are required to modify when the account is deleted
                if (!deleteAccountResponse.MyID.Equals(SessionId))
                {
                    DataManager.RemoveGroupAccountNode(deleteAccountResponse.GroupAccount);
                }

                StatusMessage = "Deleted account: " + deleteAccountResponse.GroupAccount.DisplayName;
            }
            else
            {
                StatusMessage = "Failed to delete account: " + deleteAccountResponse.GroupAccount.DisplayName;
            }

        }

        public void onMoveAccount(MoveAccount moveAccountResposne)
        {
            if (moveAccountResposne.ReturnCode == 0)
            {
                GroupAccount account = DataManager.GetGroupAccount(moveAccountResposne.GroupAccount.Id);
                if (account != null)
                {
                    DataManager.ReadGroupAccountsInstance.GroupAccounts.Remove(account);
                }
                DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(moveAccountResposne.GroupAccount);

                //Other sessions are required to modify when the account is deleted
                if (!moveAccountResposne.MyID.Equals(SessionId))
                {
                    DataManager.MoveAccountNode(moveAccountResposne.GroupAccount.Settings, moveAccountResposne.GroupAccount.Id, moveAccountResposne.GroupAccount.OwnerID);
                }
                else
                {
                    DataManager.MoveAccountNodeUpdateSetting(moveAccountResposne.GroupAccount.Settings, moveAccountResposne.GroupAccount.Id);
                }

                MoveAccountCompleted?.Invoke(this, new GroupAccountEventArgs(moveAccountResposne.GroupAccount));

                DataManager.Alphabetize();

                StatusMessage = "Moved account: " + moveAccountResposne.GroupAccount.DisplayName;
            }
            else
            {
                StatusMessage = "Failed to move account: " + moveAccountResposne.GroupAccount.DisplayName;
            }
        }


        public void onUpdateGroupName(UpdateGroupName updateGroupNameResponse)
        {
            if (updateGroupNameResponse.ReturnCode == 0)
            {
                GroupAccount groupAccount = DataManager.GetGroupAccount(updateGroupNameResponse.GroupAccount.Id);
                if (groupAccount != null)
                {
                    groupAccount = updateGroupNameResponse.GroupAccount;
                    DataManager.UpdateGroupNodeName(groupAccount);
                }
                else
                {
                    DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(updateGroupNameResponse.GroupAccount);
                }

                DataManager.Alphabetize();

                StatusMessage = "Modified group name of group: " + updateGroupNameResponse.GroupAccount.DisplayName;
            }
            else
            {
                StatusMessage = "Failed to update group name of group: " + updateGroupNameResponse.GroupAccount.DisplayName;
            }


            UpdateGroupNameCompleted?.Invoke(this, new LSConnectionEventArgs(null));
        }

        public void onUpdateGroupSettings(UpdateGroupSettings updateGroupSettingsResponse)
        {
            if (updateGroupSettingsResponse.ReturnCode == 0)
            {
                GroupAccount groupAccount = DataManager.GetGroupAccount(updateGroupSettingsResponse.GroupAccount.Id);
                if (groupAccount != null)
                {
                    groupAccount = updateGroupSettingsResponse.GroupAccount;
                    DataManager.UpdateGroupNodeRiskSetting(groupAccount);
                }
                else
                {
                    DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(updateGroupSettingsResponse.GroupAccount);
                }
                StatusMessage = "Modified risk-settings of group/account: " + updateGroupSettingsResponse.GroupAccount.DisplayName;
            }
            else
            {
                StatusMessage = "Failed to update risk-settings of group/account: " + updateGroupSettingsResponse.GroupAccount.DisplayName;
            }


            UpdateGroupAccountSettingCompleted?.Invoke(this, new GroupAccountEventArgs(updateGroupSettingsResponse.GroupAccount));

        }


        public void UpdatePriceCheck(bool isPriceCheckON)
        {
            CompanySettingsUpdate companySettingsUpdate = new CompanySettingsUpdate();
            companySettingsUpdate.Settings = new Properties();
            companySettingsUpdate.Settings.PropertyMap = new TagValueMsg();
            companySettingsUpdate.Settings.PropertyMap.tagValues["MDS_ON"] = isPriceCheckON.ToString().ToLower();
            companySettingsUpdate.Notes = ChangeSettingReason;
            SendMessage(companySettingsUpdate);
        }


        public void OnUpdatePriceCheck(OMSSettings omsSettings)
        {
            Logger.DEBUG("public void OnUpdatePriceCheck(OMSSettings omsSettings)...");
            DataManager.UpdatePriceCheck(omsSettings.CompanySettings.PropertyMap);

            if (!DataManager.IsPriceCheckSpecified)
            {
                Logger.WARN("Key: [" + DataManager.PRICE_CHECK_KEY + "], is not received on  onOMSSetting response.");
            }
        }

        public void OnUpdatePriceCheck(CompanySettingsUpdate companySettingsUpdate)
        {
            Logger.DEBUG("public void OnUpdatePriceCheck(CompanySettingsUpdate companySettingsUpdate)...");
            if (companySettingsUpdate != null && companySettingsUpdate.Settings != null
                && companySettingsUpdate.Settings.PropertyMap != null)
            {
                DataManager.UpdatePriceCheck(companySettingsUpdate.Settings.PropertyMap);
            }

            if (!DataManager.IsPriceCheckSpecified)
            {
                Logger.WARN("Key: [" + DataManager.PRICE_CHECK_KEY + "], is not received on  onOMSSetting response.");
            }

            StatusMessage = "Price check has been modified.";
        }

        public void onReadGroupAccounts(ReadGroupAccounts readGroupAccounts)
        {
            Logger.DEBUG("OnCommunicationManagerLoggedIn(ILSCommunicationManager ICommunicationManager) Started");

            string selectedGroupName = (dataManager.SelectedGroupNode() == null ? string.Empty : dataManager.SelectedGroupNode().Name);
            bool isSelectedGroupIsExpended = (dataManager.SelectedGroupNode() != null && dataManager.SelectedGroupNode().IsExpanded);

            dataManager.LoadAllGroupAndAccount(readGroupAccounts);

            LoadAllGroupAndAccountCompleted?.Invoke(this, new LSConnectionEventArgs(null));

            DataManager.Alphabetize();

            ReselectGroupNode(selectedGroupName, isSelectedGroupIsExpended);

            StatusMessage = "Groups/Accounts loaded successfully.";
        }


        public List<string> ApexFilterList { get; set; }

        public void SetFilterValues(OMSSettings omsSettings)
        {
            ApexFilterList = new List<string>();
            foreach (ChoiceGroup settingsChoiceGroup in omsSettings.Settings.ChoiceGroups)
            {
                if (settingsChoiceGroup.ChoiceGroupName != "RiskGuiAccountFilter") continue;
                foreach (ChoiceItem choiceItem in settingsChoiceGroup.ChoiceItems)
                {
                  //  if (choiceItem.ItemName == "Filter")
                 //   {
                        ApexFilterList.Add(choiceItem.ItemValue);
                 //   }
                }
            }
            StatusMessage = "Accounts loaded successfully.";
        }

    }
}
