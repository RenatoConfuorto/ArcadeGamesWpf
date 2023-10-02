using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static LIB.Entities.Data.Base.GameEnums;

namespace MemoryGame.Common.Entities
{
    public class MemorySingleplayerSettings : MemorySettingsBase
    {
        private MemorySpDifficulty _gameDifficulty;
        private int _errorsLimit;

        public MemorySpDifficulty GameDifficulty
        {
            get => _gameDifficulty;
            set => SetProperty(ref _gameDifficulty, value);
        }
        public int ErrorsLimit
        {
            get => _errorsLimit;
            set => SetProperty(ref _errorsLimit, value);
        }
    }
}
