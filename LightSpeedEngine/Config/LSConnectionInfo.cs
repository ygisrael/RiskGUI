using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;
using System.Xml.Serialization;
using EZXLib;

namespace EZX.LightSpeedEngine.Config
{
    public class LSConnectionInfo : ObservableBase
    {
        private string host;
        private int port;
        private string company;
        private bool isSSL;
        private int connectionId;

        public string Host
        {
            get { return this.host; }
            set
            {
                this.host = value;
                this.RaisePropertyChanged(p=>p.Host);
            }
        }
        public int Port
        {
            get { return this.port; }
            set
            {
                this.port = value;
                this.RaisePropertyChanged(p=>p.Port);
            }
        }
        public string Company
        {
            get { return this.company; }
            set
            {
                this.company = value;
                this.RaisePropertyChanged(p=>p.Company);
            }
        }
        public bool IsSSL
        {
            get { return isSSL; }
            set
            {
                isSSL = value;
                this.RaisePropertyChanged(p=>p.IsSSL);
            }
        }

        [XmlIgnore()]
        public int ConnectionId
        {
            get { return connectionId; }
            set
            {
                connectionId = value;
                this.RaisePropertyChanged(p=>p.ConnectionId);
            }
        }

        public LSConnectionInfo()
            : base()
        {
            Logger.DEBUG("LSConnectionInfo() ...");
        }

        public LSConnectionInfo(string company, string host, int port, bool isSSL)
            : base()
        {
            Logger.DEBUG("LSConnectionInfo(string company, string host, int port, bool isSSL) ...");
            this.Company = company;
            this.Host = host;
            this.Port = port;
            this.IsSSL = isSSL;
        }
        
    }
}
