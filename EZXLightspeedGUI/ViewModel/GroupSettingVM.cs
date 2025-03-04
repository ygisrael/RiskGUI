using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXWPFLibrary.Helpers;
using EZX.LightspeedEngine.Entity;

namespace EZXLightspeedGUI.ViewModel
{
    public class GroupSettingVM : ObservableBase
    {
        private string groupName;
        private RiskSetting slectedRiskSetting;
        private Group selectedGroup;
        private Account selectedAccount;
        private bool isAddingNewGroup;


        public string SaveButtonContent
        {
            get
            {
                if (this.SelectedGroup.IsAccountGroup)
                {
                    return "Save Account";
                }
                else
                {
                    return "Save Group";
                }
            }
        }

        public string Heading
        {
            get 
            {
                string txtEditMode = string.Empty;

                if (this.IsAddingNewGroup)
                {
                    if (this.SelectedGroup.IsAccountGroup)
                    {
                        return "Adding New Account*";
                    }
                    return "Adding New Group*";
                }
                else
                {
                    if (this.SelectedGroup.IsInEditMode)
                    {
                        txtEditMode = "*";
                    }
                    if (this.SelectedGroup.IsAccountGroup)
                    {
                        return "ACCOUNT: " + this.SelectedGroup.Name + txtEditMode;
                    } 
                    
                    return "GROUP: "+this.SelectedGroup.Name + txtEditMode;
                }
            }
        }

        public string GroupName
        {
            get { return groupName; }
            set 
            { 
                groupName = value;
                this.RaisePropertyChanged(p => p.GroupName);
            }
        }
        public RiskSetting SelectedRiskSetting
        {
            get { return slectedRiskSetting; }
            set
            {
                slectedRiskSetting = value;
                this.RaisePropertyChanged(p => p.SelectedRiskSetting);
            }
        }
        public Group SelectedGroup
        {
            get { return selectedGroup; }
            set 
            { 
                selectedGroup = value;
                this.RaisePropertyChanged(p => p.SelectedGroup);
                this.RaisePropertyChanged(p => p.Heading);
                this.RaisePropertyChanged(p => p.SaveButtonContent);
                
            }
        }
        public Account SelectedAccount
        {
            get { return selectedAccount; }
            set 
            { 
                selectedAccount = value;
                this.RaisePropertyChanged(p => p.SelectedAccount);
                this.RaisePropertyChanged(p => p.Heading);
            }
        }
        public bool IsAddingNewGroup
        {
            get { return isAddingNewGroup; }
            set 
            { 
                isAddingNewGroup = value;
                this.RaisePropertyChanged(p => p.IsAddingNewGroup);
                this.RaisePropertyChanged(p => p.Heading);
            }
        }


        public GroupSettingVM()
            : base()
        {
            this.SelectedGroup = new Group();
            this.SelectedRiskSetting = new RiskSetting();
            App.AppManager.GUILSEngine.UpdateGroupAccountSettingCompleted += new EZX.LightSpeedEngine.UpdateGroupAccountSettingHandler(GUILSEngine_UpdateGroupAccountSettingCompleted);
            App.AppManager.GUILSEngine.UpdateGroupNameCompleted += new EZX.LightSpeedEngine.UpdateGroupNameHandler(GUILSEngine_UpdateGroupNameCompleted);
        }

        void GUILSEngine_UpdateGroupNameCompleted(object sender, EZX.LightSpeedEngine.LSConnectionEventArgs e)
        {
            this.IsAddingNewGroup = false;
        }

        void GUILSEngine_UpdateGroupAccountSettingCompleted(object sender, EZX.LightSpeedEngine.GroupAccountEventArgs e)
        {
            if (this.SelectedAccount != null && App.AppManager.GUILSEngine.IsAccountSettingON)
            {
                this.SelectedRiskSetting = this.SelectedAccount.RiskSetting.CloneRiskSetting();
            }
            else
            {
                this.SelectedRiskSetting = this.SelectedGroup.RiskSetting.CloneRiskSetting();
            }
        }

        public void Init(RiskSetting riskSetting, Group group)
        {
            this.SelectedRiskSetting = riskSetting;
            this.SelectedGroup = group;
        }

        
        public void Init(RiskSetting riskSetting)
        {
            this.Init(riskSetting, new Group());
        }
    }
}
