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

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for ExceptionView.xaml
    /// </summary>
    public partial class ExceptionView : Window
    {
        public ExceptionView()
        {
            InitializeComponent();
        }

        public ExceptionView(Exception exp)
            : this()
        {
            this.txtMessage.Text = "Exception Occured: "+exp.Message;
            this.txtMessageDetail.Text = exp.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (this.Height < 450)
            {
                this.Height = 450;
            }
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.Height = 150;
        }
    }
}
