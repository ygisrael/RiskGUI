using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EZXApexRiskGUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static ApplicationManager AppManager
        {
            get
            {
                if (App.Current == null)
                {
                    return null;
                }

                return App.Current.Resources["AppManager"] as ApplicationManager;
            }
        }

        public App()
        {
            Application.Current.DispatcherUnhandledException += new System.Windows.Threading.DispatcherUnhandledExceptionEventHandler(Current_DispatcherUnhandledException);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            AppManager.SaveSettings();
            base.OnExit(e);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.GotFocusEvent, new RoutedEventHandler(TextBox_SelectAllText));
            EventManager.RegisterClassHandler(typeof(TextBox), TextBox.PreviewMouseDownEvent, new MouseButtonEventHandler(TextBox_SelectivelyIgnoreMouseButton));
        }

        bool isShuttingDown = false;
        void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                if (isShuttingDown)
                    return; //dont do anything (otherwise, it would create cyclic unhandling)

                if (App.Current == null)
                    return;

                Exception exp = e.Exception;

                if (AppManager != null)
                {
                    AppManager.ThrowException(exp);
                    EZXLib.Logger.ERROR(exp.ToString());
                }
                e.Handled = true;
            }
            catch (Exception exp)
            {
                EZXLib.Logger.ERROR(exp.ToString());
            }
        }

        private void TextBox_SelectAllText(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        private void TextBox_SelectivelyIgnoreMouseButton(object sender, MouseButtonEventArgs e)
        {
            // If its a triple click, select all text for the user.
            if (e.ClickCount == 3)
            {
                TextBox_SelectAllText(sender, new RoutedEventArgs());
                return;
            }

            // Find the TextBox
            DependencyObject parent = e.OriginalSource as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
            }

            if (parent != null)
            {
                if (parent is TextBox)
                {
                    var textBox = (TextBox)parent;
                    if (!textBox.IsKeyboardFocusWithin)
                    {
                        // If the text box is not yet focussed, give it the focus and
                        // stop further processing of this click event.
                        textBox.Focus();
                        e.Handled = true;
                    }
                }
            }
        }
    }
}
