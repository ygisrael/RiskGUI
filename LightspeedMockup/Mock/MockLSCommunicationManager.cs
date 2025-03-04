using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZX.LightSpeedEngine;
using EZX.LightSpeedEngine.Config;
using EZXLib;
using System.Threading;

namespace EZX.LightspeedMockup.Mock
{
    public class MockLSCommunicationManager : ILSCommunicationManager
    {
        private MockEZXLibCommunicationManager comMgr;
        private LSEngine lsEngine;
        private int lastSeqNo;

        public LSConnectionInfo ConnectionInfo { get; set; }
        public int LastSeqNo
        {
            get 
            {
                return lastSeqNo;
            }
        }

        public MockLSCommunicationManager(LSEngine lsEngine)
        {
            this.lsEngine = lsEngine;
        }

        public void Init(ConfigInfo config)
        {
            this.ConnectionInfo = config.LSConnectionInfo;
            this.comMgr = new MockEZXLibCommunicationManager(this);
        }


        public void onConnected()
        {
            if (this.ConnectionInfo.Host.Equals("mock.mock.mock"))
            {
                if (this.ConnectionInfo.Company.Equals("TESTCOMPANY"))
                {
                    this.lsEngine.OnCommunicationManagerConnected(this);
                }
                else
                {
                    this.lsEngine.OnCommunicationManagerDisconnected(this);
                }
            }
            else
            {
                this.lsEngine.OnCommunicationManagerErrorOccurred(this);
            }
        }

        public void onConnectError(string errMsg)
        {
            this.lsEngine.OnCommunicationManagerErrorOccurred(this);
        }

        public void onDisconnected()
        {
            this.lsEngine.OnCommunicationManagerDisconnected(this);
        }

        public void onLogonResponse(EZXLib.LogonResponse logonResponse)
        {
            this.lsEngine.OnCommunicationManagerLoggedIn(this);
        }

        public static int ORDER_SEQNO = 0;

        private static void CreateClientOrderResponse(ClientOrderResponse response, int seqNo, int OrderId, string symbol, int qty, double price, int tempAccountNo)
        {
            response.SeqNo = seqNo;
            response.OrderID = OrderId;
            response.Symbol = symbol;
            response.CumQty = qty;
            response.Price = price;
            response.OrdType = "2";
            response.ClientNameID = 1;

            response.Account = "Account A" + tempAccountNo;
            response.State = "ACKED";
            response.TimeInForce = 1;
            response.TotExec = price * qty;
            response.UserName = "mockuser";
            response.TimeStamp = DateTime.Now.ToString("yyyyMMdd-HH:mm:ss");
        }

        public void onOrderResponse(EZXLib.OrderResponse orderResponse)
        {
            
        }

        public void onMarketDataSnapshot(EZXLib.MarketDataSnapshot snapshot)
        {
            
        }

        public void onMarketDataIncremental(EZXLib.MarketDataIncremental incremental)
        {
            
        }

        public void onParentOrderResponse(EZXLib.ParentOrderResponse parentOrderResponse)
        {
            
        }

        public void onRejectResponse(EZXLib.Reject rejectResponse)
        {
            
        }

        public void onOrderInfoResponse(EZXLib.OrderInfoResponse orderInfoResponse)
        {
            
        }

        public void onExecutionResponse(EZXLib.ExecutionResponse execResponse)
        {
            
        }

        public void onBroadcastResponse(EZXLib.BroadcastResponse broadcastResponse)
        {
            
        }

        public void onDestinationStatus(EZXLib.DestinationStatus destinationStatus)
        {
            
        }

        public void onOMSSettings(EZXLib.OMSSettings omsSettings)
        {
            
        }

        public void onGetOrdersResponse(EZXLib.GetOrdersResponse getOrdersResponse)
        {
            
        }

        public void onClientOrderResponse(EZXLib.ClientOrderResponse clientOrderResponse)
        {
            //this.lastSeqNo = clientOrderResponse.SeqNo;
            //this.lsEngine.OnClientOrderResponse(clientOrderResponse, this);
        }

        public void onIndicationOfInterest(EZXLib.IOI ioi)
        {
            
        }

        public void onAllocationResponse(EZXLib.AllocationResponse allocationResponse)
        {
            
        }

        public void onAllocationInfoResponse(EZXLib.AllocationInfoResponse allocInfoResponse)
        {
            
        }

        public void onChangePasswordResponse(EZXLib.ChangePasswordResponse changePasswordResponse)
        {
            
        }

        public void onForgotPasswordResponse(EZXLib.ForgotPasswordResponse forgotPasswordResponse)
        {
            
        }

        public void onCompanySettingUpdateResponse(EZXLib.CompanySettingsUpdate companySettingsUpdate)
        {
            
        }


        public int Connect()
        {
            EZXLib.Logger.INFO("LSCommunicationManager.Connect() ...");
            return this.comMgr.Connect(this.ConnectionInfo.Host, this.ConnectionInfo.Port, this.ConnectionInfo.IsSSL);
        }


        public void onReadGroupAccounts(EZXLib.ReadGroupAccounts readGroupAccounts)
        {
            this.lsEngine.onReadGroupAccounts(readGroupAccounts);
        }

        public void onCreateGroupAccount(EZXLib.CreateGroupAccount createGroupAccount)
        {
            this.lsEngine.onCreateGroupAccount(createGroupAccount);
        }


        public bool SendMessage(EZXLib.EZXMsg ezxMsg)
        {
            return this.comMgr.SendMessage(ezxMsg);
        }

        public void MockSendClientOrderResponse(int orderCount)
        {
            for (int x = 0; x < orderCount; x++)
            {
                ORDER_SEQNO++;
                int tempAccountNo = x % 10;

                ClientOrderResponse response = new ClientOrderResponse();
                int seqNo = ORDER_SEQNO;
                int OrderId = ORDER_SEQNO;
                string symbol = "IBM";
                int qty = 100;
                double price = 25.00;

                CreateClientOrderResponse(response, seqNo, OrderId, symbol, qty, price, (tempAccountNo + 1));
                this.onClientOrderResponse(response);

            }
        }


        internal void MockSendClientOrderResponse2(int orderCount)
        {
            orderCount = orderCount + 1001;
            for (int x = 1001; x < orderCount; x++)
            {
                ORDER_SEQNO++;
                int tempAccountNo = 0;
                if (x % 2 == 0)
                {
                    tempAccountNo = 25;
                }
                else
                {
                    tempAccountNo = 30;
                }

                if (x > 1006)
                {
                    tempAccountNo = x % 10;
                }
                ClientOrderResponse response = new ClientOrderResponse();
                int seqNo = ORDER_SEQNO;
                int OrderId = ORDER_SEQNO;
                string symbol = "MSFT";
                int qty = 100;
                double price = 25.00;

                CreateClientOrderResponse(response, seqNo, OrderId, symbol, qty, price, (tempAccountNo + 1));
                this.onClientOrderResponse(response);

            }
        }

        public void onDeleteAccount(DeleteAccount deleteAccount)
        {
            if (deleteAccount.MyID.Equals("11111111"))
            {
                deleteAccount.MyID = "111111112";
            } 
            this.lsEngine.onDeleteAccount(deleteAccount);
        }

        public void onDeleteGroup(DeleteGroup deleteGroup)
        {
            if (deleteGroup.MyID.Equals("11111111"))
            {
                deleteGroup.MyID = "111111112";
            }
            this.lsEngine.onDeleteGroup(deleteGroup);
        }

        public void onMoveAccount(MoveAccount moveAccount)
        {
            if (moveAccount.MyID.Equals("11111111"))
            {
                moveAccount.MyID = "111111112";
            }
            this.lsEngine.onMoveAccount(moveAccount);
        }

        public void onUpdateGroupName(UpdateGroupName updateGroupName)
        {
            lsEngine.onUpdateGroupName(updateGroupName);
        }

        public void onUpdateGroupSettings(UpdateGroupSettings updateGroupSettings)
        {
            lsEngine.onUpdateGroupSettings(updateGroupSettings);
        }

        public void Disconnect()
        {
            
        }


        public void onPositionResponse(PositionResponse positionResponse)
        {
        }

        public void onClientSettingUpdateResponse(ClientSettingsUpdate clientSettingsUpdate)
        {
        }


        public void onFixedIncomeMarketDataIncrementalRefresh(FixedIncomeMarketDataIncrementalRefresh incremental)
        {
        }

        public void onMarketDataHealthMessage(MarketDataHealthMessage mdMessage)
        {
        }

        public void onOtherMsg(EZXMsg msg)
        {
        }

        public void onQuoteResponseMessage(QuoteResponse quoteResponse)
        {
        }
    }
}
