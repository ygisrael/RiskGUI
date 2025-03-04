using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZX.LightspeedEngine.Entity;

namespace EZXLightspeedGUI.Model
{
    public class NodeImportEntity
    {
        private string groupId;
        private string groupName;
        private string accountId;
        private double buyingPower;
        private int lineNumber;
        private bool isSubmitted;
        private INodeEntity nodeEntity;


        public string GroupId
        {
            get { return groupId; }
            set { groupId = value; }
        }

        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        public string AccountId
        {
            get { return accountId; }
            set { accountId = value; }
        }

        public double BuyingPower
        {
            get { return buyingPower; }
            set { buyingPower = value; }
        }

        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; }
        }

        public bool IsSubmitted
        {
            get { return isSubmitted; }
            set { isSubmitted = value; }
        }

        public INodeEntity NodeEntity
        {
            get { return nodeEntity; }
            set { nodeEntity = value; }
        }
        public NodeImportEntity()
        {
        }

    }
}
