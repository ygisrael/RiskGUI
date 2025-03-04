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
using System.Diagnostics;
using System.Globalization;
using EZXLib;
using EZXLightspeedGUI.Model;
using EZXWPFLibrary.Utils;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for MainMenuUserControl.xaml
    /// </summary>
    public partial class MainMenuUserControl : UserControl
    {
        public MainMenuUserControl()
        {
            InitializeComponent();
            App.AppManager.GUILSEngine.LoginCompleted += new EZX.LightSpeedEngine.LoginCompleteHandler(GUILSEngine_LoginCompleted);
        }

        void GUILSEngine_LoginCompleted(object sender, EZX.LightSpeedEngine.LSConnectionEventArgs e)
        {
            App.AppManager.RunOnDispatcherThread(() =>
            {
                this.LoginMenuItem.IsEnabled = false;
                this.LogoutMenuItem.IsEnabled = true;
                this.RefreshMenuItem.IsEnabled = true;
            });
        }

        private void CreateGroupMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Group newGroup = App.AppManager.GUILSEngine.DataManager.CreateNewGroupNode();
            if ((this.Parent is Grid))
            {
                if ((this.Parent as Grid).Parent is MainView)
                {
                    MainView mainView = (this.Parent as Grid).Parent as MainView;

                    if (mainView.IsRiskSettingModified())
                    {
                        MessageBoxResult result = MessageBox.Show("Risk settings are modified and not saved.\nWould you like to save changes?", "Risk Settings", MessageBoxButton.YesNoCancel);
                        if (result == MessageBoxResult.Cancel)
                        {
                            return;
                        }
                        else if (result == MessageBoxResult.Yes)
                        {
                            mainView.SaveRiskSetting();
                        }
                    }

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

        private void CreateAccountMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Group newGroup = App.AppManager.GUILSEngine.DataManager.CreateNewStandAloneAccountNode("New Account");
            if ((this.Parent is Grid))
            {
                if ((this.Parent as Grid).Parent is MainView)
                {
                    MainView mainView = (this.Parent as Grid).Parent as MainView;
                    if (mainView.IsRiskSettingModified())
                    {
                        MessageBoxResult result = MessageBox.Show("Risk settings are modified and not saved.\nWould you like to save changes?", "Risk Settings", MessageBoxButton.YesNoCancel);
                        if (result == MessageBoxResult.Cancel)
                        {
                            return;
                        }
                        else if (result == MessageBoxResult.Yes)
                        {
                            mainView.SaveRiskSetting();
                        }
                    }
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

        private void SaveGroupAccountOrder_Click(object sender, RoutedEventArgs e)
        {
            App.AppManager.GUILSEngine.DataManager.SaveGroupAccountConfig(string.Empty);
            App.AppManager.GUILSEngine.StatusMessage = "Saved Group/Account Display-Order.";
        }

        private void ResetGroupAccountOrder_Click(object sender, RoutedEventArgs e)
        {
            App.AppManager.GUILSEngine.DataManager.LoadAllGroupAndAccount(App.AppManager.GUILSEngine.DataManager.ReadGroupAccountsInstance);
            App.AppManager.GUILSEngine.StatusMessage = "Reset Group/Account Display-Order.";
        }

        private void RefreshGroupAccount_Click(object sender, RoutedEventArgs e)
        {
            App.AppManager.GUILSEngine.GetGroupAccountData();
            App.AppManager.GUILSEngine.StatusMessage = "Refreshed Group/Account data.";
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            LoginMenuItem.IsEnabled = true;
            LogoutMenuItem.IsEnabled = false;
            this.RefreshMenuItem.IsEnabled = false;
            App.AppManager.GUILSEngine.Disconnect();
            App.AppManager.GUILSEngine.Logout();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            LoginView view = LoginView.GetLoginView();
            view.ShowDialog();
            view.BringIntoView();            
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            EZXLib.Logger.DEBUG("MainMenuControlView.Exit_Click()...");

            App.Current.Shutdown();

        }

        private void AbountMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EZXLib.Logger.DEBUG("MainMenuUserControlView.AbountMenuItem_Click()...");

            AboutWindow view = new AboutWindow();
            view.ShowDialog();

        }

        private void ChangeHistoryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string reportLink = Properties.Settings.Default.CHANGECONFIGPAGE;
                string host = App.AppManager.GUILSEngine.ConfigInfo.LSConnectionInfo.Host;
                string reportURL = "http://" + host + reportLink;

                Process.Start(reportURL);
            }
            catch (Exception ex)
            {
            }

        }

        private void ViolationLogMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string reportLink = Properties.Settings.Default.RULVIOLATIONPAGE;
                string host = App.AppManager.GUILSEngine.ConfigInfo.LSConnectionInfo.Host;
                string reportURL = "http://" + host + reportLink;

                Process.Start(reportURL);
            }
            catch (Exception ex)
            {
            }
        }

        private void ImportBuyingPower_Click(object sender, RoutedEventArgs e)
        {
            Logger.DEBUG("ImportBuyingPower_Click(...)");
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = ""; 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Comma Seperated Value Files(.csv)|*.csv|Text documents (.txt)|*.txt";

            
            Nullable<bool> result = dlg.ShowDialog();
            
            if (result == true)
            {
                string filename = dlg.FileName;
                if (filename.ToLower().EndsWith(".txt") || filename.ToLower().EndsWith(".csv"))
                {
                    try
                    {
                        string[] lines = System.IO.File.ReadAllLines(filename);
                        if (!LoadAllData(lines))
                        {
                            Logger.DEBUG("LoadAllData(filename) is failed to load data successfully!");
                        }
                        else
                        {
                            Logger.DEBUG("LoadAllData(filename) loaded data successfully!");
                        }
                    }
                    catch (Exception ex)
                    {
                        string msg = "Failed to import file as exception occured as following:\n";
                        msg = msg + ex.ToString();
                        ProcessImportError(msg);
                    }
                }
                else
                {
                    string msg = "Failed to import file: [" + filename + "].\nOnly files with extention: [.txt or .csv], are able to import.";
                    ProcessImportError(msg);
                }
            }
        }

        private bool LoadAllData(string[] lines)
        {
            Logger.DEBUG("LoadAllData(string[] lines)");
            int lineCount = lines.Count();
            string headerErr = string.Empty;
            if (lineCount > 2)
            {
                if (!ValidateHeader(lines[0], lines[1], out headerErr))
                {
                    string msg = "Failed to import file due to following error(s)\n";
                    msg = msg + headerErr;
                    ProcessImportError(msg);
                    return false;
                }
                return LoadDataInColumn(lines);
            }
            else
            {
                string msg = "Failed to import file as not enough rows are exists to import data successfully!.";
                ProcessImportError(msg);
                return false;
            }
            return true;
        }

        private bool LoadDataInColumn(string[] lines)
        {
            List<NodeImportEntity> nodeImportEntityList = new List<NodeImportEntity>();
            if (lines == null)
            {
                lines = new String[0];
            }

            if (lines.Count() > 2)
            {
                List<string> allErrMsg = new List<string>();
                bool allValidRow = true;

                //LoadData from 3rd Row as the 1st 2 rows are header-rows
                for (int lineIndex = 2; lineIndex < lines.Count(); lineIndex++)
                {
                    string errMsg = string.Empty;
                    bool isValidRow = LoadData(lines[lineIndex], nodeImportEntityList, lineIndex, out errMsg);
                    if (!isValidRow)
                    {
                        string error = errMsg + "(line " + (lineIndex+1) + ")";
                        allErrMsg.Add(error);
                        allValidRow = false;
                        Logger.DEBUG(error);
                    }
                }

                if (!allValidRow)
                {
                    //Display Error Notification:
                    int goodDataRowCount = nodeImportEntityList.Count;
                    int errorDataRowCount = allErrMsg.Count;
                    ImportErrorView errorView = new ImportErrorView();
                    errorView.Init(allErrMsg, errorDataRowCount, goodDataRowCount);
                    if (!ApplicationManager.MOCK_MODE)
                    {
                        errorView.ShowDialog();
                    }
                    return false;
                }
                else
                {
                    //Display Import Data Grid
                    ImportDataView dataView = new ImportDataView();
                    dataView.Init(nodeImportEntityList);
                    if (!ApplicationManager.MOCK_MODE)
                    {
                        dataView.ShowDialog();
                    }
                }
            }
            return true;
        }

        private bool LoadData(string dataLine, List<NodeImportEntity> nodeImportEntityList, int lineNumber, out string errMsg)
        {
            Logger.DEBUG("LoadData(string dataLine,List<NodeImportEntity> nodeImportEntityList, out string errMsg)");
            errMsg = string.Empty;
            //Check if line is not empty there is no need to show errors
            if (string.IsNullOrEmpty(dataLine) || string.IsNullOrEmpty(dataLine.Trim()))
            {
                return true;
            }
            
            //dataLine = dataLine.Replace(" ", "");
            dataLine = dataLine.Trim();
            string[] data = dataLine.Split(new char[] { ',' });
            if (data.Count() != 3)
            {
                if (data.Count() == 4)
                {
                    data[3] = data[3].Trim();
                    if (!string.IsNullOrEmpty(data[3]))
                    {
                        errMsg = "Invalid format, requires 3 values on line";
                        return false;
                    }
                }
                else
                {
                    errMsg = "Invalid format, requires 3 values on line";
                    return false;
                }
            }

            string groupName = data[0].Trim();
            string accountId = data[1].Trim();
            string buyingPower = data[2].Trim();

            groupName = StringUtils.TrimDoubleQuotes(groupName);
            accountId = StringUtils.TrimDoubleQuotes(accountId);

            if (string.IsNullOrEmpty(groupName) && string.IsNullOrEmpty(accountId) && string.IsNullOrEmpty(buyingPower))
            {
                //empty row having data as [, , ,], so not raising error and/or loading data
                return true;
            }

            if (!string.IsNullOrEmpty(buyingPower) && (!string.IsNullOrEmpty(groupName) || !string.IsNullOrEmpty(accountId)))
            {
                if (!string.IsNullOrEmpty(groupName) && !string.IsNullOrEmpty(accountId))
                {
                    errMsg = "Failed to load data, as Group and Account could never be on the same line.";
                    return false;
                }

                long buyingPowerValue = 0;
                if (!Int64.TryParse(buyingPower.Trim(), out buyingPowerValue))
                {
                    errMsg = "Failed to load data, as Buying Power value: " + buyingPower + ", is failed to parse into whole number.";
                    return false;
                }

                if (buyingPowerValue > 20000000000 || buyingPowerValue < 0)
                {
                    errMsg = "Failed to load data, as Buying Power value: [" + buyingPowerValue + "] is not in range  [0 to 20 Billion].";
                    return false;
                }


                //Till here Row is validated successfully, now need to validate data with existing Groups/Accounts
                string valiationErrMsg = string.Empty;
                INodeEntity node = null;
                if (!ValidateAndParseExistingGroupAccount(groupName, accountId, out valiationErrMsg, out node))
                {
                    errMsg = valiationErrMsg;
                    return false;
                }

                if (node == null)
                {
                    errMsg = "Error occurred as node value is null, when no validation error occurred!";
                    Logger.ERROR(errMsg);
                    return false;
                }

                NodeImportEntity nodeEntity = new NodeImportEntity();
                if (!string.IsNullOrEmpty(groupName))
                {
                    if (nodeImportEntityList.Where(n => n.GroupId == node.Id).Any())
                    {
                        errMsg = "Group: [" + node.Name + "] is more than one in import file";
                        return false;
                    }

                    nodeEntity.GroupId = node.Id;
                    nodeEntity.GroupName = node.Name;
                }
                else
                {
                    if (nodeImportEntityList.Where(n => n.AccountId == node.Id).Any())
                    {
                        errMsg = "Account: [" + node.Name + "] is more than one in import file";
                        return false;
                    }
                    nodeEntity.GroupId = node.Id;
                    nodeEntity.AccountId = node.Name;
                }
                nodeEntity.NodeEntity = node;
                nodeEntity.BuyingPower = buyingPowerValue;
                nodeEntity.LineNumber = lineNumber;
                nodeEntity.IsSubmitted = false;
                nodeImportEntityList.Add(nodeEntity);
            }
            else
            {
                errMsg = "Failed to load data as the row must have either Group or Account, and Buying Power.";
                return false;
            }

            return true;
        }

        private bool ValidateAndParseExistingGroupAccount(string groupName, string accountId, out string errMsg, out INodeEntity nodeEntity)
        {
            nodeEntity = null;
            errMsg = string.Empty;
            if (!string.IsNullOrEmpty(groupName))
            {
                Group group = App.AppManager.GUILSEngine.DataManager.FindGroupNodeByName(groupName);
                if (group == null || group.IsAccountGroup)
                {
                    errMsg = "Unknown group: [" + groupName + "]";
                    return false;
                }
                nodeEntity = group;
            }
            else if (!string.IsNullOrEmpty(accountId))
            {
                Group standaloneAccountGroup = App.AppManager.GUILSEngine.DataManager.FindGroupNodeByName(accountId);
                if (standaloneAccountGroup == null || !standaloneAccountGroup.IsAccountGroup)
                {
                    INodeEntity node = App.AppManager.GUILSEngine.DataManager.FindNodeEntity(accountId);
                    if (node != null && node is Account)
                    {
                        Account account = node as Account;
                        errMsg = "Not allowed to specify settings for account [" + accountId + "], which is part of group: [" + account.ParentGroup.Name + "]";
                    }
                    else
                    {
                        errMsg = "Unknown account: [" + accountId + "]";
                    }
                    return false;
                }
                else
                {
                    //Get with exact match-case
                    Group standaloneAccountGroupCaseSensitive = App.AppManager.GUILSEngine.DataManager.FindCaseSensitiveGroupNodeByName(accountId);
                    if (standaloneAccountGroupCaseSensitive != null)
                    {
                        standaloneAccountGroup = standaloneAccountGroupCaseSensitive;
                    }
                }
                nodeEntity = standaloneAccountGroup;
            }
            return true;
        }

        private void ProcessImportError(string msg)
        {
            Logger.DEBUG(msg);
            if (!ApplicationManager.MOCK_MODE)
            {
                MessageBox.Show(msg, "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateHeader(string dateHeaderline, string columnHeaderLine, out string errMsg)
        {
            errMsg = string.Empty;
            string dateHeaderError = string.Empty;
            string columnHeaderError = string.Empty;

            bool isValidDateHeaderLine = ValidateDateLine(dateHeaderline, out dateHeaderError);
            bool isValidColumnHeaderLine = ValidateColumnHeaderLine(columnHeaderLine, out columnHeaderError);

            if (isValidDateHeaderLine && isValidColumnHeaderLine)
            {
                Logger.DEBUG("Date and Column header lines are validated successfully!");
                return true;
            }

            if (string.IsNullOrEmpty(dateHeaderError))
            {
                errMsg = columnHeaderError;
            }
            else
            {
                errMsg = dateHeaderError;
                if (!string.IsNullOrEmpty(columnHeaderError))
                {
                    errMsg = errMsg+"\n"+columnHeaderError;
                }
            }
            return false;
        }

        private bool ValidateColumnHeaderLine(string columnHeaderLine, out string errMsg)
        {
            Logger.DEBUG("ValidateColumnHeaderLine(...) for  columnHeaderLine: " + columnHeaderLine);

            errMsg = string.Empty;
            string[] headerfields = columnHeaderLine.Split(new char[] { ',' });
            if (headerfields.Count() > 2 && headerfields.Count() < 5)
            {
                bool isValidGroupHeader = ValidateColumnName(headerfields[0], "Group");
                bool isValidAccountHeader = ValidateColumnName(headerfields[1], "Account");
                bool isValidBuyingPowerHeader = ValidateColumnName(headerfields[2], "Buying Power");

                if (headerfields.Count() > 3)
                {
                    headerfields[3] = headerfields[3].Trim();
                    if (!string.IsNullOrEmpty(headerfields[3]))
                    {
                        errMsg = "Invalid data [" + headerfields[3] + "], is exists at the end of column-header";
                        return false;
                    }
                }

                if (isValidGroupHeader && isValidAccountHeader && isValidBuyingPowerHeader)
                {
                    return true;
                }

                if (!isValidGroupHeader)
                {
                    errMsg = "Column header row is not valid to match [Group] column.\n";
                }
                if (!isValidAccountHeader)
                {
                    errMsg = errMsg + "Column header row is not valid to match [Account] column.\n";
                }
                if (!isValidBuyingPowerHeader)
                {
                    errMsg = errMsg + "Column header row is not valid to match [Buying Power] column.\n";
                }
                Logger.WARN(errMsg);    
            }
            else
            {
                errMsg = "Failed to load Column Header Row: "+columnHeaderLine;
                Logger.WARN(errMsg);
            }
            return false;
        }

        private bool ValidateColumnName(string columnHeader, string matchedColumn)
        {
            matchedColumn = matchedColumn.Replace(" ", "");
            columnHeader = columnHeader.Replace(" ", "");
            if (!string.IsNullOrEmpty(columnHeader) && columnHeader.Trim().ToLower().Equals(matchedColumn.ToLower()))
            {
                return true;
            }
            return false;
        }

        private bool ValidateDateLine(string dateHeaderline, out string errMsg)
        {
            Logger.DEBUG("ValidateDateLine(...) for  dateHeaderline: " + dateHeaderline);

            errMsg = string.Empty;
            string[] formats = { "MM/dd/yyyy HH:mm", "MM/d/yyyy HH:mm", "M/dd/yyyy HH:mm", "M/d/yyyy HH:mm",
                                 "MM/dd/yyyy", "MM/d/yyyy", "M/dd/yyyy", "M/d/yyyy"};

            string[] fields = dateHeaderline.Split(new char[] { ',' });
            if (fields.Count() > 1 && fields.Count() < 4 && !string.IsNullOrEmpty(fields[0]) && fields[0].Trim().ToLower().Equals("date"))
            {
                if (fields.Count() > 2)
                {
                    fields[2] = fields[2].Trim();
                    if (!string.IsNullOrEmpty(fields[2]))
                    {
                        errMsg = "Invalid data " + fields[3] + ", is exists at the end of date-header";
                        return false;
                    }
                }

                
                string dateValue = fields[1].Trim();
                DateTime parsedDate;
                if (DateTime.TryParseExact(dateValue, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
                {
                    Logger.DEBUG("Date Header row: "+dateHeaderline+", is validated successfully!");
                    return true;
                }
                else
                {
                    errMsg = "Date Value:" + dateValue + ", is not valid to parse corectly in format [MM/DD/YYYY HH24:MI]!";
                    Logger.WARN(errMsg);
                }
            }
            else
            {
                errMsg = "Failed to validate date-header-row: "+dateHeaderline;
                Logger.WARN(errMsg);
            }
            return false;
        }

        private void EnableDisablePriceCheckMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (App.AppManager.GUILSEngine.DataManager.IsPriceCheckON != null)
            {
                GroupUserControl.EditChangeReason("Company: ["+ App.AppManager.Config.LSConnectionInfo.Company +"]");
                if (App.AppManager.GUILSEngine.DataManager.IsPriceCheckON == true)
                {
                    App.AppManager.GUILSEngine.UpdatePriceCheck(false);
                }
                else
                {
                    App.AppManager.GUILSEngine.UpdatePriceCheck(true);
                }
            }
        }


        private void AlphabetizeMenuItem_Checked(object sender, RoutedEventArgs e)
        {
            App.AppManager.GUILSEngine.DataManager.Alphabetize();
        }

    }
}
