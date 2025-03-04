using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EZXLightspeedGUI.ViewModel;
using EZXLib;
using EZX.LightspeedEngine.Entity;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for GroupUserControl.xaml
    /// </summary>
    public partial class GroupUserControl : UserControl
    {
        private GroupSettingVM VM
        {
            get
            {
                return this.DataContext as GroupSettingVM;
            }
        }

        public GroupUserControl()
        {
            InitializeComponent();
            App.AppManager.AddingGroupAccount += new AddingGroupAccountHandler(AppManager_AddingGroupAccount);
        }


        void AppManager_AddingGroupAccount(object sender, EventArgs e)
        {
            this.grpAccName.Focus();
            this.grpAccName.SelectAll();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveRiskSetting();
        }

        public void SaveRiskSetting()
        {
            if (!this.VM.IsAddingNewGroup && this.VM.SelectedRiskSetting.CompareSetting(this.VM.SelectedGroup.RiskSetting))
            {
                // avoid sending muliple save when values are not modified (issue 6387)
                return;
            }

            EditChangeReason(this.VM.SelectedGroup.Name);
            this.VM.SelectedGroup.RiskSetting = this.VM.SelectedRiskSetting.CloneRiskSetting();
            if (this.VM.IsAddingNewGroup)
            {
                if (this.VM.SelectedGroup.IsAccountGroup)
                {
                    this.VM.SelectedGroup.GroupAccount.Id = this.VM.SelectedGroup.GroupAccount.Name;
                }

                App.AppManager.GUILSEngine.DataManager.ValidateNewGroup(this.VM.SelectedGroup);

                this.VM.IsAddingNewGroup = false;

                this.VM.SelectedGroup.IsSelected = true;
                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Insert(0, this.VM.SelectedGroup);
                App.AppManager.GUILSEngine.DataManager.SaveGroup(this.VM.SelectedGroup);
                this.VM.SelectedGroup.IsInEditMode = false;
            }
            else
            {
                App.AppManager.GUILSEngine.DataManager.UpdateRiskSetting(this.VM.SelectedGroup);
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            CancelRiskSetting();
        }

        private void CancelRiskSetting()
        {
            if (this.VM.IsAddingNewGroup)
            {
                this.VM.SelectedGroup = App.AppManager.GUILSEngine.DataManager.SelectedGroupNode();
                this.VM.SelectedRiskSetting = this.VM.SelectedGroup.RiskSetting.CloneRiskSetting();
                this.VM.IsAddingNewGroup = false;
            }
            else
            {
                if (this.VM.SelectedAccount != null && App.AppManager.GUILSEngine.IsAccountSettingON)
                {
                    this.VM.SelectedRiskSetting = this.VM.SelectedAccount.RiskSetting.CloneRiskSetting();
                }
                else
                {
                    this.VM.SelectedRiskSetting = this.VM.SelectedGroup.RiskSetting.CloneRiskSetting();
                }

                this.VM.IsAddingNewGroup = false;
            }
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).SelectAll();
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {                
                this.VM.SelectedGroup.Name = (sender as TextBox).Text;
                this.VM.GroupName = this.VM.SelectedGroup.Name;
                SaveRiskSetting();
            }
            else if (e.Key == Key.Escape)
            {
                CancelRiskSetting();
            }
        }

        public static void EditChangeReason(string groupAccountName)
        {
            if (App.AppManager.Config.ShowChangeSettingPopup)
            {
                ChangeSetting changeSettingPopup = new ChangeSetting(groupAccountName);
                changeSettingPopup.ShowDialog();
            }
        }

    }
}
