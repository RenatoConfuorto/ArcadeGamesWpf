﻿using LIB.Entities;
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

        public MemorySpDifficulty GameDifficulty
        {
            get => _gameDifficulty;
            set => SetProperty(ref _gameDifficulty, value);
        }
        
    }
}
