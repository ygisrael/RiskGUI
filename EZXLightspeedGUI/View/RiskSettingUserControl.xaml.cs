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
using Microsoft.Windows.Controls;
using EZX.LightspeedEngine.Entity;
using EZXLib;

namespace EZXLightspeedGUI.View
{
    /// <summary>
    /// Interaction logic for RiskSettingUserControl.xaml
    /// </summary>
    public partial class RiskSettingUserControl : UserControl
    {
        public RiskSettingUserControl()
        {
            InitializeComponent();
        }

        private void IntegerUpDown_GotFocus(object sender, RoutedEventArgs e)
        {
            (sender as IntegerUpDown).SelectAllOnGotFocus = true;
        }

        private void TxtCreditLimit_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Logger.DEBUG("TxtCreditLimit_PreviewKeyDown: " + e.Key.ToString());
            CheckNumericValue(e);
            if (!e.Handled)
            {
                if (e.Key == Key.Return || e.Key == Key.Tab || e.Key == Key.OemBackTab)
                {
                    if (!CheckCreditLimit())
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void CheckNumericValue(KeyEventArgs e)
        {

            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.C)
            {
            }
            else if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.A)
            {
            }
            else
            {
                switch (e.Key)
                {
                    case Key.LeftCtrl:
                    case Key.RightCtrl:
                    case Key.Home:
                    case Key.End:
                    case Key.System:
                    case Key.Return:
                    case Key.Delete:
                    case Key.Tab:
                    case Key.OemBackTab:
                    case Key.Left:
                    case Key.Right:
                    case Key.D0:
                    case Key.D1:
                    case Key.D2:
                    case Key.D3:
                    case Key.D4:
                    case Key.D5:
                    case Key.D6:
                    case Key.D7:
                    case Key.D8:
                    case Key.D9:
                    case Key.NumLock:
                    case Key.NumPad0:
                    case Key.NumPad1:
                    case Key.NumPad2:
                    case Key.NumPad3:
                    case Key.NumPad4:
                    case Key.NumPad5:
                    case Key.NumPad6:
                    case Key.NumPad7:
                    case Key.NumPad8:
                    case Key.NumPad9:
                    case Key.Back:
                        break;
                    default:
                        e.Handled = true;
                        break;
                }
            }
        }

        private bool CheckCreditLimit()
        {
            if (this.TxtCreditLimit.Text == null || string.IsNullOrEmpty(this.TxtCreditLimit.Text))
            {
                System.Windows.MessageBox.Show("Could not set blank value in Credit Limit!", "Credit Limit value error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.TxtCreditLimit.Text = (this.TxtCreditLimit.DataContext as RiskSetting).CreditLimit.ToString();
                return false;
            }
            else
            {
                try
                {
                    long testCreditLimitValue = Int64.Parse(this.TxtCreditLimit.Text.Trim().Replace(",",""));
                    if (testCreditLimitValue > 20000000000)
                    {
                        System.Windows.MessageBox.Show("Could not set value more than 20 Billion in Credit Limit!", "Credit Limit value error", MessageBoxButton.OK, MessageBoxImage.Error);
                        this.TxtCreditLimit.Text = (this.TxtCreditLimit.DataContext as RiskSetting).CreditLimit.ToString();
                        return false;
                    }

                    (this.TxtCreditLimit.DataContext as EZX.LightspeedEngine.Entity.RiskSetting).CreditLimit = testCreditLimitValue;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Could not set value: [" + this.TxtCreditLimit.Text + "], in Credit Limit!", "Credit Limit value error", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.TxtCreditLimit.Text = (this.TxtCreditLimit.DataContext as RiskSetting).CreditLimit.ToString();
                    return false;
                }
            }
            return true;
        }

        private void TxtCreditLimit_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (!CheckCreditLimit())
            {
                e.Handled = true;
            }
        }

        private void TxtCreditLimit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveRiskSetting();
            }
        }

        private void SaveRiskSetting()
        {
            (((this.Parent as Grid).Parent as GroupBox).Parent as EZXLightspeedGUI.View.GroupUserControl).SaveRiskSetting();
        }

        private void IntegerUpDown_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveRiskSetting();
            }
        }

        private void IntegerUpDown_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Logger.DEBUG("IntegerUpDown_PreviewKeyDown: " + e.Key.ToString());
            CheckNumericValue(e);
        }

        private void cmbWashCheck_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveRiskSetting();
            }
        }
    }
}
