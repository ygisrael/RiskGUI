using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EZXLib;
using EZXWPFLibrary.Helpers;

namespace EZX.LightspeedEngine.Entity
{
    public class RiskSetting : ObservableBase, ICloneable
    {
        private double creditLimit; //CREDIT_LIMIT_INT = "CL";
        private double clientCreditLimit; //CLIENT_CREDIT_LIMIT_DBL = "CCL";
        private int aggregateExecutedAmount; //MAX_AGGREGATED_EXECUTE_INT = "AG_EX";
        private int maxNetTraded;//MAX_NET_TRADE_INT = "NET_EX";
        private int maxSharesPerOrder;//MAX_SHARES_LIMIT_INT = "MS";
        private int maxSharesPerOptionOrder;//MAX_SHARES_LIMIT_INT = "MS_OPT";
        private int maxNotionalPerOrder; //MAX_NOTIONAL_INT = "MAX_DOLLAR";
        private int maxDuplicateOrder;//MAX_DUPES_CSV = "MAX_DUPES";
        private int duplicateOrderTimeInterval;//MAX_DUPES_CSV = "MAX_DUPES";
        private int maxPriceDiff;
        private bool? mocLocAllowed; //ON_CLOSE_CSV = "OC"; Is Nulable as need to track if this field is comming from server or not
        private string latestTime;////ON_CLOSE_CSV = "OC";
        private bool? enableSellShort;//ALLOW_SELL_SHORT_BOOL = "EN_SS";  Is Nulable as need to track if this field is comming from server or not
        private bool? enableSellShortExempt;//ALLOW_SELL_SHORT_EXEMP_BOOL = "EN_SSE";  Is Nulable as need to track if this field is comming from server or not

        private int maxNotionalPerOptionOrder; //MAX_NOTIONAL_OPT_INT = "MAX_DOLLAR_OPT";        
        private int maxOpenOrders; //MAX_OPEN_ORDERS_INT = "MOO";


        private bool isValueSetInCreditLimit;
        private bool isValueSetInClientCreditLimit;
        private bool isValueSetInAggregateExecutedAmount;
        private bool isValueSetInMaxNetTraded;
        private bool isValueSetInMaxSharesPerOrder;
        private bool isValueSetInMaxSharesPerOptionOrder;
        private bool isValueSetInMaxNotionalPerOrder;
        private bool isValueSetInMaxDuplicateOrder;
        private bool isValueSetInDuplicateOrderTimeInterval;
        private bool isValueSetInMaxPriceDiff;
        private bool isValueSetInLatestTime;
        private bool isValueSetInMocLocAllowed;
        private bool isValueSetInEnableSellShort;
        private bool isValueSetInEnableSellShortExempt;
        private bool isValueSetInEnableSellShortOrSellShortExempt;

        private bool isValueSetInMaxNotionalPerOptionOrder;
        private bool isValueSetInMaxOpenOrders;


        private bool isSettingExists;

        public bool IsSettingExists
        {
            get => isSettingExists;
            set
            {
                isSettingExists = value;
                RaisePropertyChanged("IsSettingExists");
            }
        }

        public double ClientCreditLimit
        {
            get => clientCreditLimit;
            set
            {                
                clientCreditLimit = value;
                RaisePropertyChanged("CreditLimit");
                IsValueSetInClientCreditLimit = !IsDirtyValue(value);
            }
        }

        public double CreditLimit
        {
            get => creditLimit;
            set
            {                
                creditLimit = value;
                RaisePropertyChanged("CreditLimit");
                IsValueSetInCreditLimit = !IsDirtyValue(value);
            }
        }

        public int AggregateExecutedAmount
        {
            get => aggregateExecutedAmount;
            set
            {
                value = int.MaxValue;
                aggregateExecutedAmount = value;
                RaisePropertyChanged("AggregateExecutedAmount");
                IsValueSetInAggregateExecutedAmount = !IsDirtyValue(value);

            }
        }

        public int MaxNetTraded
        {
            get => maxNetTraded;
            set
            {
                value = int.MaxValue;
                maxNetTraded = value;
                RaisePropertyChanged("MaxNetTraded");
                IsValueSetInMaxNetTraded = !IsDirtyValue(value);
            }
        }

        public int MaxSharesPerOrder
        {
            get => maxSharesPerOrder;
            set
            {
                maxSharesPerOrder = value;
                RaisePropertyChanged("MaxSharesPerOrder");
                IsValueSetInMaxSharesPerOrder = !IsDirtyValue(value);
            }
        }

        public int MaxSharesPerOptionOrder
        {
            get => maxSharesPerOptionOrder;
            set
            {
                maxSharesPerOptionOrder = value;
                RaisePropertyChanged("MaxSharesPerOptionOrder");
                IsValueSetInMaxSharesPerOptionOrder = !IsDirtyValue(value);
            }
        }
        public int MaxNotionalPerOrder
        {
            get => maxNotionalPerOrder;
            set
            {
                maxNotionalPerOrder = value;
                RaisePropertyChanged("MaxNotionalPerOrder");
                IsValueSetInMaxNotionalPerOrder = !IsDirtyValue(value);
            }
        }

        public bool? MocLocAllowed
        {
            get => mocLocAllowed;
            set
            {
                value = null;
                mocLocAllowed = value;
                RaisePropertyChanged("MocLocAllowed");
                IsValueSetInMocLocAllowed = !IsDirtyValue(value);
            }
        }

        public string LatestTime
        {
            get => latestTime;
            set
            {
                value = null;
                latestTime = value;
                RaisePropertyChanged("LatestTime");
                IsValueSetInLatestTime = !IsDirtyValue(value);

            }
        }

        public bool? EnableSellShort
        {
            get => enableSellShort;
            set
            {
                value = null;
                enableSellShort = value;
                RaisePropertyChanged("EnableSellShort");
                IsValueSetInEnableSellShort = !IsDirtyValue(value);

            }
        }

        public bool? EnableSellShortExempt
        {
            get => enableSellShortExempt;
            set
            {
                value = null;
                enableSellShortExempt = value;
                RaisePropertyChanged("EnableSellShortExempt");
                IsValueSetInEnableSellShortExempt = !IsDirtyValue(value);
            }
        }

        public int MaxDuplicateOrder
        {
            get => maxDuplicateOrder;
            set
            {
                maxDuplicateOrder = value;
                RaisePropertyChanged("MaxDuplicateOrder");
                IsValueSetInMaxDuplicateOrder = !IsDirtyValue(value);
            }
        }

        public int DuplicateOrderTimeInterval
        {
            get => duplicateOrderTimeInterval;
            set
            {
                duplicateOrderTimeInterval = value;
                RaisePropertyChanged("DuplicateOrderTimeInterval");
                IsValueSetInDuplicateOrderTimeInterval = !IsDirtyValue(value);
            }
        }

        public int MaxPriceDiff
        {
            get => maxPriceDiff;
            set
            {
                maxPriceDiff = value;
                RaisePropertyChanged("MaxPriceDiff");
                IsValueSetInMaxPriceDiff = !IsDirtyValue(value);
            }
        }

        public bool IsValueSetInCreditLimit
        {
            get => isValueSetInCreditLimit;
            set
            {
                isValueSetInCreditLimit = value;
                RaisePropertyChanged("IsValueSetInCreditLimit");
            }
        }

        public bool IsValueSetInClientCreditLimit
        {
            get => isValueSetInClientCreditLimit;
            set
            {
                isValueSetInClientCreditLimit = value;
                RaisePropertyChanged("IsValueSetInClientCreditLimit");
            }
        }

        public bool IsValueSetInAggregateExecutedAmount
        {
            get => isValueSetInAggregateExecutedAmount;
            set
            {
                isValueSetInAggregateExecutedAmount = value;
                RaisePropertyChanged("IsValueSetInAggregateExecutedAmount");
            }
        }

        public bool IsValueSetInMaxNetTraded
        {
            get 
            {
                return false;
                //return isValueSetInMaxNetTraded; 
            }
            set
            {
                isValueSetInMaxNetTraded = value;
                RaisePropertyChanged("IsValueSetInMaxNetTraded");
            }
        }

        public bool IsValueSetInMaxSharesPerOrder
        {
            get => isValueSetInMaxSharesPerOrder;
            set
            {
                isValueSetInMaxSharesPerOrder = value;
                RaisePropertyChanged("IsValueSetInMaxSharesPerOrder");
            }
        }

        public bool IsValueSetInMaxSharesPerOptionOrder
        {
            get => isValueSetInMaxSharesPerOptionOrder;
            set
            {
                isValueSetInMaxSharesPerOptionOrder = value;
                RaisePropertyChanged("IsValueSetInMaxSharesPerOptionOrder");
            }
        }

        public bool IsValueSetInMaxNotionalPerOrder
        {
            get => isValueSetInMaxNotionalPerOrder;
            set
            {
                isValueSetInMaxNotionalPerOrder = value;
                RaisePropertyChanged("IsValueSetInMaxNotionalPerOrder");
            }
        }

        public bool IsValueSetInMaxDuplicateOrder
        {
            get => isValueSetInMaxDuplicateOrder;
            set
            {
                isValueSetInMaxDuplicateOrder = value;
                RaisePropertyChanged("IsValueSetInMaxDuplicateOrder");
            }
        }

        public bool IsValueSetInDuplicateOrderTimeInterval
        {
            get => isValueSetInDuplicateOrderTimeInterval;
            set
            {
                isValueSetInDuplicateOrderTimeInterval = value;
                RaisePropertyChanged("IsValueSetInDuplicateOrderTimeInterval");
            }
        }

        public bool IsValueSetInMaxPriceDiff
        {
            get => isValueSetInMaxPriceDiff;
            set
            {
                isValueSetInMaxPriceDiff = value;
                RaisePropertyChanged("IsValueSetInMaxPriceDiff");
            }
        }

        public bool IsValueSetInLatestTime
        {
            get => isValueSetInLatestTime;
            set
            {
                isValueSetInLatestTime = value;
                RaisePropertyChanged("IsValueSetInLatestTime");
            }
        }

        public bool IsValueSetInMocLocAllowed
        {
            get => isValueSetInMocLocAllowed;
            set
            {
                isValueSetInMocLocAllowed = value;
                RaisePropertyChanged("IsValueSetInMocLocAllowed");
            }
        }

        public bool IsValueSetInEnableSellShort
        {
            get => isValueSetInEnableSellShort;
            set
            {
                isValueSetInEnableSellShort = value;
                RaisePropertyChanged("IsValueSetInEnableSellShort");
                if (IsValueSetInEnableSellShortExempt || IsValueSetInEnableSellShortExempt)
                {
                    IsValueSetInEnableSellShortOrSellShortExempt = true;
                }
            }
        }

        public bool IsValueSetInEnableSellShortExempt
        {
            get => isValueSetInEnableSellShortExempt;
            set
            {
                isValueSetInEnableSellShortExempt = value;
                RaisePropertyChanged("IsValueSetInEnableSellShortExempt");
                if (IsValueSetInEnableSellShortExempt || IsValueSetInEnableSellShortExempt)
                {
                    IsValueSetInEnableSellShortOrSellShortExempt = true;
                }
            }
        }

        public bool IsValueSetInEnableSellShortOrSellShortExempt
        {
            get => isValueSetInEnableSellShortOrSellShortExempt;
            set
            {
                isValueSetInEnableSellShortOrSellShortExempt = value;
                RaisePropertyChanged("IsValueSetInEnableSellShortOrSellShortExempt");
            }
        }

        
        //Wash Check Properties
        private string optionsWashCheck;    //WASH_CHECK_CSV
        private string equitiesWashCheck;   //WASH_CHECK_CSV
        

        public string OptionsWashCheck
        {
            get => optionsWashCheck;
            set
            {
                optionsWashCheck = value;
                RaisePropertyChanged("OptionsWashCheck");
            }
        }
        public string EquitiesWashCheck
        {
            get => equitiesWashCheck;
            set
            {
                equitiesWashCheck = value;
                RaisePropertyChanged("EquitiesWashCheck");
            }
        }

        public int MaxNotionalPerOptionOrder
        {
            get => maxNotionalPerOptionOrder;
            set 
            {
                maxNotionalPerOptionOrder = value;
                RaisePropertyChanged("MaxNotionalPerOptionOrder");
                IsValueSetInMaxNotionalPerOptionOrder = !IsDirtyValue(value);
            }
        }

        public int MaxOpenOrders
        {
            get => maxOpenOrders;
            set
            {
                maxOpenOrders = value;
                RaisePropertyChanged("MaxOpenOrders");
                IsValueSetInMaxOpenOrders = !IsDirtyValue(value);
            }
        }

        public bool IsValueSetInMaxNotionalPerOptionOrder
        {
            get => isValueSetInMaxNotionalPerOptionOrder;
            set
            {
                isValueSetInMaxNotionalPerOptionOrder = value;
                RaisePropertyChanged("IsValueSetInMaxNotionalPerOptionOrder");
            }
        }

        public bool IsValueSetInMaxOpenOrders
        {
            get => isValueSetInMaxOpenOrders;
            set
            {
                isValueSetInMaxOpenOrders = value;
                RaisePropertyChanged("IsValueSetInMaxOpenOrders");
            }
        }

        private void SetDefault()
        {
            CreditLimit = 0;
            AggregateExecutedAmount = int.MaxValue;
            MaxNetTraded = int.MaxValue;
            MaxNotionalPerOrder = 0;
            MaxPriceDiff = 1;
            MaxDuplicateOrder = 0;
            DuplicateOrderTimeInterval = 0;
            EnableSellShort = null;
            EnableSellShortExempt = null;
            LatestTime = null;
            MocLocAllowed = null;

            OptionsWashCheck = WashTradeCheck.PRICE_ONLY_CHECK;
            EquitiesWashCheck = WashTradeCheck.PRICE_ONLY_CHECK;

            MaxSharesPerOrder = 0;
            MaxSharesPerOptionOrder = 0;
            MaxNotionalPerOptionOrder = 0;
            MaxOpenOrders = 0;
        }

        public RiskSetting()
            : this(new Hashtable())
        {
        }

        public RiskSetting(Hashtable riskSetting)
        {
            SetDefault();
            IsSettingExists = false;
            if (riskSetting != null)
            {
                foreach (string key in riskSetting.Keys)
                {
                    IsSettingExists = true;
                    string keyValue = riskSetting[key] == null ? string.Empty : riskSetting[key].ToString();
                    SetClientCompanySetting(key, keyValue);
                }
            }
        }

        private void SetClientCompanySetting(string key, string keyValue)
        {
            switch (key)
            {
                //Not sure why this isn't in and CCL is set to CreditLimit??
                //case CompanySetting.CREDIT_LIMIT_DBL:
                //    CreditLimit = GetINTValueFromText(keyValue);
                //    break;
                case CompanySetting.CLIENT_CREDIT_LIMIT_DBL:
                   // ClientCreditLimit = GetINTValueFromText(keyValue);
                    CreditLimit = GetINTValueFromText(keyValue);
                    break;
                case CompanySetting.MAX_AGGREGATED_EXECUTE_INT:
                    AggregateExecutedAmount = GetINTValueFromText(keyValue);
                    break;
                case CompanySetting.MAX_NET_TRADE_INT:
                    MaxNetTraded = GetINTValueFromText(keyValue);
                    break;
                case CompanySetting.MAX_NOTIONAL_INT:
                    MaxNotionalPerOrder = GetINTValueFromText(keyValue);
                    break;
                case CompanySetting.MAX_SHARES_LIMIT_INT:
                    MaxSharesPerOrder = GetINTValueFromText(keyValue);
                    break;
                case CompanySetting.MAX_SHARES_LIMIT_OPT_INT:
                    MaxSharesPerOptionOrder = GetINTValueFromText(keyValue);
                    break;
                case CompanySetting.MAX_PRICE_DIFF_PCT:
                    MaxPriceDiff = GetINTValueFromText(keyValue);
                    break;
                case CompanySetting.ALLOW_SELL_SHORT_BOOL:
                    EnableSellShort = GetBoolValueFromText(keyValue);
                    break;
                case CompanySetting.ALLOW_SELL_SHORT_EXEMP_BOOL:
                    EnableSellShortExempt = GetBoolValueFromText(keyValue);
                    break;
                case CompanySetting.MAX_DUPES_CSV:
                    SetValueIntoDuplicatePropertiesFromText(keyValue);
                    break;
                case CompanySetting.ON_CLOSE_CSV:
                    SetValueIntoMocLocPropertiesFromText(keyValue);
                    break;
                case CompanySetting.WASH_TRADE_CHECK_CSV:
                    SetValueInWashCheckPropertiesFromText(keyValue);
                    break;
                case CompanySetting.MAX_NOTIONAL_OPT_INT:
                    MaxNotionalPerOptionOrder = GetINTValueFromText(keyValue);
                    break;
                case CompanySetting.MAX_OPEN_ORDERS_INT:
                    MaxOpenOrders = GetINTValueFromText(keyValue);
                    break;
            }
        }

        private void SetValueIntoMocLocPropertiesFromText(string keyValueCSV)
        {
            List<string> mocLocPropertyList = new List<string>();
            mocLocPropertyList = GetTextListFromCVSText(keyValueCSV, mocLocPropertyList, 2);


            bool val = false;
            if (Boolean.TryParse(mocLocPropertyList[0], out val))
            {
                MocLocAllowed = val;
            }

            if (MocLocAllowed == true)
            {
                LatestTime = mocLocPropertyList[1];
            }
            else
            {
                LatestTime = string.Empty;
            }
        }

        private void SetValueIntoDuplicatePropertiesFromText(string keyValueCSV)
        {
            List<string> duplicatePropertyList = new List<string>();
            duplicatePropertyList = GetTextListFromCVSText(keyValueCSV, duplicatePropertyList, 2);

            double val = int.MaxValue;

            if (Double.TryParse(duplicatePropertyList[0].Replace(",", "").Trim(), out val))
            {
                MaxDuplicateOrder = (int)val;
            }
            else
            {
                MaxDuplicateOrder = int.MaxValue;
            }

            val = int.MaxValue;
            if (Double.TryParse(duplicatePropertyList[1].Replace(",", "").Trim(), out val))
            {
                DuplicateOrderTimeInterval = (int)val;
            }
            else
            {
                DuplicateOrderTimeInterval = int.MaxValue;
            }

        }

        private void SetValueInWashCheckPropertiesFromText(string keyValueCSV)
        {
            List<string> washCheckPropertyList = new List<string>();
            washCheckPropertyList = GetTextListFromCVSText(keyValueCSV, washCheckPropertyList, 2);
            OptionsWashCheck = GetWashCheckValue(washCheckPropertyList, false);
            EquitiesWashCheck = GetWashCheckValue(washCheckPropertyList, true);                   
        }

        private string GetWashCheckValue(List<string> washCheckValueList, bool IsEquityTypeValue)
        {
            if (washCheckValueList == null || washCheckValueList.Count == 0)
            {
                Logger.ERROR("GetWashCheckValue(List<string> washCheckValueList):  washCheckValueList is null or empty!");
                return WashTradeCheck.PRICE_ONLY_CHECK;
            }

            string washCheckValue = WashTradeCheck.PRICE_ONLY_CHECK;
            string lookUpCheck = "OPT=";
            if (IsEquityTypeValue)
            {
                lookUpCheck = "CS=";
            }

            foreach (string val in washCheckValueList)
            {
                if (val.Trim().StartsWith(lookUpCheck))
                {
                    washCheckValue = val.Replace(lookUpCheck, "");
                    break;
                }
            }

            if (!string.IsNullOrEmpty(washCheckValue) &&
                (washCheckValue.Equals(WashTradeCheck.NO_PRICE_CHECK)
                || washCheckValue.Equals(WashTradeCheck.PRICE_ONLY_CHECK)
                || washCheckValue.Equals(WashTradeCheck.PRICE_PLUS_DESTINATION)))
            {
                return washCheckValue;
            }

            Logger.WARN("GetWashCheckValue(string washCheckValue): Found washCheckValue:" + washCheckValue);
            return WashTradeCheck.PRICE_ONLY_CHECK;
        }

        private List<string> GetTextListFromCVSText(string keyValueCSV, List<string> propertyList, int numberOfPropertyToGetFromCSV)
        {
            for (int i = 0; i < numberOfPropertyToGetFromCSV; i++)
            {
                propertyList.Add("");
            }

            if (keyValueCSV != null)
            {
                keyValueCSV = keyValueCSV.Trim();
                List<string> textList = keyValueCSV.Split(',').ToList();
                if (textList != null)
                {
                    for (int index = 0; (index < textList.Count && index < propertyList.Count); index++)
                    {
                        if (propertyList != null)
                        {
                            propertyList[index] = GetTextFromList(textList, index);
                        }
                    }
                }
            }
            return propertyList;
        }

        private string GetTextFromList(List<string> csvTextList, int index)
        {
            if (csvTextList != null && csvTextList.Count > index)
            {
                return csvTextList[index].Trim();
            }
            return string.Empty;
        }

        private bool? GetBoolValueFromText(string keyValue)
        {
            if (keyValue != null && !keyValue.ToLower().Trim().Equals("null"))
            {
                bool val = false;
                keyValue = keyValue.ToLower().Trim();
                if (keyValue.Length > 0)
                {
                    keyValue = char.ToUpper(keyValue[0]) + keyValue.Substring(1);
                }

                keyValue = keyValue.Trim();
                Boolean.TryParse(keyValue, out val);
                if (keyValue.Equals("1"))
                {
                    val = true;
                }
                return val;
            }
            return null;
        }

        private int GetINTValueFromText(string keyValue)
        {
            double val = int.MaxValue;
            if (!string.IsNullOrEmpty(keyValue))
            {
                keyValue = keyValue.Replace(",", "").Replace("$", "").Trim();
                if (!Double.TryParse(keyValue, out val))
                {
                    val = int.MaxValue;
                }
            }
            return (int)val;
        }

        public Hashtable GetUpdatedRiskSetting()
        {
            Hashtable clientCompanySetting = new Hashtable();

            if (IsValueSetInCreditLimit)
            {
                clientCompanySetting.Add(CompanySetting.CREDIT_LIMIT_DBL, CreditLimit.ToString());
            }

            if (IsValueSetInAggregateExecutedAmount)
            {
                clientCompanySetting.Add(CompanySetting.MAX_AGGREGATED_EXECUTE_INT, AggregateExecutedAmount.ToString());
            }

            if (IsValueSetInMaxNetTraded)
            {
                clientCompanySetting.Add(CompanySetting.MAX_NET_TRADE_INT, MaxNetTraded.ToString());
            }

            if (IsValueSetInMaxNotionalPerOrder)
            {
                clientCompanySetting.Add(CompanySetting.MAX_NOTIONAL_INT, MaxNotionalPerOrder.ToString());
            }

            if (IsValueSetInMaxPriceDiff)
            {
                clientCompanySetting.Add(CompanySetting.MAX_PRICE_DIFF_PCT, MaxPriceDiff.ToString());
            }


            if (IsValueSetInMaxSharesPerOrder)
            {
                clientCompanySetting.Add(CompanySetting.MAX_SHARES_LIMIT_INT, MaxSharesPerOrder.ToString());
            }

            if (IsValueSetInMaxSharesPerOptionOrder)
            {
                clientCompanySetting.Add(CompanySetting.MAX_SHARES_LIMIT_OPT_INT, MaxSharesPerOptionOrder.ToString());
            }


            if (IsValueSetInMocLocAllowed)
            {
                string mocLocCSV = MocLocAllowed.ToString().ToLower() + "," + LatestTime;
                if (MocLocAllowed == null || MocLocAllowed == false)
                {
                    if (isValueSetInMocLocAllowed)
                    {
                        mocLocCSV = MocLocAllowed.ToString().ToLower() + ",";
                    }
                    else
                    {
                        mocLocCSV = string.Empty;
                    }
                }
                clientCompanySetting.Add(CompanySetting.ON_CLOSE_CSV, mocLocCSV);
            }

            if (IsValueSetInMaxDuplicateOrder)
            {
                string duplicateOrderCSV = string.Empty;
                if (MaxDuplicateOrder != int.MaxValue)
                {
                    duplicateOrderCSV = MaxDuplicateOrder.ToString();
                    if (DuplicateOrderTimeInterval != int.MaxValue)
                    {
                        duplicateOrderCSV = duplicateOrderCSV + "," + DuplicateOrderTimeInterval;
                    }
                    else
                    {
                        duplicateOrderCSV = duplicateOrderCSV + ",0"; 
                    }
                }
                clientCompanySetting.Add(CompanySetting.MAX_DUPES_CSV, duplicateOrderCSV);
            }

            if (IsValueSetInEnableSellShort)
            {
                if (EnableSellShort != null)
                {
                    clientCompanySetting.Add(CompanySetting.ALLOW_SELL_SHORT_BOOL, EnableSellShort.ToString().ToLower());
                }
                else
                {
                    clientCompanySetting.Add(CompanySetting.ALLOW_SELL_SHORT_BOOL, "");
                }
            }

            if (IsValueSetInEnableSellShortExempt)
            {
                if (EnableSellShortExempt != null)
                {
                    clientCompanySetting.Add(CompanySetting.ALLOW_SELL_SHORT_EXEMP_BOOL, EnableSellShortExempt.ToString().ToLower());
                }
                else
                {
                    clientCompanySetting.Add(CompanySetting.ALLOW_SELL_SHORT_BOOL, "");
                }
            }


            if (IsValueSetInMaxNotionalPerOptionOrder)
            {
                clientCompanySetting.Add(CompanySetting.MAX_NOTIONAL_OPT_INT, MaxNotionalPerOptionOrder.ToString());
            }

            if (isValueSetInMaxOpenOrders)
            {
                clientCompanySetting.Add(CompanySetting.MAX_OPEN_ORDERS_INT, MaxOpenOrders.ToString());
            }


            //Seting value in wash check setting to send to server
            string washCheckCSV = "CS="+(EquitiesWashCheck == null ? WashTradeCheck.PRICE_ONLY_CHECK : EquitiesWashCheck)
                                    + "," +
                                  "OPT=" + (OptionsWashCheck == null ? WashTradeCheck.PRICE_ONLY_CHECK : OptionsWashCheck); 
            clientCompanySetting.Add(CompanySetting.WASH_TRADE_CHECK_CSV, washCheckCSV);

            return clientCompanySetting;
        }

        public Properties GetUpdatedRiskSettingProperties()
        {
            Properties properties = new Properties();
            properties.PropertyMap = new TagValueMsg();
            properties.PropertyMap.tagValues = GetUpdatedRiskSetting();
            return properties;
        }

        private bool IsDirtyValue(double value)
        {
            if (value == Double.MaxValue)
            {
                return true;
            }
            return false;
        }

        private bool IsDirtyValue(int value)
        {
            if (value == int.MaxValue)
            {
                return true;
            }
            return false;
        }
        private bool IsDirtyValue(int? value)
        {
            if (value == null)
            {
                return false;
            }

            if (value == int.MaxValue)
            {
                return true;
            }
            return false;
        }
        private bool IsDirtyValue(bool? value)
        {
            if (value == null)
            {
                return true;
            }
            return false;
        }
        private bool IsDirtyValue(string value)
        {
            if (value == null)
            {
                return true;
            }
            return false;
        }

        public RiskSetting CloneRiskSetting()
        {
            return MemberwiseClone() as RiskSetting;
        }

        object ICloneable.Clone()
        {
            return CloneRiskSetting();
        }

        public bool CompareSetting(RiskSetting riskSetting)
        {
            if (CreditLimit != riskSetting.CreditLimit)
            {
                return false;
            }

            if (MaxDuplicateOrder != riskSetting.MaxDuplicateOrder)
            {
                return false;
            }

            if (DuplicateOrderTimeInterval != riskSetting.DuplicateOrderTimeInterval)
            {
                return false;
            }

            if (MaxNotionalPerOrder != riskSetting.MaxNotionalPerOrder)
            {
                return false;
            }

            if (MaxPriceDiff != riskSetting.MaxPriceDiff)
            {
                return false;
            }

            if (OptionsWashCheck != riskSetting.OptionsWashCheck)
            {
                return false;
            }

            if (EquitiesWashCheck != riskSetting.EquitiesWashCheck)
            {
                return false;
            }

            if (MaxNotionalPerOptionOrder != riskSetting.MaxNotionalPerOptionOrder)
            {
                return false;
            }

            if (MaxSharesPerOrder != riskSetting.MaxSharesPerOrder)
            {
                return false;
            }

            if (MaxSharesPerOptionOrder != riskSetting.MaxSharesPerOptionOrder)
            {
                return false;
            }

            if (MaxOpenOrders != riskSetting.MaxOpenOrders)
            {
                return false;
            }
            return true;
        }
    }

}
