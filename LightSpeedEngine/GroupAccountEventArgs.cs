using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXLib;

namespace EZX.LightSpeedEngine
{
    public class GroupAccountEventArgs : EventArgs
    {
        private GroupAccount groupAccount;

        public GroupAccount GroupAccount
        {
          get { return groupAccount; }
          set { groupAccount = value; }
        }

        public GroupAccountEventArgs()
            : base()
        {
        }

        public GroupAccountEventArgs(EZXLib.GroupAccount groupAccount): this()
        {
            this.GroupAccount = groupAccount;
        }
    }
}
