using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace EZX.LightspeedEngine.Entity
{
    public interface INodeEntity
    {
        string Id { get; set; }
        string Name { get; set; }        
        int DisplayIndex { get; set; }
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }
        bool IsInEditMode { get; set; }
        bool IsWaitingForServerResponse { get; set; }
        RiskSetting RiskSetting { get; set; }
    }
}
