using Core.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Interfaces.Entities
{
    public interface IOnlineSettingsBase : ISerializableBase
    {
        int PlayersTime { get; set; }
        string SettingsPopupName { get; set; }
    }
}
