using Core.Interfaces.ViewModels;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Attributes
{
    public class SettingsPopup : Attribute
    {
        private string _popupName;

        public string PopupName
        {
            get => _popupName;
            set => _popupName = value;
        }

        public SettingsPopup(string popupName)
        {
            PopupName = popupName;
        }
    }
}
