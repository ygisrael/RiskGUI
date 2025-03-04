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
using EZX.LightspeedEngine.Entity;
using System.Collections;
using EZXLib;
using EZXWPFLibrary.CustomControls;
using EZX.LightSpeedEngine;
using EZXLightspeedGUI.ViewModel;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for AccountUserControl.xaml
    /// </summary>
    public partial class AccountUserControl : UserControl
    {
        public AccountUserControl()
        {
            Logger.DEBUG("AccountUserControl()");
            InitializeComponent();
            //App.AppManager.ContextMenuOpeningThrougKey += new ContextMenuKeyHandler(AppManager_ContextMenuOpeningThrougKey);

        }

        //void AppManager_ContextMenuOpeningThrougKey(object sender, EventArgs e)
        //{

        //    TreeViewItem tvi = this.treeView.ItemContainerGenerator.ContainerFromItem(this.treeView.SelectedItem) as TreeViewItem;
        //    if (tvi != null)
        //    {
        //        StackPanel stkPnl = FindChild<StackPanel>(tvi, "stkPnlTreeViewItemTextBlock");
        //        if (stkPnl != null)
        //        {
        //            stkPnl.ContextMenu.StaysOpen = true; 
        //            stkPnl.ContextMenu.IsOpen = true;
        //        }
        //    }
        //}

        void TreeViewItem_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            Logger.DEBUG("TreeViewItem_RequestBringIntoView");
            e.Handled = true; //This will stop automatic scroll of treeview on selecting node in it.
        }


        Group groupTo;
        Group groupFrom;


        private void treeViewItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isContextMenuOpened)
            {
                return;
            }

            if ((sender is TreeViewItem) && ((sender as TreeViewItem).DataContext is INodeEntity))
            {
                if (((sender as TreeViewItem).DataContext as INodeEntity).IsInEditMode)
                {
                    return;
                }

                if (((sender as TreeViewItem).DataContext as INodeEntity) is Group)
                {
                    Group grp = (((sender as TreeViewItem).DataContext as INodeEntity) as Group);
                    if (grp.IsAccountGroup)
                    {
                        if (grp.IsInEditMode || grp.GroupAccount.IsInEditMode)
                        {
                            return;
                        }
                    }
                    else
                    {
                        foreach (Account acc in grp.AccountList)
                        {
                            if (acc.IsInEditMode)
                            {
                                return;
                            }
                        }
                    }
                }
            }

            if ((sender is INodeEntity) && (sender as INodeEntity).IsInEditMode)
            {
                return;
            }



            if (e.LeftButton == MouseButtonState.Pressed &&
                treeView.SelectedItem != null)
            {
                Logger.DEBUG("MouseButtonState.Pressed ");
                DataObject data = new DataObject();

                if (treeView.SelectedItem is Account)
                {
                    data.SetData("Object", treeView.SelectedItem as Account);
                }
                else if (treeView.SelectedItem is Group)
                {
                    if ((App.AppManager.GUILSEngine.DataManager.IsAlphabetize) && !((treeView.SelectedItem as Group).IsAccountGroup))
                    {
                        return;
                    }
                    data.SetData("Object", treeView.SelectedItem as Group);
                }
                App.AppManager.IsNodeStartDraging = true;
                DragDrop.DoDragDrop(treeView, data, DragDropEffects.Move);
                this.IsItemMoved = false;
                Logger.DEBUG("treeViewItem_Move(..)");
            }
        }

        private void treeViewItem_DragEnter(object sender, DragEventArgs e)
        {
            Logger.DEBUG("treeViewItem_DragEnter(..)");
            if (this.isContextMenuOpened)
            {
                return;
            }

            if ((e.OriginalSource is TextBlock) && (e.OriginalSource as TextBlock).Parent is StackPanel)
            {
                StackPanel sktPnl = (e.OriginalSource as TextBlock).Parent as StackPanel;
                sktPnl.Children[0].Visibility = System.Windows.Visibility.Visible;
                sktPnl.Children[1].Visibility = System.Windows.Visibility.Visible;
            }

            TreeViewItem treeViewItem = sender as TreeViewItem;
            Group selectedGroup = treeViewItem.DataContext as Group;
            Account selectedAccount = treeViewItem.DataContext as Account;

            if (selectedGroup != null)
            {
                savedBackground = treeViewItem.Background;
                treeViewItem.Background = Brushes.Transparent;
            }
            else if (selectedAccount != null)
            {
                savedBackground = treeViewItem.Background;
                treeViewItem.Background = Brushes.Transparent;
            }
        }

        private void treeViewItem_DragLeave(object sender, DragEventArgs e)
        {
            Logger.DEBUG("treeViewItem_DragLeave(..)");
            if (this.isContextMenuOpened)
            {
                return;
            }
            ((TreeViewItem)sender).Background = savedBackground;
        }

        private void treeViewItem_Drop(object sender, DragEventArgs e)
        {
            Logger.DEBUG("treeViewItem_Drop(..)");
            this.IsItemMoved = true;
            ResetUpDownIconVisibility(e.OriginalSource);
            if (this.isContextMenuOpened)
            {
                return;
            }

            TreeViewItem treeViewItem = sender as TreeViewItem;
            Group groupTo = treeViewItem.DataContext as Group;
            Account accountTo = treeViewItem.DataContext as Account;
            
            if (groupTo != null || accountTo != null)
            {
                if (groupTo != null && groupTo.IsDefaultGroup && this.node_drop_to == NODE_DROP_LOCATION.DEFAULT)
                {
                    //Not allowed to move into DefaultGroup
                    
                    treeViewItem.Background = savedBackground;
                    this.node_drop_to = NODE_DROP_LOCATION.DEFAULT;
                    return;
                }
                if (e.Data.GetData("Object") is Account)
                {
                    Account accountToMove = e.Data.GetData("Object") as Account;

                    if (accountToMove != null)
                    {
                        if (groupTo != null)
                        {
                            if (!groupTo.IsAccountGroup)
                            {
                                if (groupTo.Id == accountToMove.ParentGroup.Id && App.AppManager.GUILSEngine.DataManager.IsAlphabetize)
                                {
                                    this.node_drop_to = NODE_DROP_LOCATION.DEFAULT;
                                    return;
                                }
                                
                                accountToMove.ParentGroup.AccountList.Remove(accountToMove);
                                accountToMove.ParentGroup.RaisePropertyChanged("AccountList");
                                if (this.node_drop_to == NODE_DROP_LOCATION.DEFAULT)
                                {
                                    accountToMove.ParentGroup = groupTo;
                                    MoveAccount(accountToMove, accountToMove.ParentGroup.Id);
                                    groupTo.AccountList.Add(accountToMove);
                                }
                                else
                                {
                                    int oldPosition = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupTo);
                                    int newPosition = 0;
                                    if (this.node_drop_to == NODE_DROP_LOCATION.UP)
                                    {
                                        newPosition = oldPosition;
                                    }
                                    Group newAccountGroup = CreateNewAccountGroup(accountToMove);
                                    MoveAccount(accountToMove, accountToMove.Id);
                                    App.AppManager.GUILSEngine.DataManager.AccountGroupList.Insert(newPosition, newAccountGroup);
                                }                                
                            }
                            else
                            {
                                //if (App.AppManager.GUILSEngine.DataManager.IsAlphabetize)
                                //{
                                //    this.node_drop_to = NODE_DROP_LOCATION.DEFAULT;
                                //    return;
                                //}

                                Group newAccountGroup = CreateNewAccountGroup(accountToMove);
                                MoveAccount(accountToMove, accountToMove.Id);
                                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(newAccountGroup);
                            }
                            treeViewItem.Background = savedBackground;
                        }
                        else if (accountTo != null)
                        {
                            if (accountTo.ParentGroup == accountToMove.ParentGroup)
                            {
                                if (App.AppManager.GUILSEngine.DataManager.IsAlphabetize)
                                {
                                    this.node_drop_to = NODE_DROP_LOCATION.DEFAULT;
                                    return;
                                } 
                                
                                int oldPosition = accountToMove.ParentGroup.AccountList.IndexOf(accountToMove);
                                int newPosition = accountToMove.ParentGroup.AccountList.IndexOf(accountTo);
                                accountToMove.ParentGroup.AccountList.Move(oldPosition, newPosition);

                                accountToMove.DisplayIndex = accountToMove.ParentGroup.AccountList.IndexOf(accountToMove);
                                accountTo.DisplayIndex = accountToMove.ParentGroup.AccountList.IndexOf(accountTo);
                                e.Handled = true;
                            }
                        }
                    }
                }
                else if (e.Data.GetData("Object") is Group)
                {
                    Group groupToMove = e.Data.GetData("Object") as Group;
                    //Only Account-Group (The account with no group is allowed to move inside other group)
                    if (groupToMove != null && groupToMove.IsAccountGroup && groupToMove.GroupAccount != null)
                    {                       
                        Account accountToMove = groupToMove.GroupAccount;
                        if (groupTo != null && !groupTo.IsAccountGroup)
                        {
                            if (this.node_drop_to == NODE_DROP_LOCATION.DEFAULT)
                            {
                                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Remove(groupToMove);
                                groupToMove.GroupAccount.IsOwnGroup = false;
                                groupToMove.GroupAccount.ParentGroup = groupTo;
                                groupTo.AccountList.Add(groupToMove.GroupAccount);
                                MoveAccount(accountToMove, groupTo.Id);
                            }
                            else
                            {
                                int groupOldPosition = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupToMove);
                                int groupNewPosition = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupTo);
                                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Move(groupOldPosition, groupNewPosition);

                                groupToMove.DisplayIndex = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupToMove);
                                groupTo.DisplayIndex = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupTo);
                            }
                        }
                        else if (groupTo != null && groupTo.IsAccountGroup)
                        {
                            if (App.AppManager.GUILSEngine.DataManager.IsAlphabetize)
                            {
                                this.node_drop_to = NODE_DROP_LOCATION.DEFAULT;
                                return;
                            }

                            int groupOldPosition = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupToMove);
                            int groupNewPosition = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupTo);

                            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Move(groupOldPosition, groupNewPosition);

                            groupToMove.DisplayIndex = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupToMove);
                            groupTo.DisplayIndex = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupTo);
                        }
                        treeViewItem.Background = savedBackground;
                    }
                    else
                    {
                        if (groupToMove != null)
                        {
                            if (App.AppManager.GUILSEngine.DataManager.IsAlphabetize)
                            {
                                this.node_drop_to = NODE_DROP_LOCATION.DEFAULT;
                                return;
                            }

                            int groupOldPosition = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupToMove);
                            int groupNewPosition = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupTo);
                            if (groupOldPosition < groupNewPosition)
                            {
                                groupNewPosition = groupNewPosition - 1;
                            }

                            if (groupOldPosition >= 0 && groupNewPosition >= 0)
                            {
                                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Move(groupOldPosition, groupNewPosition);
                                //Modify DisplayIndex
                                groupToMove.DisplayIndex = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupToMove);
                                groupTo.DisplayIndex = App.AppManager.GUILSEngine.DataManager.AccountGroupList.IndexOf(groupTo);
                            }
                        }
                    }
                }
            }
            this.node_drop_to = NODE_DROP_LOCATION.DEFAULT;
        }

        private void ResetUpDownIconVisibility(object sourceObj)
        {
            Logger.DEBUG("ResetUpDownIconVisibility(..)");
            if ((sourceObj is TextBlock) && (sourceObj as TextBlock).Parent is StackPanel)
            {
                if (((sourceObj as TextBlock).Parent as StackPanel).Parent is StackPanel)
                {
                    Logger.DEBUG("(sourceObj is Border) 1");
                    StackPanel sktPnl = (((sourceObj as TextBlock).Parent as StackPanel).Parent as StackPanel);
                    sktPnl.Children[0].Visibility = System.Windows.Visibility.Collapsed;
                    sktPnl.Background = Brushes.Transparent;
                }
            }
            else if ((sourceObj is Border) && ((sourceObj as Border).Parent is StackPanel))
            {
                Logger.DEBUG("(sourceObj is Border) 2");
                StackPanel sktPnl = ((sourceObj as Border).Parent  as StackPanel);
                sktPnl.Children[0].Visibility = System.Windows.Visibility.Collapsed;
            }
            else if ((sourceObj is Image) && ((sourceObj as Image).Parent is Border)
               && (sourceObj as Image).Tag.Equals("ImageIcon"))
            {
                Logger.DEBUG("(sourceObj is Image) 3");
                if (((sourceObj as Image).Parent as Border).Parent is StackPanel)
                {
                    StackPanel sktPnl = (((sourceObj as Image).Parent as Border).Parent as StackPanel);
                    sktPnl.Children[0].Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else if ((sourceObj is Image) && ((sourceObj as Image).Parent is Border)
                && (sourceObj as Image).Tag.Equals("UpIcon"))
            {
                Logger.DEBUG("(sourceObj is Image) 4");
                Border brdr = ((sourceObj as Image).Parent as Border);
                brdr.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if ((sourceObj is StackPanel) && ((sourceObj as StackPanel).Parent is StackPanel))
            {
                Logger.DEBUG("(sourceObj is StackPanel) 5");
                StackPanel sktPnl = ((sourceObj as StackPanel).Parent as StackPanel);
                sktPnl.Children[0].Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (((sourceObj is StackPanel) && ((sourceObj as StackPanel).Parent == null)
                && ((sourceObj as StackPanel).Children[0] is Border)))
            {
                Logger.DEBUG("(sourceObj is top StackPanel) 6");
                Border brdr = ((sourceObj as StackPanel).Children[0] as Border);
                brdr.Visibility = System.Windows.Visibility.Collapsed;
            }
            else 
            {
                Logger.WARN("(sourceObj is unknown) 7:"+ sourceObj.GetType().Name);
            }
        }

        private static Group CreateNewAccountGroup(Account accountToMove)
        {
            Logger.DEBUG("CreateNewAccountGroup(..)");
            Group newAccountGroup = new Group()
            {
                Name = accountToMove.Name,
                IsAccountGroup = false,
                IsDefaultGroup = false,
                RiskSetting = accountToMove.ParentGroup.RiskSetting.CloneRiskSetting(),
                DisplayIndex = 100,
                Id = accountToMove.Id,
                IsInEditMode = true,
                AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>()
            };
            accountToMove.ParentGroup.AccountList.Remove(accountToMove);
            newAccountGroup.AccountList.Add(accountToMove);
            newAccountGroup.IsAccountGroup = true;
            accountToMove.ParentGroup = newAccountGroup;
            accountToMove.IsOwnGroup = true;
            return newAccountGroup;
        }

        private void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("ContextMenu_Closed(..)");
            this.isContextMenuOpened = false;
        }

        public Brush savedBackground { get; set; }

        bool isItemMoved = true;

        public bool IsItemMoved
        {
            get { return isItemMoved; }
            set 
            {
                if (isItemMoved != value)
                {
                    App.AppManager.IsNodeStartDraging = !isItemMoved;
                }
                isItemMoved = value;
            }
        }


        private void treeView_Drop(object sender, DragEventArgs e)
        {
            Logger.DEBUG("treeView_Drop(..)");
            ResetUpDownIconVisibility(e.OriginalSource);
            if (this.isContextMenuOpened)
            {
                return;
            }

            if (!this.IsItemMoved)
            {
                this.IsItemMoved = true;
                if (sender is TreeView)
                {
                    if ((sender as TreeView).SelectedItem is Account)
                    {
                        if (e.Data.GetData("Object") is Account)
                        {
                            Account accountToMove = e.Data.GetData("Object") as Account;

                            if (accountToMove != null)
                            {
                                Group newAccountGroup = CreateNewAccountGroup(accountToMove);
                                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(newAccountGroup);
                                MoveAccount(accountToMove, accountToMove.ParentGroup.Id);
                            }
                        }
                    }
                    else if ((sender as TreeView).SelectedItem is Group)
                    {
                        if (e.Data.GetData("Object") is Group)
                        {
                            Group groupToMove = e.Data.GetData("Object") as Group;

                            if (groupToMove.IsAccountGroup && App.AppManager.GUILSEngine.DataManager.IsAlphabetize)
                            {
                                this.node_drop_to = NODE_DROP_LOCATION.DEFAULT;
                                return;
                            }

                            if (groupToMove != null)
                            {
                                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Remove(groupToMove);
                                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(groupToMove);
                            }
                        }
                    }
                }
            }
            this.node_drop_to = NODE_DROP_LOCATION.DEFAULT;
        }

        private bool LoadRiskControl(object sender)
        {
            Logger.DEBUG("LoadRiskControl(..)");
            if ((this.Parent is Grid))
            {
                Group selectedGroup = null;
                Account selectedAccount = null;

                if (sender is MenuItem)
                {
                    selectedGroup = (sender as MenuItem).DataContext as Group;
                    selectedAccount = (sender as MenuItem).DataContext as Account;
                }
                else if (sender is TreeView)
                {
                    selectedGroup = (sender as TreeView).SelectedItem as Group;
                    selectedAccount = (sender as TreeView).SelectedItem as Account;
                }
                else
                {
                    throw new Exception("Invalid sender type: " + sender.GetType().Name);
                }

                if (selectedAccount == null && selectedGroup == null)
                {
                    return true;
                }

                if ((this.Parent as Grid).Parent is MainView)
                {
                    MainView mainView = (this.Parent as Grid).Parent as MainView;

                    if (selectedGroup == null)
                    {
                        selectedGroup = selectedAccount.ParentGroup;
                        if (selectedGroup == null)
                        {
                            throw new LightspeedException("Failed to get selectedGroup as selectedAccount.ParentGroup == null", LIGHTSPEED_EXCEPTION_TYPE.GUI_KNOWN);
                        }
                    }

                    if (selectedAccount != null && selectedAccount.IsInEditMode)
                    {
                        return true;
                    }
                    
                    mainView.LoadGroup(selectedGroup, false, selectedAccount);
                }
            }
            else
            {
                throw new Exception("Failed to evaluate condition [(this.Parent is Grid) && (sender as MenuItem).DataContext is Group)]");
            }
            return true;
        }

        private void CreateAccountMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("CreateAccountMenuItem_Click(..)");
            if (CheckModifiedRiskSetting() == false)
            {
                return;
            }

            Group group = (sender as MenuItem).DataContext as Group;
            if (group != null)
            {
                Account newAccount = new Account()
                {
                    Name = " New Account",
                    IsInEditMode = true,
                    IsOwnGroup = false,
                    ParentGroup = group
                };

                group.AccountList.Insert(0, newAccount);
                group.IsExpanded = true;

                newAccount.IsSelected = true;
            }
        }

        private void DeleteGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("DeleteGroupMenuItem_Click(..)");
            if (CheckModifiedRiskSetting() == false)
            {
                return;
            }

            Group group = (sender as MenuItem).DataContext as Group;
            if (group != null && !group.IsAccountGroup)
            {
                MessageBoxResult result = MessageBoxResult.Yes;
                if (!ApplicationManager.MOCK_MODE)
                {
                    result = MessageBox.Show("Are you sure to delete Group: " + group.Name + "?", "Delete Group", MessageBoxButton.YesNo);
                }
                if (result == MessageBoxResult.Yes)
                {
                    ////Move Account from deleting group
                    ////Since server is sending move-account response when sending delete-group request, therfore comment the following code(to avoid moving at client)
                    //if (group.AccountList != null)
                    //{
                    //    int i = 1;
                    //    foreach (Account account in group.AccountList)
                    //    {
                    //        Group accountGroup = new Group()
                    //        {
                    //            Name = account.Name,
                    //            Id = Group.newId(),
                    //            AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(),
                    //            IsDefaultGroup = false,
                    //            RiskSetting = group.RiskSetting.CloneRiskSetting(),
                    //        };
                    //        account.ParentGroup = accountGroup;
                    //        accountGroup.AccountList.Add(account);
                    //        accountGroup.IsAccountGroup = true;
                    //        App.AppManager.GUILSEngine.DataManager.AccountGroupList.Insert(i, accountGroup);
                    //        i++;
                    //    }

                    //    App.AppManager.GUILSEngine.DataManager.MoveDeletingGroupAccountAsStandAlone(group.Id);   
                    //}
                    GroupUserControl.EditChangeReason(group.Name);
                    App.AppManager.GUILSEngine.DataManager.RemoveGroupNode(group);
                    App.AppManager.GUILSEngine.DataManager.AccountGroupList.Remove(group);
                }
            }
        }

        private void DeleteAccountMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("DeleteAccountMenuItem_Click(..)");
            if (CheckModifiedRiskSetting() == false)
            {
                return;
            }

            Account account = (sender as MenuItem).DataContext as Account;
            Group accountGroup = (sender as MenuItem).DataContext as Group;
            if (account != null)
            {
                MessageBoxResult result = MessageBoxResult.Yes;
                if (!ApplicationManager.MOCK_MODE)
                {
                    result = MessageBox.Show("Are you sure to delete Account: " + account.Name + "?", "Delete Account", MessageBoxButton.YesNo);
                }

                if (result == MessageBoxResult.Yes)
                {
                    if (!account.IsOwnGroup)
                    {
                        GroupUserControl.EditChangeReason(account.Name);
                        App.AppManager.GUILSEngine.DataManager.RemoveAccountNode(account);
                        account.ParentGroup.AccountList.Remove(account);
                    }
                }
            }
            else if (accountGroup != null)
            {
                if (accountGroup.IsAccountGroup)
                {
                    MessageBoxResult result = MessageBoxResult.Yes;
                    if (!ApplicationManager.MOCK_MODE)
                    {
                        result = MessageBox.Show("Are you sure to delete Account: " + accountGroup.Name + "?", "Delete Account", MessageBoxButton.YesNo);
                    }
                    if (result == MessageBoxResult.Yes)
                    {
                        GroupUserControl.EditChangeReason(accountGroup.Name);
                        App.AppManager.GUILSEngine.DataManager.RemoveAccountNode(accountGroup.GroupAccount);
                        App.AppManager.GUILSEngine.DataManager.AccountGroupList.Remove(accountGroup);
                    }
                }
            }


        }

        private void ContextMenu_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Logger.DEBUG("DeleteAccountMenuItem_Click(..)");
            e.Handled = true;
        }


        private void CreateAccountGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("CreateAccountGroupMenuItem_Click(..)");
            if (CheckModifiedRiskSetting() == false)
            {
                return;
            }

            Group newGroup = App.AppManager.GUILSEngine.DataManager.CreateNewStandAloneAccountNode("New Account");
            if ((this.Parent is Grid))
            {
                if ((this.Parent as Grid).Parent is MainView)
                {
                    MainView mainView = (this.Parent as Grid).Parent as MainView;
                    Group selectedGroup = newGroup;
                    mainView.LoadGroup(selectedGroup, true, null);
                    App.AppManager.AddingNewGroupAccount();
                }
            }
            else
            {
                throw new Exception("Failed to evaluate condition (this.Parent is Grid)");
            }
        }

        private void CreateGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("CreateGroupMenuItem_Click(..)");

            if (CheckModifiedRiskSetting() == false)
            {
                return;
            }

            Group newGroup = App.AppManager.GUILSEngine.DataManager.CreateNewGroupNode();
            if ((this.Parent is Grid))
            {
                if ((this.Parent as Grid).Parent is MainView)
                {
                    MainView mainView = (this.Parent as Grid).Parent as MainView;
                    Group selectedGroup = newGroup;
                    mainView.LoadGroup(selectedGroup, true, null);
                    App.AppManager.AddingNewGroupAccount();
                }
            }
            else
            {
                throw new Exception("Failed to evaluate condition (this.Parent is Grid)");
            }

        }

        private void SaveGroupAndAccountButton_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("SaveGroupAndAccountButton_Click(..)");
            Button btnSaveGroup = sender as Button;
            if (btnSaveGroup != null)
            {
                Group groupNode = btnSaveGroup.DataContext as Group;
                Account accountNode = btnSaveGroup.DataContext as Account;
                if (groupNode != null)
                {
                    if (groupNode.IsAccountGroup)
                    {
                        groupNode.GroupAccount.Id = groupNode.GroupAccount.Name;
                        groupNode.Name = groupNode.GroupAccount.Name;
                        groupNode.Id = groupNode.GroupAccount.Id;
                        groupNode.GroupAccount.IsInEditMode = false;
                        App.AppManager.GUILSEngine.DataManager.SaveGroup(groupNode);
                    }
                    else
                    {                        
                        App.AppManager.GUILSEngine.DataManager.SaveGroup(groupNode);
                    }
                    groupNode.IsInEditMode = false;
                    groupNode.IsWaitingForServerResponse = true;
                }
                else if (accountNode != null)
                {
                    accountNode.Id = accountNode.Name;
                    App.AppManager.GUILSEngine.DataManager.SaveAccount(accountNode);
                    accountNode.IsInEditMode = false;
                    accountNode.IsWaitingForServerResponse = true;
                }
            }
        }

        bool isContextMenuOpened = false;
        private void treeView_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Logger.DEBUG("treeView_PreviewMouseRightButtonDown(..)");
            this.isContextMenuOpened = true;
            e.Handled = true;
        }

        private void RenameGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("RenameGroupMenuItem_Click(..)");
            if (CheckModifiedRiskSetting() == false)
            {
                return;
            }

            INodeEntity selectedNode = (sender as MenuItem).DataContext as INodeEntity;
            if (selectedNode != null)
            {
                if ((selectedNode is Group) && (selectedNode as Group).IsDefaultGroup)
                {
                    return;
                }
                selectedNode.IsInEditMode = true;
                if (selectedNode is Group && (selectedNode as Group).IsAccountGroup)
                {
                    (selectedNode as Group).GroupAccount.IsInEditMode = true;
                }

                TreeViewItem tvi = this.treeView.ItemContainerGenerator.ContainerFromItem(this.treeView.SelectedItem) as TreeViewItem;
                if (tvi != null)
                {
                    object txtBoxObj = GetChildDependencyObjectFromVisualTree(tvi, typeof(TextBox));
                    if (txtBoxObj is TextBox)
                    {
                        (txtBoxObj as TextBox).Focus();
                        (txtBoxObj as TextBox).SelectAll();
                    }
                }
            }
        }


        private DependencyObject GetChildDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        {
            Logger.DEBUG("GetChildDependencyObjectFromVisualTree(..)");
            //Look in every branch inside to find the object
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(startObject); i++)
            {
                if (type.IsInstanceOfType(VisualTreeHelper.GetChild(startObject, i)))
                {
                    return VisualTreeHelper.GetChild(startObject, i);
                }
                else
                {
                    DependencyObject child = GetChildDependencyObjectFromVisualTree(VisualTreeHelper.GetChild(startObject, i), type);
                    if (type.IsInstanceOfType(child))
                    {
                        return child;
                    }
                }
            }

            return null;
        }

        private void treeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Logger.DEBUG("treeView_SelectedItemChanged(..)"); 
            
            //if (this.IsItemMoved)
            //{
                LoadRiskControl(sender);
//            }
        }

        private void treeView_PreviewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            Logger.DEBUG("treeView_PreviewSelectedItemChanged(..)");

            Group oldSelectedGroup;
            Group newSelectedGroup;

            oldSelectedGroup = GetGroupNode(e.OldValue);
            newSelectedGroup = GetGroupNode(e.NewValue);

            if (oldSelectedGroup != null && newSelectedGroup != null && oldSelectedGroup.Id.Equals(newSelectedGroup.Id))
            {
                Logger.DEBUG("old-selected node: " + (e.OldValue as INodeEntity).Name +", and selected node: "+(e.NewValue as INodeEntity).Name+",  has same group");
                //No need to check changes as the group is remain same for new/old selected account and/or groups
                // But Still even if old and new selection are for same group, the group-settins could be differnet when new Group/Account is creating and not saved
                // so it also need to check if the oldSelectedGroup is in Risk-Coltrol view
                MainView mainView = (this.Parent as Grid).Parent as MainView;
                GroupSettingVM oldGroupSettingVM = mainView.SelectedGroupSettingVM();
                if (oldGroupSettingVM != null && !oldGroupSettingVM.SelectedGroup.Id.Equals(oldSelectedGroup.Id))
                {
                    //
                }
                else
                {
                    return;
                }
            }
            else if (oldSelectedGroup == null || newSelectedGroup == null)
            {
                MainView mainView = (this.Parent as Grid).Parent as MainView;
                //Check to see if new group/acount is selected in Risk Setting Control
                if (!mainView.IsRiskSettingModified())
                {
                    Logger.DEBUG("Either oldSelectedGroup or  newSelectedGroup is null");
                    //No need to check changes as Either oldSelectedGroup or  newSelectedGroup is null
                    return;
                }

            }

            if (CheckModifiedRiskSetting() == false)
            {
                e.Handled = true;
            }
        }

        private Group GetGroupNode(object node)
        {
            Group nodeGroup = null;
            if (node is Account)
            {
                nodeGroup = (node as Account).ParentGroup;
            }
            else if (node is Group)
            {
                nodeGroup = (node as Group);
            }
            return nodeGroup;
        }

        private bool CheckModifiedRiskSetting()
        {
            Logger.DEBUG("CheckModifiedRiskSetting(..)"); 

            if ((this.Parent is Grid) && (this.Parent as Grid).Parent is MainView)
            {
                MainView mainView = (this.Parent as Grid).Parent as MainView;
                if (mainView.IsRiskSettingModified())
                {
                    MessageBoxResult result = MessageBox.Show("Risk settings are modified and not saved.\nWould you like to save changes?", "Risk Settings", MessageBoxButton.YesNoCancel);
                    if (result == MessageBoxResult.Cancel)
                    {
                        return false;
                    }
                    else if (result == MessageBoxResult.Yes)
                    {
                        mainView.SaveRiskSetting();
                    }
                }
            }

            return true;
        }

        private void treeView_SelectionCancelled(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("treeView_SelectionCancelled(..)"); 

            //MessageBox.Show("Cancelled");
        }

        private NODE_DROP_LOCATION node_drop_to = NODE_DROP_LOCATION.DEFAULT;

        private void Image_DropUp(object sender, DragEventArgs e)
        {
            Logger.DEBUG("Image_DropUp(..)"); 
            node_drop_to = NODE_DROP_LOCATION.UP;
            ((sender as Image).Parent as Border).Visibility = System.Windows.Visibility.Collapsed;
            ((sender as Image).Parent as Border).Background = Brushes.Transparent;
        }

        private void StackPanel_DragEnter(object sender, DragEventArgs e)
        {
            Logger.DEBUG("StackPanel_DragEnter(..)");

            if (App.AppManager.GUILSEngine.DataManager.IsAlphabetize)
            {
                return;
            }

            if (this.isContextMenuOpened)
            {
                return;
            }

            if (sender is StackPanel && (sender as StackPanel).DataContext is Group)
            {
                StackPanel sktPnl = sender as StackPanel;
                sktPnl.Children[0].Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void StackPanel_DragLeave(object sender, DragEventArgs e)
        {
            Logger.DEBUG("StackPanel_DragLeave(..)"); 
            if (this.isContextMenuOpened)
            {
                return;
            }

            if (sender is StackPanel)
            {
                StackPanel sktPnl = sender as StackPanel;
                sktPnl.Children[0].Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void Image_DragEnter(object sender, DragEventArgs e)
        {
            Logger.DEBUG("StackPanel_DragLeave(..)");
            e.Handled = true;
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("ContextMenu_Opened(..)"); 
            if (sender is ContextMenu)
            {
                if ((sender as ContextMenu).DataContext is INodeEntity)
                {
                    ((sender as ContextMenu).DataContext as INodeEntity).IsSelected = true;
                }
            }
            this.IsItemMoved = true;
        }

        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("TextBox_Loaded(..)"); 

            if (((sender as TextBox).DataContext as INodeEntity).IsInEditMode)
            {
                (sender as TextBox).Focus();
                (sender as TextBox).SelectAll();
            }
        }

        private void TextBlock_DragOver(object sender, DragEventArgs e)
        {
            Logger.DEBUG("TextBlock_DragOver(..)"); 

            (sender as StackPanel).Background = Brushes.LightGray;
        }


        private void TextBlock_DragLeave(object sender, DragEventArgs e)
        {
            Logger.DEBUG("TextBlock_DragLeave(..)"); 
            (sender as StackPanel).Background = Brushes.Transparent;
        }

        private void Border_DragOver(object sender, DragEventArgs e)
        {
            Logger.DEBUG("Border_DragOver(..)");
            (sender as Border).Background = Brushes.LightGray;
        }

        private void Border_DragLeave(object sender, DragEventArgs e)
        {
            Logger.DEBUG("Border_DragLeave(..)");
            (sender as Border).Background = Brushes.Transparent;
        }

        private void Border_Drop(object sender, DragEventArgs e)
        {
            Logger.DEBUG("Border_Drop(..)");
            
            node_drop_to = NODE_DROP_LOCATION.UP; 
            (sender as Border).Visibility = System.Windows.Visibility.Collapsed;
            (sender as Border).Background = Brushes.Transparent; 

        }

        private void StackPanel_Drop(object sender, DragEventArgs e)
        {
            Logger.DEBUG("StackPanel_Drop(..)");
            (sender as StackPanel).Background = Brushes.Transparent;
            ((sender as StackPanel).Parent as StackPanel).Children[0].Visibility = System.Windows.Visibility.Collapsed;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Logger.DEBUG("TextBox_PreviewKeyDown(..)");
            TextBox txtGroupAccount = sender as TextBox;
            Group groupNode = txtGroupAccount.DataContext as Group;
            Account accountNode = txtGroupAccount.DataContext as Account;
            if (txtGroupAccount != null)
            {
                if (e.Key.GetHashCode() == 13)
                {
                    if (groupNode != null)
                    {
                        if (!groupNode.IsAccountGroup)
                        {
                            GroupAccount groupAccount = App.AppManager.GUILSEngine.DataManager.GetGroupAccount(groupNode.Id);
                            if (groupAccount != null)
                            {
                                groupNode.IsInEditMode = false;
                                groupNode.Name = groupAccount.DisplayName;
                            }
                        }
                    }
                    else if (accountNode != null)
                    {
                        accountNode.Id = accountNode.Name;
                        accountNode.ParentGroup.AccountList.Remove(accountNode);
                    }
                }
                else if (e.Key == Key.Enter)
                {
                    try
                    {
                        SaveGroupAndAccount(txtGroupAccount, groupNode, accountNode);
                    }
                    catch (Exception ex)
                    {
                        e.Handled = true;
                        throw ex;
                    }
                }
            }
        }

        private void TextBox_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            Logger.DEBUG("TextBox_PreviewLostKeyboardFocus(..)");
            TextBox txtGroupAccount = sender as TextBox;
            Group groupNode = txtGroupAccount.DataContext as Group;
            Account accountNode = txtGroupAccount.DataContext as Account;
            try
            {
                SaveGroupAndAccount(txtGroupAccount, groupNode, accountNode);
            }
            catch (Exception ex)
            {
                e.Handled = true;
                throw ex;
            }
        }

        private bool SaveGroupAndAccount(TextBox txtGroupAccount, Group groupNode, Account accountNode)
        {
            if (groupNode != null)
            {
                if (groupNode.IsAccountGroup)
                {
                    groupNode.GroupAccount.Id = groupNode.GroupAccount.Name;
                    groupNode.Name = groupNode.GroupAccount.Name;
                    groupNode.Id = groupNode.GroupAccount.Id;
                    groupNode.GroupAccount.IsInEditMode = false;
                    GroupUserControl.EditChangeReason(groupNode.Name);
                    App.AppManager.GUILSEngine.DataManager.SaveGroup(groupNode);
                    groupNode.IsInEditMode = false;
                    groupNode.IsWaitingForServerResponse = true;
                }
                else
                {
                    if (!groupNode.Name.Equals(txtGroupAccount.Text.Trim()))
                    {
                        groupNode.Name = txtGroupAccount.Text.Trim();
                        if (App.AppManager.GUILSEngine.DataManager.ValidateExistingGroupAccount(groupNode))
                        {
                            GroupUserControl.EditChangeReason(groupNode.Name);
                            App.AppManager.GUILSEngine.DataManager.SaveGroup(groupNode);
                            groupNode.IsInEditMode = false;
                            groupNode.IsWaitingForServerResponse = true;
                        }
                    }
                    else
                    {
                        groupNode.IsInEditMode = false;
                        groupNode.IsWaitingForServerResponse = true;
                    }
                }
            }
            else if (accountNode != null)
            {
                accountNode.Name = txtGroupAccount.Text.Trim();
                accountNode.Id = accountNode.Name;
                GroupUserControl.EditChangeReason(accountNode.Name);
                App.AppManager.GUILSEngine.DataManager.SaveAccount(accountNode);
                accountNode.IsInEditMode = false;
                accountNode.IsWaitingForServerResponse = true;
            }
            return true;
        }

        private void stkPnlTreeViewItemTextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.treeView.SelectedItem == null)
            {
                if (((sender as StackPanel).DataContext == null) || (((sender as StackPanel).DataContext as INodeEntity) == null))
                {
                    return;
                }
                ((sender as StackPanel).DataContext as INodeEntity).IsSelected = true;
            }
        }

        private static void MoveAccount(Account accountToMove, string parentId)
        {
            GroupUserControl.EditChangeReason(accountToMove.Name);
            App.AppManager.GUILSEngine.DataManager.MoveAccountNode(accountToMove, parentId);
        }
    }

    public enum NODE_DROP_LOCATION
    {
        UP,
        DEFAULT,
    }
}
