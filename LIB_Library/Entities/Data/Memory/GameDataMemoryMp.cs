using LIB.Constants;
using LIB.Entities.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities.Data.Memory
{
    public class GameDataMemoryMp : GameDataMemoryBase
    {
        public GameDataMemoryMp() { }

        public GameDataMemoryMp(string userName, 
            DateTime gameDate, 
            int cardsNumber, 
            int errorNumber, 
            int points, 
            GameEnums.MemoryResult gameResult) 
            : base(Cnst.GAME_GUID_MEMORY_MP, userName, gameDate, cardsNumber, errorNumber, points, gameResult)
        {
        }
    }
}
