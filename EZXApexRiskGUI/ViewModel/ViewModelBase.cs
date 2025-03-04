using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using EZXLib;
using EZXWPFLibrary.Helpers;

namespace EZXApexRiskGUI.ViewModel
{
    public abstract class ViewModelBase : ObservableBase
    {
        Dispatcher currentDispatcher = null;

        protected ViewModelBase()
        {
            currentDispatcher = Dispatcher.CurrentDispatcher;
        }

        protected ApplicationManager AppManager => App.AppManager;

        public bool IsDesignMode => (bool)(DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue);

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
