using LIB.Constants;
using LIB.Entities.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Entities.Data.Base.GameEnums;

namespace LIB.Entities.Data.Memory
{
    public class GameDataMemorySp : GameDataMemoryBase
    {
        private MemorySpDifficulty _gameDifficulty;
        public GameDataMemorySp()
        {
        }
        public GameDataMemorySp(string userName,
            DateTime gameDate,
            int cardsNumber,
            MemorySpDifficulty gameDifficulty,
            int errorsLimit,
            int errorsNumber,
            int point,
            MemoryResult gameResult) 
            : base(Cnst.GAME_GUID_MEMORY_SP, userName, gameDate, cardsNumber, errorsNumber, errorsLimit, point, gameResult)
        {
            GameDifficulty = gameDifficulty;
        }
        public MemorySpDifficulty GameDifficulty
        {
            get => _gameDifficulty;
            set => SetProperty(ref _gameDifficulty, value);
        }
    }
}
