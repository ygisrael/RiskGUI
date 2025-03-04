using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZXLib;

namespace EZXLightspeedGUI.Model
{
    public class WashCheckComboData
    {
        public object Name { get; set; }
        public object Value { get; set; }

        public WashCheckComboData(object name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        public static string GetTextFormRersources(string key)
        {
            string text = string.Empty;
            switch (key)
            {
                case WashTradeCheck.NO_PRICE_CHECK:
                    text = "No Wash Check";
                    break;
                case WashTradeCheck.PRICE_ONLY_CHECK:
                    text = "Price Only";
                    break;
                case WashTradeCheck.PRICE_PLUS_DESTINATION:
                    text = "Price + Destination";
                    break;
                default:
                    text = "INVALID VALUE MAPPING ???";
                    break;
            }
            return text;
        }
    }
}
