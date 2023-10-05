using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tris.Common.Entities
{
    public class SuperTrisSettings : GameSettingsBase
    {
        private int _playersTime;

        public int PlayersTime
        {
            get => _playersTime;
            set => SetProperty(ref _playersTime, value);
        }
        public SuperTrisSettings() { }

        public SuperTrisSettings(int time)
        {
            PlayersTime = time;
        }
    }
}
