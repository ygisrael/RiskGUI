using EZXLib;
using System.Collections.ObjectModel;

namespace EZXApexRiskGUI.Model
{
    public class GroupAccountWPF_MockData
    {
        public static ObservableCollection<GroupAccountWPF> accountsList { get; private set; }

        public GroupAccountWPF_MockData()
        {
            accountsList = new ObservableCollection<GroupAccountWPF>
            {
                AddItem("Ted", "1", CompanySetting.TRADING_ENABLED),
                AddItem("Jack", "2", CompanySetting.TRADING_ENABLED),
                AddItem("Jill", "3", CompanySetting.TRADING_DISABLED),
                AddItem("Jane", "4", CompanySetting.TRADING_DISABLED),
                AddItem("Gail", "5", CompanySetting.TRADING_ENABLED),
                AddItem("Oscar", "6", CompanySetting.TRADING_DISABLED),
                AddItem("Peter", "7", CompanySetting.TRADING_ENABLED),
                AddItem("Olive", "8", CompanySetting.TRADING_ENABLED),
                AddItem("Donald", "9", CompanySetting.TRADING_DISABLED),
                AddItem("Raymond", "10", CompanySetting.TRADING_LOSS),
                AddItem("Stacy", "11", CompanySetting.TRADING_ENABLED),
                AddItem("Patricia", "12", CompanySetting.TRADING_DISABLED),
                AddItem("Janet", "13", CompanySetting.TRADING_ENABLED),
                AddItem("Steven", "14", CompanySetting.TRADING_ENABLED),
                AddItem("Sean", "15", CompanySetting.TRADING_DISABLED),                
                AddItem("Mildred", "16", CompanySetting.TRADING_DISABLED),
                AddItem("David", "17", CompanySetting.TRADING_ENABLED),
                AddItem("John", "18", CompanySetting.TRADING_ENABLED),
                AddItem("Mickey", "19", CompanySetting.TRADING_DISABLED),
                AddItem("Rebecca", "20", CompanySetting.TRADING_LOSS),
                AddItem("Audry", "21", CompanySetting.TRADING_ENABLED),
                AddItem("Avril", "22", CompanySetting.TRADING_DISABLED)
            };
        }

        private GroupAccountWPF AddItem(string _name, string _id, string isEnabled)
        {
            GroupAccount ga = new GroupAccount()
            {
                DisplayName = _name,
                Id = _id,
                Settings = new EZXLib.Properties()                
            };
            
            ga.Settings.PropertyMap = new TagValueMsg();
            ga.Settings.PropertyMap.tagValues = new System.Collections.Hashtable
            {
                ["TRADEONOFF"] = isEnabled
            };

            return new GroupAccountWPF(ga);

        }
    }

}
