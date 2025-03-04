using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using EZXLib;
using System.ComponentModel;
using EZXWPFLibrary.Helpers;
using System.Windows;

namespace EZXLightspeedGUI.ViewModel
{
    public abstract class ViewModelBase : ObservableBase
    {
        Dispatcher currentDispatcher = null;

        public ViewModelBase()
        {
            currentDispatcher = Dispatcher.CurrentDispatcher;
        }

        protected ApplicationManager AppManager
        {
            get 
            { 
                return App.AppManager; 
            }
        }

        public bool IsDesignMode
        {
            get 
            { 
                return (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue); 
            }
        }

        public void RunOnDispatcherThread(Action action)
        {
            Logger.DEBUG("ViewModelBase.RunOnDispatcherThread()...");

            Dispatcher dispatcherToRun = currentDispatcher; //try to get VM's dispatcher
            if (currentDispatcher == null)
                dispatcherToRun = App.Current.Dispatcher; //get from main applicaiton

            //invoke
            dispatcherToRun.Invoke(action, null);
        }


    }

}
