using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using EZXLib;
using EZXWPFLibrary.Helpers;

namespace EZX.LightSpeedEngine.Config
{
    [XmlType(Namespace = "urn:EZX.LightSpeedEngine.Config")]
    [XmlRoot(Namespace = "urn:EZX.LightSpeedEngine.Config")]
    public class ConfigInfo
    {
        const string FILE_NAME = "ConfigInfo.xml";
        private static string baseDirectory;
        private static string fileNameWithPath;
        public LSConnectionInfo LSConnectionInfo { get; set; }


        public string Username { get; set; }
        public string Password { get; set; }

        public string SysLogHost { get; set; }
        public bool ApplyLogSettingForAllSession { get; set; }
        public bool ShowChangeSettingPopup { get; set; }

        public string lastLoginDate = "";

        public bool IsAlphabetize { get; set; }      

        public ConfigInfo()
        {
            Logger.DEBUG("ConfigInfo()...");

            LSConnectionInfo = new LSConnectionInfo();
        }

        public static string BaseDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(baseDirectory))
                {
                    baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "EZXLightspeedGUI" + Path.DirectorySeparatorChar + "Configuration";
                }
                return baseDirectory;
            }
        }

        public static string FileNameWithPath
        {
            get
            {
                if (string.IsNullOrEmpty(fileNameWithPath))
                {
                    fileNameWithPath = BaseDirectory + Path.DirectorySeparatorChar + FILE_NAME;
                }
                return fileNameWithPath;
            }
            set
            {
                fileNameWithPath = value;
            }
        }

    }
}
