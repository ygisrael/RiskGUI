using EZXLib;

namespace EZXApexRiskGUI.Model
{
    public class GroupAccountWPF : GroupAccount
    {
        public GroupAccountWPF(GroupAccount ga)
        {
            IsEnabled = ga.Settings.PropertyMap.tagValues["TRADEONOFF"].Equals(CompanySetting.TRADING_ENABLED);
            IsItemSelected = false;
            DisplayName = ga.DisplayName;
            Id = ga.Id;
            Settings = ga.Settings;
        }

        private bool isEnabled;

        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                isEnabled = value;
                NotifyPropertyChanged("IsEnabled");
            }
        }

        private bool isItemSelected;

        public bool IsItemSelected
        {
            get => isItemSelected;
            set
            {
                isItemSelected = value;
                NotifyPropertyChanged("IsItemSelected");
            }
        }
    }
}