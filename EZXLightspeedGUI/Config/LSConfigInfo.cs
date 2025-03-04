using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EZX.LightSpeedEngine.Config;
using System.Xml.Serialization;

namespace EZXLightspeedGUI.Config
{
    [XmlType(Namespace = "urn:EZXLightspeedGUI.Config")]
    [XmlRoot(Namespace = "urn:EZXLightspeedGUI.Config")]

    public class LSConfigInfo : ConfigInfo
    {
        public LSConfigInfo()
            : base()
        {
        }
    }
}
