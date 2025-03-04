using System;
using System.Windows;

namespace EZXApexRiskGUI.View
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
            txtMessage.Text = "Exception Occurred: "+exp.Message;
            txtMessageDetail.Text = exp.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            if (Height < 450)
            {
                Height = 450;
            }
        }

        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            Height = 150;
        }
    }
}
