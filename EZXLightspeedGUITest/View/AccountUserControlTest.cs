using EZXLightspeedGUI.View;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using System.Windows.Input;
using EZX.LightspeedEngine.Entity;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using EZXLightspeedGUI;
using EZX.LightspeedMockup.Mock;
using EZXLib;

namespace EZXLightspeedGUITest
{


    [TestClass()]
    public class AccountUserControlTest
    {
        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            //Create the application for resources.
            if (System.Windows.Application.Current == null)
            {
                ApplicationManager.MOCK_MODE = true;
                App application = new App();
                application.InitializeComponent();
                App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Clear();
                App.AppManager.GUILSEngine.DataManager.AccountGroupList.Clear();
            }
        }


        [TestMethod()]
        public void AccountUserControlConstructorTest()
        {
            AccountUserControl target = new AccountUserControl();
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void Border_DragLeaveTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            Border brdr = new Border();
            brdr.Background = Brushes.Blue;
            DragEventArgs e = null; 
            target.Border_DragLeave(brdr, e);
            Assert.AreEqual(Brushes.Transparent, brdr.Background);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void Border_DragOverTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            Border brdr = new Border();
            brdr.Background = Brushes.Transparent;
            DragEventArgs e = null;
            target.Border_DragOver(brdr, e);
            Assert.AreEqual(Brushes.LightGray, brdr.Background);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void Border_DropTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            Border brdr = new Border();
            brdr.Background = Brushes.LightGray;
            brdr.Visibility = Visibility.Visible;
            DragEventArgs e = null;
            target.Border_Drop(brdr, e);
            Assert.AreEqual(Brushes.Transparent, brdr.Background);
            Assert.AreEqual(Visibility.Collapsed, brdr.Visibility);
            Assert.AreEqual(target.node_drop_to, NODE_DROP_LOCATION.UP);

        }

        //[TestMethod()]
        //[DeploymentItem("EZXAccountRiskGUI.exe")]
        //public void CheckModifiedRiskSettingTest()
        //{
        //    AccountUserControl_Accessor target = new AccountUserControl_Accessor(); 
        //    bool expected = false; 
        //    bool actual;
        //    actual = target.CheckModifiedRiskSetting();
        //    Assert.AreEqual(expected, actual);
        //}

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ContextMenu_ClosedTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor(); 
            ContextMenu sender = new ContextMenu();
            RoutedEventArgs e = null;
            target.isContextMenuOpened = true;
            target.ContextMenu_Closed(sender, e);
            Assert.IsFalse(target.isContextMenuOpened); 
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ContextMenu_MouseRightButtonDownTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            ContextMenu sender = new ContextMenu();
            MouseButtonEventArgs e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Right);
            e.RoutedEvent = UIElement.MouseRightButtonDownEvent;
            e.Source = sender;
            e.Handled = false;
            Assert.IsFalse(e.Handled);
            target.ContextMenu_MouseRightButtonDown(sender, e);
            Assert.IsTrue(e.Handled);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ContextMenu_OpenedTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            Group grp = new Group() { Id = "G1", Name = "G1", IsSelected = false };
            ContextMenu sender = new ContextMenu();
            sender.DataContext = grp;
            RoutedEventArgs e = null;
            Assert.IsFalse(grp.IsSelected);
            target.ContextMenu_Opened(sender, e);
            Assert.IsTrue(grp.IsSelected);
            Assert.IsTrue(target.IsItemMoved);

        }

        //[TestMethod()]
        //[DeploymentItem("EZXAccountRiskGUI.exe")]
        //public void CreateAccountGroupMenuItem_ClickTest()
        //{
        //    AccountUserControl_Accessor target = new AccountUserControl_Accessor(); 
        //    object sender = null; 
        //    RoutedEventArgs e = null; 
        //    target.CreateAccountGroupMenuItem_Click(sender, e);
        //}

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void CreateAccountMenuItem_ClickTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            Group grp = new Group() { Id = "G1", Name = "G1", IsSelected = false };
            grp.AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>();
            MenuItem sender = new MenuItem();
            sender.DataContext = grp;
            RoutedEventArgs e = null; 
            target.CreateAccountMenuItem_Click(sender, e);

            Assert.AreEqual(1, grp.AccountList.Count);
            Assert.AreEqual(" New Account", grp.AccountList[0].Name);
            Assert.AreEqual(true, grp.AccountList[0].IsSelected);
            Assert.AreEqual(true, grp.IsExpanded);
            Assert.AreEqual(grp, grp.AccountList[0].ParentGroup);

        }

        //[TestMethod()]
        //[DeploymentItem("EZXAccountRiskGUI.exe")]
        //public void CreateGroupMenuItem_ClickTest()
        //{
        //    AccountUserControl_Accessor target = new AccountUserControl_Accessor(); 
        //    object sender = null; 
        //    RoutedEventArgs e = null; 
        //    target.CreateGroupMenuItem_Click(sender, e);
        //}

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void CreateNewAccountGroupTest()
        {
            Group grp = new Group() { Id = "G1", Name = "G1", IsSelected = false };
            grp.AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>();
            grp.RiskSetting = new RiskSetting();

            Account accountToMove = new Account();
            accountToMove.Name = "ACC1";
            accountToMove.Id = "ACC1";
            accountToMove.ParentGroup = grp;
            grp.AccountList.Add(accountToMove);


            Group standaloneGroup = AccountUserControl_Accessor.CreateNewAccountGroup(accountToMove);
            Assert.AreEqual(accountToMove.Name, standaloneGroup.Name);
            Assert.AreEqual(accountToMove.Id, standaloneGroup.Id);
            Assert.AreEqual(null, standaloneGroup.AccountList);
            Assert.AreEqual(accountToMove, standaloneGroup.GroupAccount);
            Assert.AreEqual(true, standaloneGroup.IsAccountGroup);
            Assert.AreEqual(true, accountToMove.IsOwnGroup);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void DeleteAccountMenuItem_ClickTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            MenuItem sender = new MenuItem();
            RoutedEventArgs e = null;
            
            Group grp = new Group() { Id = "G1", Name = "G1", IsSelected = false };
            grp.AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>();
            grp.RiskSetting = new RiskSetting();

            Account account = new Account();
            account.Name = "ACC1";
            account.Id = "ACC1";
            account.ParentGroup = grp;
            grp.AccountList.Add(account);
            ApplicationManager.MOCK_MODE = true;
            sender.DataContext = account;

            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance = new EZXLib.ReadGroupAccounts();

            EZXLib.GroupAccount groupaccountObj1 = new EZXLib.GroupAccount();
            groupaccountObj1.DisplayName = grp.Name;
            groupaccountObj1.Id = grp.Id;
            groupaccountObj1.OwnerID = null;
            groupaccountObj1.Settings = new EZXLib.Properties();

            
            EZXLib.GroupAccount groupaccountObj2 = new EZXLib.GroupAccount();
            groupaccountObj2.DisplayName = account.Name;
            groupaccountObj2.Id = account.Name;
            groupaccountObj2.OwnerID = grp.Id;
            groupaccountObj2.Settings = new EZXLib.Properties();

            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupaccountObj1); 
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupaccountObj2);

            Assert.AreEqual(2, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
            Assert.AreEqual(1, grp.AccountList.Count);

            target.DeleteAccountMenuItem_Click(sender, e);
            
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
            Assert.AreEqual(0, grp.AccountList.Count);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void DeleteStandAloneAccountMenuItem_ClickTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            MenuItem sender = new MenuItem();
            RoutedEventArgs e = null;

            Group grp = new Group() { Id = "G1", Name = "G1", IsSelected = false };
            grp.AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>();
            grp.RiskSetting = new RiskSetting();

            Account account = new Account();
            account.Name = "ACC1";
            account.Id = "ACC1";
            account.ParentGroup = grp;
            grp.AccountList.Add(account);
            grp.IsAccountGroup = true;

            ApplicationManager.MOCK_MODE = true;
            sender.DataContext = grp;

            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance = new EZXLib.ReadGroupAccounts();


            EZXLib.GroupAccount groupaccountObj2 = new EZXLib.GroupAccount();
            groupaccountObj2.DisplayName = account.Name;
            groupaccountObj2.Id = account.Name;
            groupaccountObj2.OwnerID = account.Name;
            groupaccountObj2.Settings = new EZXLib.Properties();

            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupaccountObj2);

            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);

            target.DeleteAccountMenuItem_Click(sender, e);

            Assert.AreEqual(0, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void DeleteGroupMenuItem_ClickTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            MenuItem sender = new MenuItem();
            RoutedEventArgs e = null;

            Group grp = new Group() { Id = "G1", Name = "G1", IsSelected = false };
            grp.AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>();
            grp.RiskSetting = new RiskSetting();

            EZXLib.GroupAccount groupaccountObj2 = new EZXLib.GroupAccount();
            groupaccountObj2.DisplayName = grp.Name;
            groupaccountObj2.Id = grp.Name;
            groupaccountObj2.OwnerID = null;
            groupaccountObj2.Settings = new EZXLib.Properties();

            sender.DataContext = grp;

            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupaccountObj2);

            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
            target.DeleteGroupMenuItem_Click(sender, e);
            Assert.AreEqual(0, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void GetChildDependencyObjectFromVisualTreeTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            //DependencyObject startObject = null; 
            //Type type = null; 
            //DependencyObject expected = null;
            DependencyObject actual;


            Grid grd = new Grid();
            StackPanel sktpnk1 = new StackPanel();
            TextBox txtBx = new TextBox();
            txtBx.Text = "TextBox1";
            sktpnk1.Children.Add(txtBx);

            StackPanel sktpnk2 = new StackPanel();
            TextBlock txtBlk = new TextBlock();
            txtBlk.Text = "TextBlock1";
            sktpnk2.Children.Add(txtBlk);

            grd.Children.Add(sktpnk1);
            grd.Children.Add(sktpnk2);

            actual = target.GetChildDependencyObjectFromVisualTree(grd, typeof(TextBox));
            if (actual is TextBox)
            {
                Assert.AreEqual("TextBox1", (actual as TextBox).Text);
            }
            else
            {
                Assert.IsTrue(false);
            }

            actual = target.GetChildDependencyObjectFromVisualTree(grd, typeof(TextBlock));
            if (actual is TextBlock)
            {
                Assert.AreEqual("TextBlock1", (actual as TextBlock).Text);
            }
            else
            {
                Assert.IsTrue(false);
            }


            actual = target.GetChildDependencyObjectFromVisualTree(grd, typeof(Border));
            Assert.IsNull(actual);
        
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void GetGroupNodeTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            INodeEntity node1 = new Group() { Name = "G1", Id = "G1" }; 
            Group expected = node1 as Group; 
            Group actual;
            actual = target.GetGroupNode(node1);
            Assert.AreEqual(expected, actual);

            INodeEntity node2 = new Group() { Name = "G2", Id = "G2" };
            INodeEntity nodeAccount = new Account() { Name = "A1", Id = "A1", ParentGroup = (node2 as Group)};

            expected = node2 as Group;
            actual = target.GetGroupNode(nodeAccount);
            Assert.AreEqual(expected, actual);

            expected = null;
            actual = target.GetGroupNode(null);
            Assert.AreEqual(expected, actual);


        }


        //[TestMethod()]
        //[DeploymentItem("EZXAccountRiskGUI.exe")]
        //public void LoadRiskControlTest()
        //{
        //    Grid grid = new Grid();

        //    AccountUserControl_Accessor target = new AccountUserControl_Accessor();

        //    object sender = null; 
        //    bool expected = false; 
        //    bool actual;
        //    actual = target.LoadRiskControl(sender);
        //    Assert.AreEqual(expected, actual);
        //}

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void RenameGroupMenuItem_ClickTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            MenuItem sender = new MenuItem(); 
            RoutedEventArgs e = null;

            Group grp = new Group() { Name = "G1", Id = "G1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };

            sender.DataContext = grp;
            Assert.IsFalse(grp.IsInEditMode);
            target.RenameGroupMenuItem_Click(sender, e);
            Assert.IsTrue(grp.IsInEditMode);

            Group grpDefault = new Group() { Name = "Default", Id = "Default", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), IsDefaultGroup = true };
            sender.DataContext = grpDefault;
            Assert.IsFalse(grpDefault.IsInEditMode);
            target.RenameGroupMenuItem_Click(sender, e);
            Assert.IsFalse(grpDefault.IsInEditMode);


            Group standaloneAccountGroup = new Group() { Name = "AccountGroup", Id = "AccountGroup", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>()};
            Account standaloneAccount = new Account() { Name = "SA1", Id = "SA1", ParentGroup = standaloneAccountGroup };
            standaloneAccountGroup.AccountList.Add(standaloneAccount);
            standaloneAccountGroup.IsAccountGroup = false;

            sender.DataContext = standaloneAccountGroup;
            Assert.IsFalse(standaloneAccountGroup.IsInEditMode);
            target.RenameGroupMenuItem_Click(sender, e);
            Assert.IsTrue(standaloneAccountGroup.IsInEditMode);



        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ResetUpDownIconVisibility_WhenSourceIsTextBlockTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            StackPanel stkPnl1 = new StackPanel();
            StackPanel stkPnl2 = new StackPanel();
            TextBlock txtBlk = new TextBlock();
            stkPnl2.Children.Add(txtBlk);
            stkPnl1.Children.Add(stkPnl2);

            stkPnl2.Visibility = Visibility.Visible;
            stkPnl1.Background = Brushes.Gray;

            target.ResetUpDownIconVisibility(txtBlk);

            Assert.AreEqual(Visibility.Collapsed, stkPnl2.Visibility);
            Assert.AreEqual(Brushes.Transparent, stkPnl1.Background);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ResetUpDownIconVisibility_WhenSourceIsBorderTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            StackPanel stkPnl1 = new StackPanel();
            StackPanel stkPnl2 = new StackPanel();
            Border brdr = new Border();
            stkPnl2.Children.Add(brdr);
            stkPnl1.Children.Add(stkPnl2);

            brdr.Visibility = Visibility.Visible;

            target.ResetUpDownIconVisibility(brdr);

            Assert.AreEqual(Visibility.Collapsed, brdr.Visibility);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ResetUpDownIconVisibility_WhenSourceIsImageTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            StackPanel stkPnl2 = new StackPanel();
            Border brdr = new Border();
            Image img = new Image();
            img.Tag = "ImageIcon";
            brdr.Child = img;            
            stkPnl2.Children.Add(brdr);

            brdr.Visibility = Visibility.Visible;

            target.ResetUpDownIconVisibility(img);

            Assert.AreEqual(Visibility.Collapsed, brdr.Visibility);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ResetUpDownIconVisibility_WhenSourceIsUpIconImageTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            StackPanel stkPnl2 = new StackPanel();
            Border brdr = new Border();
            Image img = new Image();
            img.Tag = "UpIcon";
            brdr.Child = img;
            stkPnl2.Children.Add(brdr);

            brdr.Visibility = Visibility.Visible;

            target.ResetUpDownIconVisibility(img);

            Assert.AreEqual(Visibility.Collapsed, brdr.Visibility);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ResetUpDownIconVisibility_WhenSourceIsStackPanelWithParentTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            StackPanel stkPnl1 = new StackPanel();
            StackPanel stkPnl2 = new StackPanel();
            Border brdr = new Border();
            stkPnl2.Children.Add(brdr);
            stkPnl1.Children.Add(stkPnl2);

            stkPnl2.Visibility = Visibility.Visible;

            target.ResetUpDownIconVisibility(stkPnl2);

            Assert.AreEqual(Visibility.Collapsed, stkPnl2.Visibility);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ResetUpDownIconVisibility_WhenSourceIsStackPanelWithNoParentTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            StackPanel stkPnl2 = new StackPanel();
            Border brdr = new Border();
            stkPnl2.Children.Add(brdr);

            brdr.Visibility = Visibility.Visible;

            target.ResetUpDownIconVisibility(stkPnl2);

            Assert.AreEqual(Visibility.Collapsed, brdr.Visibility);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void ResetUpDownIconVisibility_WhenSourceIsGridTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            Grid grid = new Grid();
            grid.Visibility = Visibility.Visible;

            target.ResetUpDownIconVisibility(grid);

            Assert.AreEqual(Visibility.Visible, grid.Visibility);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void SaveGroupAndAccount_StandaloneAccountTest()
        {
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Clear();

            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            TextBox txtGroupAccount = new TextBox();
            Account accountNode = null;
            Group groupNode = new Group() { Name = "SA1", Id = "SA1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account standaloneAccount = new Account() { Name = "SA1", Id = "SA1", ParentGroup = groupNode, OwnerId = "SA1", IsOwnGroup = true };
            groupNode.RiskSetting = new RiskSetting();
            groupNode.AccountList.Add(standaloneAccount);
            groupNode.IsAccountGroup = true;
            groupNode.IsInEditMode = true;

            Assert.AreEqual(0, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(0, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
            target.SaveGroupAndAccount(txtGroupAccount, groupNode, accountNode);
            bool expected = false;
            bool actual = groupNode.IsInEditMode;

            Assert.AreEqual(expected, groupNode.IsInEditMode);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void SaveGroupAndAccount_RenameGroupName()
        {
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Clear();

            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            TextBox txtGroupAccount = new TextBox();
            Account accountNode = null;
            Group groupNode = new Group() { Name = "G1", Id = "G1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            groupNode.RiskSetting = new RiskSetting();
            groupNode.IsInEditMode = true;

            GroupAccount groupAccountObj = new GroupAccount() { Id = "G1", DisplayName = "G1", OwnerID = null, Settings = new Properties() };

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(groupNode);
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj);
            
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);

            txtGroupAccount.Text = "G1Ren";

            target.SaveGroupAndAccount(txtGroupAccount, groupNode, accountNode);
            
            bool expected = false;
            bool actual = groupNode.IsInEditMode;

            Assert.AreEqual(expected, groupNode.IsInEditMode);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
            
            Assert.AreEqual(txtGroupAccount.Text, App.AppManager.GUILSEngine.DataManager.AccountGroupList[0].Name);
            Assert.AreEqual(txtGroupAccount.Text, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts[0].DisplayName);


        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void SaveGroupAndAccount_RenameGroupNameAsSameName()
        {
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Clear();
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Clear();

            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            TextBox txtGroupAccount = new TextBox();
            Account accountNode = null;
            Group groupNode = new Group() { Name = "G1", Id = "G1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            groupNode.RiskSetting = new RiskSetting();
            groupNode.IsInEditMode = true;

            GroupAccount groupAccountObj = new GroupAccount() { Id = "G1", DisplayName = "G1", OwnerID = null, Settings = new Properties() };

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(groupNode);
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj);

            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);

            txtGroupAccount.Text = "G1";

            target.SaveGroupAndAccount(txtGroupAccount, groupNode, accountNode);

            bool expected = false;
            bool actual = groupNode.IsInEditMode;

            Assert.AreEqual(expected, groupNode.IsInEditMode);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);

            Assert.AreEqual(txtGroupAccount.Text, App.AppManager.GUILSEngine.DataManager.AccountGroupList[0].Name);
            Assert.AreEqual(txtGroupAccount.Text, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts[0].DisplayName);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void SaveGroupAndAccount_RenameGroupNameAsOtherGroupName()
        {
            EZX.LightSpeedEngine.LSEngine.TEST_MODE = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Clear();
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Clear();

            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            TextBox txtGroupAccount = new TextBox();
            Account accountNode = null;
            Group groupNode1 = new Group() { Name = "G1", Id = "G1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            Group groupNode2 = new Group() { Name = "G2", Id = "G2", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            Group groupNode3 = new Group() { Name = "G3", Id = "G3", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };

            GroupAccount groupAccountObj1 = new GroupAccount() { Id = "G1", DisplayName = "G1", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj2 = new GroupAccount() { Id = "G2", DisplayName = "G2", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj3 = new GroupAccount() { Id = "G3", DisplayName = "G3", OwnerID = null, Settings = new Properties() };

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(groupNode1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(groupNode2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(groupNode3);
            
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj1);
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj2);
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj3);

            Assert.AreEqual(3, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(3, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);

            txtGroupAccount.Text = "G2";

            groupNode1.IsInEditMode = true;

            target.SaveGroupAndAccount(txtGroupAccount, groupNode1, accountNode);

            bool expected = true;
            bool actual = groupNode1.IsInEditMode;

            Assert.AreEqual(expected, groupNode1.IsInEditMode);
            Assert.AreEqual(3, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(3, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);

            Assert.AreEqual(txtGroupAccount.Text, App.AppManager.GUILSEngine.DataManager.AccountGroupList[0].Name);
            Assert.AreNotEqual(txtGroupAccount.Text, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts[0].DisplayName);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void SaveGroupAndAccount_SaveNewAccount()
        {
            EZX.LightSpeedEngine.LSEngine.TEST_MODE = true;

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Clear();
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Clear();

            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            TextBox txtGroupAccount = new TextBox();
            //Account accountNode = null;
            Group groupNode1 = new Group() { Name = "G1", Id = "G1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            Group groupNode2 = new Group() { Name = "G2", Id = "G2", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            Group groupNode3 = new Group() { Name = "G3", Id = "G3", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };

            Account account1 = new Account() { Name = "New Account", Id = "New Account", ParentGroup = groupNode1 };
            groupNode1.AccountList.Add(account1);
            
            GroupAccount groupAccountObj1 = new GroupAccount() { Id = "G1", DisplayName = "G1", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj2 = new GroupAccount() { Id = "G2", DisplayName = "G2", OwnerID = null, Settings = new Properties() };
            GroupAccount groupAccountObj3 = new GroupAccount() { Id = "G3", DisplayName = "G3", OwnerID = null, Settings = new Properties() };

            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(groupNode1);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(groupNode2);
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Add(groupNode3);

            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj1);
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj2);
            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Add(groupAccountObj3);

            Assert.AreEqual(3, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(3, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);

            txtGroupAccount.Text = "A1";

            account1.IsInEditMode = true;

            target.SaveGroupAndAccount(txtGroupAccount, null, account1);

            bool expected = false;
            bool actual = account1.IsInEditMode;

            Assert.AreEqual(expected, groupNode1.IsInEditMode);
            Assert.AreEqual(3, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(4, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);

            Assert.AreEqual(txtGroupAccount.Text, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts[3].DisplayName);
        }

        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void SaveGroupAndAccount_GroupTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            TextBox txtGroupAccount = new TextBox();
            Account accountNode = null;
            Group groupNode = new Group() { Name = "SA1", Id = "SA1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>() };
            Account standaloneAccount = new Account() { Name = "SA1", Id = "SA1", ParentGroup = groupNode, OwnerId = "SA1", IsOwnGroup = true };
            groupNode.RiskSetting = new RiskSetting();
            groupNode.AccountList.Add(standaloneAccount);
            groupNode.IsAccountGroup = true;
            groupNode.IsInEditMode = true;

            App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Clear();
            App.AppManager.GUILSEngine.DataManager.AccountGroupList.Clear();

            Assert.AreEqual(0, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(0, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
            target.SaveGroupAndAccount(txtGroupAccount, groupNode, accountNode);
            bool expected = false;
            bool actual = groupNode.IsInEditMode;

            Assert.AreEqual(expected, groupNode.IsInEditMode);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.AccountGroupList.Count);
            Assert.AreEqual(1, App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance.GroupAccounts.Count);
        }




        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void TextBox_LoadedTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            TextBox sender = new TextBox();
            Group g1 = new Group() { Id = "G1", Name = "G1", AccountList = new EZXWPFLibrary.Helpers.MTObservableCollection<Account>(), RiskSetting = new RiskSetting() };
            g1.IsInEditMode = true;
            Assert.AreEqual("",sender.SelectedText);
            Assert.IsFalse(sender.IsFocused);
            sender.DataContext = g1;
            sender.Text = g1.Name;
            RoutedEventArgs e = null;
            target.TextBox_Loaded(sender, e);
            Assert.IsTrue(sender.IsFocused);
            Assert.AreEqual("G1", sender.SelectedText);
        }


        [TestMethod()]
        [DeploymentItem("EZXAccountRiskGUI.exe")]
        public void stkPnlTreeViewItemTextBlock_MouseLeftButtonDownTest()
        {
            AccountUserControl_Accessor target = new AccountUserControl_Accessor();
            
            Group g1 = new Group() { Id = "G1", Name = "G1", IsSelected = false};
            StackPanel sender = new StackPanel();
            sender.DataContext = g1;
            MouseButtonEventArgs e = null;
            target.stkPnlTreeViewItemTextBlock_MouseLeftButtonDown(sender, e);
            Assert.IsTrue(g1.IsSelected);
        }

    }
}
