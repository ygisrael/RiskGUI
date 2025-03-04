using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace EZXLightspeedGUI.ViewModel
{
    public class MainViewVM :ViewModelBase
    {
        private string applicationTitle;

        public string ApplicationTitle
        {
            get { return applicationTitle; }
            set 
            { 
                applicationTitle = value;
                this.RaisePropertyChanged("ApplicationTitle");
            }
        }

        public MainViewVM()
            : base()
        {
            this.ApplicationTitle = string.Format(Properties.Resources.ApplicationTitle, Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }
    }
}
