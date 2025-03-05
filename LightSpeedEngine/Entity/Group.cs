using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using EZXWPFLibrary.Helpers;

namespace EZX.LightspeedEngine.Entity
{
    public partial class Group : EZXLightSpeedBaseEntity, INodeEntity
    {
        private string id;
        private string name;
        private int displayIndex;
        private bool isSelected;
        private bool isExpanded;
        private bool isWaitingForServerResponse;
        private RiskSetting riskSetting;
        private bool isDefaultGroup;
        private bool isAccountGroup;

        private MTObservableCollection<Account> accountList;
        public MTObservableCollection<Account> AccountList
        {
            get
            {
                if (this.IsAccountGroup)
                {
                    return null;
                }
                return accountList;

            }
            set
            {
                accountList = value;
                this.RaisePropertyChanged("AccountList");
            }
        }

        public string Id
        {
            get 
            {
                if (this.IsAccountGroup)
                {
                    if (this.accountList != null && this.accountList.Count > 0)
                    {
                        return this.accountList[0].Id;
                    }
                }
                return id;
            }
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
                if (this.IsAccountGroup)
                {
                    if (this.accountList != null && this.accountList.Count > 0)
                    {
                        return this.accountList[0].Name;
                    }
                }
                return name; 
            }
            set
            {
                if (this.IsAccountGroup)
                {
                    if (this.accountList != null && this.accountList.Count > 0)
                    {
                        this.accountList[0].Name = value;
                    }
                }
                else
                {
                    name = value;
                }
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
            get 
            {
                if (IsAccountGroup)
                {
                    if (this.accountList != null && this.accountList.Count > 0)
                    {
                        return this.accountList[0].IsWaitingForServerResponse;
                    }
                }
                return isWaitingForServerResponse; 
            }
            set 
            { 
                isWaitingForServerResponse = value;
                this.RaisePropertyChanged(p => p.IsWaitingForServerResponse);
                this.RaisePropertyChanged(p => p.Name);
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

        public Account GroupAccount
        {
            get 
            {
                if (this.IsAccountGroup && this.accountList.Count > 0)
                {
                    return this.accountList[0];
                }
                return null; 
            }
        }

        public static string newId()
        {
            return Guid.NewGuid().ToString();
        }

        private bool isInEditMode;

        public bool IsInEditMode
        {
            get 
            {
                if (IsAccountGroup)
                {
                    if (this.accountList != null && this.accountList.Count > 0)
                    {
                        return this.accountList[0].IsInEditMode;
                    }
                }
                return isInEditMode; 
            }
            set 
            { 
                isInEditMode = value;
                this.RaisePropertyChanged(p => p.IsInEditMode);
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

        public bool IsDefaultGroup
        {
            get { return isDefaultGroup; }
            set
            {
                isDefaultGroup = value;
                this.RaisePropertyChanged("IsDefaultGroup");
            }
        }

        public bool IsAccountGroup
        {
            get { return isAccountGroup; }
            set
            {
                isAccountGroup = value;
                this.RaisePropertyChanged("IsAccountGroup");
                this.RaisePropertyChanged("Name");
                this.RaisePropertyChanged("GroupAccount");
                this.RaisePropertyChanged("AccountList");
            }
        }

        public Group()
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
