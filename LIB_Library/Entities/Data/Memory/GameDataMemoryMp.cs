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
        private int _opponentsNumber;
        public GameDataMemoryMp() { }

        public GameDataMemoryMp(string userName, 
            DateTime gameDate,
            int cardsNumber,
            int errorNumber,
            int errorLimit,
            int points,
            GameEnums.MemoryResult gameResult,
            int opponentsNumber)
            : base(Cnst.GAME_GUID_MEMORY_MP, userName, gameDate, cardsNumber, errorNumber, errorLimit, points, gameResult)
        {
            OpponentsNumber = opponentsNumber;
        }

        public int OpponentsNumber
        {
            get => _opponentsNumber;
            set => SetProperty(ref _opponentsNumber, value);
        }
    }
}
