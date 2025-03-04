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
using System.Windows.Shapes;
using EZXLightspeedGUI.ViewModel;
using EZX.LightSpeedEngine;
using EZX.LightspeedEngine.Entity;
using EZXLib;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            this.groupSettingUserControl.DataContext = null;
            App.AppManager.GUILSEngine.Disconnected += new DisconnectHandler(GUILSEngine_Disconnected);
            App.AppManager.GUILSEngine.Connected += new ConnectHandler(GUILSEngine_Connected);
            App.AppManager.GUILSEngine.LoadAllGroupAndAccountCompleted += new LoadAllGroupAndAccountHandler(GUILSEngine_LoadAllGroupAndAccountCompleted);
            App.AppManager.GUILSEngine.LoginCompleted += new LoginCompleteHandler(GUILSEngine_LoginCompleted);
            App.AppManager.GUILSEngine.LoggedOut += new DisconnectHandler(GUILSEngine_LoggedOut);
            App.AppManager.ErrorOccurred += new ErrorOccurredHandler(AppManager_ErrorOccurred);
            App.AppManager.GUILSEngine.MoveAccountCompleted += new EZX.LightSpeedEngine.MoveAccountHandler(GUILSEngine_MoveAccountCompleted);

            AddHandler(Keyboard.KeyDownEvent, (KeyEventHandler)HandleKeyDownEvent);

            Group defaultGroup = App.AppManager.GUILSEngine.DataManager.GetDefaultGroupNode();
            if (defaultGroup != null)
            {
                LoadGroup(defaultGroup, false, null);
            }
        }


        private void HandleKeyDownEvent(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.U && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                MessageBox.Show("CTRL + SHIFT + U\nMode has been changed to: Account Settings ON", "Account Risk Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                App.AppManager.GUILSEngine.IsAccountSettingON = true;
                e.Handled = true;
                App.AppManager.GUILSEngine.GetGroupAccountData();
            }

            if (e.Key == Key.G && (Keyboard.Modifiers & (ModifierKeys.Control | ModifierKeys.Shift)) == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                MessageBox.Show("CTRL + SHIFT + G\nMode has been changed to: Account Settings OFF", "Account Risk Settings", MessageBoxButton.OK, MessageBoxImage.Information);
                App.AppManager.GUILSEngine.IsAccountSettingON = false;
                e.Handled = true;
                App.AppManager.GUILSEngine.GetGroupAccountData();
            }
        }


        void AppManager_ErrorOccurred(object sender, LightspeedExceptionEventAgrs e)
        {
            App.AppManager.RunOnDispatcherThread(() =>
            {
                if (e.ExceptionType == LIGHTSPEED_EXCEPTION_TYPE.DATA_VALIDATION)
                {
                    MessageBox.Show(e.ExceptionMessage, "Data Validation Error!", MessageBoxButton.OK, MessageBoxImage.Stop);
                }
                else if ((e.ExceptionType == LIGHTSPEED_EXCEPTION_TYPE.GUI_KNOWN))
                {
                    MessageBox.Show(e.ExceptionMessage, "Error occurred!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    ExceptionView view = new ExceptionView(e.Exception);
                    view.ShowDialog();
                    //MessageBox.Show(e.ExceptionMessage, "Error Occurred!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        
        void GUILSEngine_LoggedOut(object sender, LSConnectionEventArgs e)
        {
            App.AppManager.RunOnDispatcherThread(() =>
            {
                DisableControl();
                LoginView view = LoginView.GetLoginView();
                view.ShowDialog();
                view.BringIntoView();
            });
            
        }

        void GUILSEngine_Connected(object sender, LSConnectionEventArgs e)
        {
            App.AppManager.RunOnDispatcherThread(() =>
            {
                EnableControl();
            });
        }

        void GUILSEngine_LoginCompleted(object sender, LSConnectionEventArgs e)
        {
            //App.AppManager.RunOnDispatcherThread(() =>
            //{
            //    EnableControl();
            //});
        }

        void GUILSEngine_MoveAccountCompleted(object sender, EZX.LightSpeedEngine.GroupAccountEventArgs e)
        {
            //App.AppManager.RunOnDispatcherThread(() =>
            //{
            //    INodeEntity node = App.AppManager.GUILSEngine.DataManager.FindNodeEntity(e.GroupAccount.Id);
            //    GroupSettingVM groupSettingVM = (this.groupSettingUserControl.DataContext as GroupSettingVM);

            //    if (groupSettingVM.SelectedAccount != null && groupSettingVM.SelectedAccount.Id.Equals(node.Id))
            //    {
            //        if (node is Group)
            //        {
            //            (this.groupSettingUserControl.DataContext as GroupSettingVM).SelectedGroup = (node as Group);
            //        }
            //        else if (node is Account)
            //        {
            //            (this.groupSettingUserControl.DataContext as GroupSettingVM).SelectedAccount = (node as Account);
            //            (this.groupSettingUserControl.DataContext as GroupSettingVM).SelectedGroup = (this.groupSettingUserControl.DataContext as GroupSettingVM).SelectedAccount.ParentGroup;
            //        }
            //        (this.groupSettingUserControl.DataContext as GroupSettingVM).SelectedRiskSetting = (this.groupSettingUserControl.DataContext as GroupSettingVM).SelectedAccount.RiskSetting.CloneRiskSetting();
            //    }
            //}
            //);
        }

        void GUILSEngine_DeleteGroupAccountCompleted(object sender, GroupAccountEventArgs e)
        {
            App.AppManager.RunOnDispatcherThread(() =>
            {
                this.groupSettingUserControl.DataContext = null;
            });
        }


        private void EnableControl()
        {
            this.groupAccountList.IsEnabled = true;
            this.groupRiskSetting.IsEnabled = true;
        }

        void GUILSEngine_Disconnected(object sender, LSConnectionEventArgs e)
        {
            App.AppManager.RunOnDispatcherThread(() =>
            {
                DisableControl();
            });
        }

        private void DisableControl()
        {
            this.groupAccountList.IsEnabled = false;
            this.groupRiskSetting.IsEnabled = false;
        }

        void GUILSEngine_LoadAllGroupAndAccountCompleted(object sender, EZX.LightSpeedEngine.LSConnectionEventArgs e)
        {
            Group defaultGroup = App.AppManager.GUILSEngine.DataManager.GetDefaultGroupNode();
            defaultGroup = null;
            if (defaultGroup != null)
            {
                App.AppManager.RunOnDispatcherThread(() => LoadGroup(defaultGroup, false, null));
            }
        }

        public bool LoadGroup(Group selectedGroup, bool isAddingNewGroup, Account selectedAccount)
        {
            GroupSettingVM oldGroupSettingVM = this.groupSettingUserControl.DataContext as GroupSettingVM;

            if (oldGroupSettingVM != null && oldGroupSettingVM.SelectedGroup.Id.Equals(selectedGroup.Id))
            {
                return false;
            }

            GroupSettingVM groupSettingVM = LoadGroupSettingVM(selectedGroup, isAddingNewGroup, selectedAccount);
            this.groupSettingUserControl.DataContext = groupSettingVM;
            return true;
        }

        public GroupSettingVM SelectedGroupSettingVM()
        {
            GroupSettingVM vm = this.groupSettingUserControl.DataContext as GroupSettingVM;
            return vm;
        }

        public bool IsRiskSettingModified()
        {
            GroupSettingVM vm = this.groupSettingUserControl.DataContext as GroupSettingVM;
            if (vm == null)
            {
                return false;
            }
            bool matched = true;

            if (vm.SelectedAccount != null && App.AppManager.GUILSEngine.IsAccountSettingON)
            {
                matched = vm.SelectedRiskSetting.CompareSetting(vm.SelectedAccount.RiskSetting);
            }
            else
            {
                matched = vm.SelectedRiskSetting.CompareSetting(vm.SelectedGroup.RiskSetting);
            }

            if (vm.IsAddingNewGroup)
            {
                matched = false;
            }

            if (!matched)
            {
                return true;
            }
            return false;
        }


        private GroupSettingVM LoadGroupSettingVM(Group selectedGroup, bool isAddingNewGroup, Account selectedAccount)
        {
            GroupSettingVM groupSettingVM = new GroupSettingVM();
            groupSettingVM.SelectedGroup = selectedGroup;
            if (isAddingNewGroup)
            {
                if (App.AppManager.GUILSEngine.DataManager.GetDefaultGroupNode() != null)
                {
                    groupSettingVM.SelectedRiskSetting = App.AppManager.GUILSEngine.DataManager.GetDefaultGroupNode().RiskSetting.CloneRiskSetting();
                }
                else
                {
                    throw new LightspeedException("Exception occured as the [Default] group is not exists. Application could not work smoothly without [Default] group");
                }
            }
            else
            {
                if (selectedAccount != null && App.AppManager.GUILSEngine.IsAccountSettingON)
                {
                    groupSettingVM.SelectedRiskSetting = selectedAccount.RiskSetting.CloneRiskSetting();
                }
                else
                {
                    groupSettingVM.SelectedRiskSetting = selectedGroup.RiskSetting.CloneRiskSetting();
                }
            }
            groupSettingVM.IsAddingNewGroup = isAddingNewGroup;
            groupSettingVM.GroupName = groupSettingVM.SelectedGroup.Name;
            groupSettingVM.SelectedAccount = selectedAccount;
            return groupSettingVM;
        }


        public void SaveRiskSetting()
        {
            this.groupSettingUserControl.SaveRiskSetting();
        }

        //private void Window_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Apps)
        //    {
        //        App.AppManager.KeyPressedToOpenContextMenu();
        //        e.Handled = true;
        //    }
        //}
    }
}
