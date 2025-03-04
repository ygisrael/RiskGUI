using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXLib;

namespace EZX.LightSpeedEngine
{
    public class LSConnectionEventArgs : EventArgs
    {
        private ILSCommunicationManager lsComMgr;

        public ILSCommunicationManager LsComMgr
        {
            get { return lsComMgr; }
            set { lsComMgr = value; }
        }

        public LSConnectionEventArgs(ILSCommunicationManager lsComMgr)
        {
            Logger.DEBUG("LSConnectionEventArgs(ILSCommunicationManager lsComMgr) started");

            this.LsComMgr = lsComMgr;

            Logger.DEBUG("LSConnectionEventArgs(ILSCommunicationManager lsComMgr) finished");
        }

    }
}
