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
using EZXLib;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for ImportErrorView.xaml
    /// </summary>
    public partial class ImportErrorView : Window
    {
        public ImportErrorView()
        {
            Logger.DEBUG("ImportErrorView()");
            InitializeComponent();
        }

        public void Init(List<string> errorMessage, int errorRowsCount, int goodRowsCount)
        {
            Logger.DEBUG("Init(List<string> errorMessage, int errorRowsCount, int goodRowsCount)");
            
            if (errorRowsCount > 0)
            {
                this.txtMessageTitle.Text = "" + goodRowsCount + " group(s)/account(s) "+(goodRowsCount == 1?"is":"are")+" able to process successfully.\n(There "+(errorRowsCount == 1?"is":"are") +" "+ errorRowsCount + " error(s), see below).";
            }

            List<ErrorMessageWrapperClass> errorMessageList = new List<ErrorMessageWrapperClass>();
            int sno = 0;
            foreach (string error in errorMessage)
            {
                sno++;
                ErrorMessageWrapperClass errorMsgObj = new ErrorMessageWrapperClass();
                errorMsgObj.ErrorMessage = error;
                errorMsgObj.SerialNumber = sno;
                errorMessageList.Add(errorMsgObj);
            }

            this.dgError.ItemsSource = errorMessageList;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private class ErrorMessageWrapperClass
        {
            private int serialNumber;
            private string errorMessage;

            public int SerialNumber
            {
                get { return serialNumber; }
                set { serialNumber = value; }
            }
            public string ErrorMessage
            {
                get { return errorMessage; }
                set { errorMessage = value; }
            }

        }


    }
}
