using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXLib;
using EZX.LightSpeedEngine.Config;

namespace EZX.LightSpeedEngine
{
    public interface ILSCommunicationManager : EZXInterface
    {
        LSConnectionInfo ConnectionInfo { get; set; }
        int Connect();
        void Disconnect();
        int LastSeqNo { get; }
        void Init(ConfigInfo config);
        bool SendMessage(EZXMsg ezxMsg);
    }
}
