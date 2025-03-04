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
using EZXLightspeedGUI.Model;
using EZX.LightspeedEngine.Entity;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for ImportDataView.xaml
    /// </summary>
    public partial class ImportDataView : Window
    {
        private List<NodeImportEntity> importEntityList;

        int countGroupAccount = 0;

        public List<NodeImportEntity> ImportEntityList
        {
            get { return importEntityList; }
            set { importEntityList = value; }
        }

        public ImportDataView()
        {
            InitializeComponent();
            App.AppManager.GUILSEngine.UpdateGroupAccountSettingCompleted += new EZX.LightSpeedEngine.UpdateGroupAccountSettingHandler(GUILSEngine_UpdateGroupAccountSettingCompleted);
        }

        bool isSubmitToServer = false;
        void GUILSEngine_UpdateGroupAccountSettingCompleted(object sender, EZX.LightSpeedEngine.GroupAccountEventArgs e)
        {
            NodeImportEntity nodeEntity = this.ImportEntityList.Where(obj => obj.GroupId.Equals(e.GroupAccount.Id)).FirstOrDefault();
            if (nodeEntity != null && isSubmitToServer)
            {
                nodeEntity.IsSubmitted = true;
                this.countGroupAccount--;

                if (this.countGroupAccount == 0)
                {
                    App.AppManager.RunOnDispatcherThread(() =>
                    {
                        MessageBox.Show("Data submit successfully.", "Submit Buying Power", MessageBoxButton.OK, MessageBoxImage.Information);
                        App.AppManager.GUILSEngine.UpdateGroupAccountSettingCompleted -= new EZX.LightSpeedEngine.UpdateGroupAccountSettingHandler(GUILSEngine_UpdateGroupAccountSettingCompleted);
                        this.Close();
                    });
                }
            }
        }

        public void Init(List<NodeImportEntity> _importEntityList)
        {            
            this.ImportEntityList = _importEntityList;
            this.txtMessageTitle.Text = importEntityList.Count.ToString() + " row(s) is(are) imported from file.\nTo modify Buying Power please press [Submit] button.";
            this.dgImportData.ItemsSource = ImportEntityList;
            this.countGroupAccount = this.ImportEntityList.Count;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.isSubmitToServer = false;
            this.Close();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            foreach (NodeImportEntity groupAccountNode in this.ImportEntityList)
            {
                groupAccountNode.NodeEntity.RiskSetting.CreditLimit = groupAccountNode.BuyingPower;
                App.AppManager.GUILSEngine.DataManager.UpdateRiskSetting(groupAccountNode.NodeEntity as Group);
            }

            (sender as Button).IsEnabled = false;
            isSubmitToServer = true;

        }

    }
}
