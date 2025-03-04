using EZX.LightSpeedEngine.Config;
using EZXLib;

namespace EZX.LightSpeedEngine
{
    public class LSCommunicationManager : ILSCommunicationManager
    {
        private LSEngine lsEngine;
        private CommunicationManager comMgr;
        private int lastSeqNo;

        public CommunicationManager ComMgr => comMgr;
        public LSConnectionInfo ConnectionInfo { get; set; }
        public int LastSeqNo => lastSeqNo;


        public LSCommunicationManager(LSEngine lsEngine)
        {
            Logger.DEBUG("LSCommunicationManager(LSEngine lsEngine) Started...");
            this.lsEngine = lsEngine;
            Logger.DEBUG("LSCommunicationManager(LSEngine lsEngine) Finished.");
        }

        public void Init(ConfigInfo config)
        {
            Logger.DEBUG("Init(ConfigInfo config) Started...");
            ConnectionInfo = config.LSConnectionInfo;
            comMgr = new CommunicationManager(this);
            Logger.DEBUG("Init(ConfigInfo config) Finished.");
        }


        public void onConnected()
        {
            Logger.DEBUG("LSCommunicationManager.onConnected() Method-call through EZXInterface");
            lsEngine.OnCommunicationManagerConnected(this);
        }

        public void onConnectError(string errMsg)
        {
            Logger.DEBUG("LSCommunicationManager.onConnectError(string errMsg) Method-call through EZXInterface, errMsg:" + errMsg);
            lsEngine.OnCommunicationManagerErrorOccurred(this);
        }

        public void onDisconnected()
        {
            Logger.DEBUG("LSCommunicationManager.onDisconnected() Method-call through EZXInterface");
            lsEngine.OnCommunicationManagerDisconnected(this);
        }

        public void onLogonResponse(LogonResponse logonResponse)
        {
            Logger.DEBUG("LSCommunicationManager.onLogonResponse(EZXLib.LogonResponse logonResponse) Method-call through EZXInterface, logonResponse.ReturnCode:" + logonResponse.ReturnCode);
            if (logonResponse.ReturnCode == 0)
            {
                lsEngine.OnCommunicationManagerLoggedIn(this);
            }
            else
            {
                lsEngine.OnCommunicationManagerErrorOccurred(this);
            }
        }

        public void onOrderResponse(OrderResponse orderResponse)
        {
            Logger.DEBUG("LSCommunicationManager.onOrderResponse(EZXLib.OrderResponse orderResponse) Method-call through EZXInterface, orderResponse.RouterOrderID:" + orderResponse.RouterOrderID);
            lsEngine.OnOrderResponse(orderResponse,this);
        }

        public void onMarketDataSnapshot(MarketDataSnapshot snapshot)
        {
            // throw new NotImplementedException();
        }

        public void onMarketDataIncremental(MarketDataIncremental incremental)
        {
            // throw new NotImplementedException();
        }

        public void onParentOrderResponse(ParentOrderResponse parentOrderResponse)
        {
            // throw new NotImplementedException();
        }

        public void onRejectResponse(Reject rejectResponse)
        {
            // throw new NotImplementedException();
        }

        public void onOrderInfoResponse(OrderInfoResponse orderInfoResponse)
        {
            // throw new NotImplementedException();
        }

        public void onExecutionResponse(ExecutionResponse execResponse)
        {
            // throw new NotImplementedException();
        }

        public void onBroadcastResponse(BroadcastResponse broadcastResponse)
        {
            // throw new NotImplementedException();
        }

        public void onDestinationStatus(DestinationStatus destinationStatus)
        {
            // throw new NotImplementedException();
        }

        public void onOMSSettings(OMSSettings omsSettings)
        {
            Logger.DEBUG("LSCommunicationManager.onOMSSettings(EZXLib.OMSSettings omsSettings) Method-call through EZXInterface" );
            lsEngine.OnUpdatePriceCheck(omsSettings);
            lsEngine.SetFilterValues(omsSettings);
        }

        public void onGetOrdersResponse(GetOrdersResponse getOrdersResponse)
        {
            // throw new NotImplementedException();
        }

        public void onClientOrderResponse(ClientOrderResponse clientOrderResponse)
        {
            //Logger.DEBUG("LSCommunicationManager.onClientOrderResponse(EZXLib.ClientOrderResponse clientOrderResponse) Method-call through EZXInterface, clientOrderResponse.OrderID: " + clientOrderResponse.OrderID);
            //this.lastSeqNo = clientOrderResponse.SeqNo;
            //this.lsEngine.OnClientOrderResponse(clientOrderResponse, this);
        }

        public void onIndicationOfInterest(IOI ioi)
        {
            // throw new NotImplementedException();
        }

        public void onAllocationResponse(AllocationResponse allocationResponse)
        {
            // throw new NotImplementedException();
        }

        public void onAllocationInfoResponse(AllocationInfoResponse allocInfoResponse)
        {
            // throw new NotImplementedException();
        }

        public void onChangePasswordResponse(ChangePasswordResponse changePasswordResponse)
        {
            // throw new NotImplementedException();
        }

        public void onForgotPasswordResponse(ForgotPasswordResponse forgotPasswordResponse)
        {
            // throw new NotImplementedException();
        }

        public void onCompanySettingUpdateResponse(CompanySettingsUpdate companySettingsUpdate)
        {
            Logger.DEBUG("LSCommunicationManager.onCompanySettingUpdateResponse(EZXLib.CompanySettingsUpdate companySettingsUpdate) Method-call through EZXInterface");
            lsEngine.OnUpdatePriceCheck(companySettingsUpdate);
        }


        public int Connect()
        {
            Logger.INFO("LSCommunicationManager.Connect() ...");
            //return comMgr.Connect(ConnectionInfo.Host, ConnectionInfo.Port, ConnectionInfo.IsSSL);
            return 1;
        }


        public void onReadGroupAccounts(ReadGroupAccounts readGroupAccounts)
        {            
            lsEngine.onReadGroupAccounts(readGroupAccounts);
        }

        public void onCreateGroupAccount(CreateGroupAccount createGroupAccount)
        {
            lsEngine.onCreateGroupAccount(createGroupAccount);    
        }

        public bool SendMessage(EZXMsg ezxMsg)
        {
            return ComMgr.SendMessage(ezxMsg);
        }


        public void onDeleteAccount(DeleteAccount deleteAccount)
        {
            lsEngine.onDeleteAccount(deleteAccount);
        }

        public void onDeleteGroup(DeleteGroup deleteGroup)
        {
            lsEngine.onDeleteGroup(deleteGroup);
        }

        public void onMoveAccount(MoveAccount moveAccount)
        {
            lsEngine.onMoveAccount(moveAccount);
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
            ComMgr.Disconnect(false, false);
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
