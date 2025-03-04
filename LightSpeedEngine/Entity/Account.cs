using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZX.LightspeedEngine.Entity
{
    public partial class Account : EZXLightSpeedBaseEntity, INodeEntity
    {

        private string id;
        private string accountName;
        private int displayIndex;
        private bool isSelected;
        private bool isExpanded;
        private string ownerId;
        private bool isWaitingForServerResponse;
        private RiskSetting riskSetting;
        private Group parentGroup;
        private bool isOwnGroup;


        public string Id
        {
            get { return id; }
            set
            {
                id = value;
                this.RaisePropertyChanged("Id");
                this.RaisePropertyChanged("IsInEditMode");                
            }
        }

        public string Name
        {
            get 
            {
                return accountName; 
            }
            set
            {
                accountName = value;
                this.RaisePropertyChanged("Name");
            }
        }

        public int DisplayIndex
        {
            get { return displayIndex; }
            set
            {
                displayIndex = value;
                this.RaisePropertyChanged("DisplayIndex");
            }
        }

        public bool IsWaitingForServerResponse
        {
            get { return isWaitingForServerResponse; }
            set
            {
                isWaitingForServerResponse = value;
                this.RaisePropertyChanged("IsWaitingForServerResponse");
                this.RaisePropertyChanged("Name");
            }
        }

        public RiskSetting RiskSetting
        {
            get { return riskSetting; }
            set
            {
                riskSetting = value;
                this.RaisePropertyChanged("RiskSetting");
            }
        }

        public Group ParentGroup
        {
            get { return parentGroup; }
            set
            {
                parentGroup = value;
                this.RaisePropertyChanged("ParentGroup");
            }
        }

        public bool IsOwnGroup
        {
            get { return isOwnGroup; }
            set
            {
                isOwnGroup = value;
                this.RaisePropertyChanged("IsOwnGroup");
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                this.RaisePropertyChanged("IsSelected");
            }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                this.RaisePropertyChanged("IsExpanded");
            }
        }

        public string OwnerId
        {
            get 
            {
                if (this.ParentGroup != null && !string.IsNullOrEmpty(this.ParentGroup.Id))
                {
                    return this.ParentGroup.Id;
                }
                return ownerId; 
            }
            set
            {
                ownerId = value;
                this.RaisePropertyChanged("OwnerId");
            }
        }

        private bool isInEditMode;

        public bool IsInEditMode
        {
            get
            {
                return isInEditMode;
            }
            set
            {
                isInEditMode = value;
                this.RaisePropertyChanged("IsInEditMode");
                if (this.ParentGroup != null)
                {
                    this.ParentGroup.RaisePropertyChanged("IsInEditMode");
                }
            }
        }

        public Account():base()
        {
        }

        public void AddRiskSetting(EZXLib.Properties properties)
        {
            if (properties == null)
            {
                properties = new EZXLib.Properties();
            }
            if (properties.PropertyMap == null)
            {
                properties.PropertyMap = new EZXLib.TagValueMsg();
            }
            this.RiskSetting = new RiskSetting(properties.PropertyMap.tagValues);
        }
    }
}
