using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Entities
{
    public class OnlineGame : NotifyerPropertyChangedBase
    {
        public string _nameDisplay;
        public string _settingControlName;
        public short _gameId;
        public OnlineSettingsBase _gameSettings;

        public string NameDisplay
        {
            get => _nameDisplay;
            set => SetProperty(ref _nameDisplay, value);
        }
        public string SettingControlName
        {
            get => _settingControlName;
            set => SetProperty(ref _settingControlName, value);
        }
        public short GameId
        {
            get => _gameId;
            set => SetProperty(ref _gameId, value);
        }
        public OnlineSettingsBase GameSettings
        {
            get => _gameSettings;
            set => SetProperty(ref _gameSettings, value);
        }
    }
}
