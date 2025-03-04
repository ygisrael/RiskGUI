using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using EZX.LightspeedEngine.Entity;

namespace EZXLightspeedGUI.ViewModel
{
    public class AccountUserControlVM : ViewModelBase
    {
        public ObservableCollection<INodeEntity> AccountAndGroupNode
        {
            get
            {
                return App.AppManager.GUILSEngine.DataManager.AccountGroupList;
            }
        }

        public AccountUserControlVM()
            : base()
        {
            Init();
        }

        private void Init()
        {
            //this.AccountAndGroupNode = App.AppManager.DataMgr.AccountGroupList;
        }


    }
}
