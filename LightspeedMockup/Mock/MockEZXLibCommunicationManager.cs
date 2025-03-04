using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXLib;
using EZX.LightspeedMockup.Utils;

namespace EZX.LightspeedMockup.Mock
{
    public class MockEZXLibCommunicationManager
    {
        private MockLSCommunicationManager mockLSCommunicationManager;

        public MockEZXLibCommunicationManager(MockLSCommunicationManager mockLSCommunicationManager)
        {
            this.mockLSCommunicationManager = mockLSCommunicationManager;
        }

        public int Connect(string host, int port, bool isSSL)
        {
            this.mockLSCommunicationManager.onConnected();
            if (this.mockLSCommunicationManager.ConnectionInfo.Company.Equals("TESTCOMPANY"))
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        internal bool SendMessage(EZXLib.EZXMsg ezxMsg)
        {
            if (ezxMsg != null && ezxMsg is LogonRequest)
            {
                LogonRequest logonRequest = ezxMsg as LogonRequest;
                if (logonRequest.CompanyName.Equals("TESTCOMPANY") && logonRequest.UserName.Equals(logonRequest.Password))
                {
                    MockLSCommunicationManager.ORDER_SEQNO = logonRequest.SeqNo;
                    if (logonRequest.UserName.Equals("mockdata100"))
                    {
                        this.mockLSCommunicationManager.MockSendClientOrderResponse(100);
                    }

                    LogonResponse resposne = new LogonResponse();
                    resposne.ReturnCode = 0;
                    resposne.ReturnDesc = "";


                    this.mockLSCommunicationManager.onLogonResponse(resposne);

                    if (logonRequest.UserName.Equals("mockdata100"))
                    {
                        this.mockLSCommunicationManager.MockSendClientOrderResponse2(10);
                    }

                    return true;
                }
                else
                {
                    this.mockLSCommunicationManager.onDisconnected();
                    return false;
                }
            }
            else if (ezxMsg != null && ezxMsg is ReadGroupAccounts)
            {
                ReadGroupAccounts readGroupAccounts = new ReadGroupAccounts();
                MockGroupAccountData(readGroupAccounts);
                this.mockLSCommunicationManager.onReadGroupAccounts(readGroupAccounts);
            }
            else if (ezxMsg != null && ezxMsg is CreateGroupAccount)
            {
                CreateGroupAccount newGroupAccount = ezxMsg as CreateGroupAccount;
                newGroupAccount.ReturnCode = 0;

                this.mockLSCommunicationManager.onCreateGroupAccount(newGroupAccount);
            }
            else if (ezxMsg != null && ezxMsg is DeleteAccount)
            {
                DeleteAccount deleteAccountResponse = ezxMsg as DeleteAccount;
                deleteAccountResponse.ReturnCode = 0;

                this.mockLSCommunicationManager.onDeleteAccount(deleteAccountResponse);
            }
            else if (ezxMsg != null && ezxMsg is DeleteGroup)
            {
                DeleteGroup deleteGroupResponse = ezxMsg as DeleteGroup;
                deleteGroupResponse.ReturnCode = 0;

                this.mockLSCommunicationManager.onDeleteGroup(deleteGroupResponse);
            }
            else if (ezxMsg != null && ezxMsg is MoveAccount)
            {
                MoveAccount moveAccountResponse = ezxMsg as MoveAccount;
                moveAccountResponse.ReturnCode = 0;

                this.mockLSCommunicationManager.onMoveAccount(moveAccountResponse);
            }
            else if (ezxMsg != null && ezxMsg is UpdateGroupName)
            {
                UpdateGroupName updateGroupNameResponse = ezxMsg as UpdateGroupName;
                updateGroupNameResponse.ReturnCode = 0;

                this.mockLSCommunicationManager.onUpdateGroupName(updateGroupNameResponse);
            }
            else if (ezxMsg != null && ezxMsg is UpdateGroupSettings)
            {
                UpdateGroupSettings updateGroupSettingsResponse = ezxMsg as UpdateGroupSettings;
                updateGroupSettingsResponse.ReturnCode = 0;

                this.mockLSCommunicationManager.onUpdateGroupSettings(updateGroupSettingsResponse);
            }

            return false;
        }

        private void MockGroupAccountData(ReadGroupAccounts readGroupAccounts)
        {
            List<GroupAccount> groupAccountList = new List<GroupAccount>();

            GroupAccount defaultGroup = CreateNewGroupAccount("d1", "Default Group", 1, new Properties(), null);
            groupAccountList.Add(defaultGroup);

            GroupAccount group1 = CreateNewGroupAccount("g1", "Group A", 0, new Properties(), null);
            GroupAccount group2 = CreateNewGroupAccount("g2", "Group B", 0, new Properties(), null);

            GroupAccount accontA1 = CreateNewGroupAccount("ga1", "Account A1", 0, null, "g1");
            GroupAccount accontA2 = CreateNewGroupAccount("ga2", "Account A2", 0, null, "g1");
            GroupAccount accontA3 = CreateNewGroupAccount("ga3", "Account A3", 0, null, "g1");
            GroupAccount accontB1 = CreateNewGroupAccount("gb1", "Account B1", 0, null, "g2");
            GroupAccount accontB2 = CreateNewGroupAccount("gb2", "Account B2", 0, null, "g2");

            groupAccountList.Add(accontA1);
            groupAccountList.Add(accontA2);
            groupAccountList.Add(accontB1);

            groupAccountList.Add(group1);
            groupAccountList.Add(group2);

            groupAccountList.Add(accontB2);
            groupAccountList.Add(accontA3);



            PrivateMemberUtil.SetPrivateField(readGroupAccounts, "groupAccounts", groupAccountList);

        }


        private GroupAccount CreateNewGroupAccount(string id, string name, int isDefault, Properties settings, string ownerId)
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
