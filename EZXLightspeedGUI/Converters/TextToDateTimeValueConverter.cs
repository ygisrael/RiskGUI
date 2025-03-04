using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace EZXLightspeedGUI.Converters
{
    public class TextToDateTimeValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            object obj = value;
            if (obj is string)
            {
                string valueText = obj.ToString();
                int secondPart = -1;
                int hourPart = -1;
                int minutePart = -1;
                if (!string.IsNullOrEmpty(valueText))
                {
                    List<string> timeTextList = valueText.Split(':').ToList();
                    if (timeTextList.Count > 0)
                    {
                        string hourText = timeTextList[0].Trim();
                        if (Int32.TryParse(hourText, out hourPart))
                        {
                            if (hourPart >= 0 && hourPart < 24)
                            {
                                string minuteText = timeTextList[1].Trim();
                                if (Int32.TryParse(minuteText, out minutePart))
                                {
                                    if (minutePart >= 0 && minutePart < 60)
                                    {
                                        if (timeTextList.Count == 2)
                                        {
                                            DateTime tempDateTime = new DateTime(2000, 01, 01, hourPart, minutePart, 0);
                                        }
                                        else if (timeTextList.Count > 2)
                                        {
                                            string secondsText = timeTextList[2].Trim();
                                            if (Int32.TryParse(secondsText, out secondPart))
                                            {
                                                DateTime tempDateTime = new DateTime(2000, 01, 01, hourPart, minutePart, secondPart);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    return null;
                                }
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }

            return obj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            object obj = value;
            if (obj != null)
            {
                if (obj is DateTime)
                {
                    DateTime tempDateTime = (DateTime)obj;
                    return tempDateTime.Hour.ToString("00") + ":" + tempDateTime.Minute.ToString("00") + ":" + tempDateTime.Second.ToString("00");
                }
            }
            return obj;
        }
    }
}
