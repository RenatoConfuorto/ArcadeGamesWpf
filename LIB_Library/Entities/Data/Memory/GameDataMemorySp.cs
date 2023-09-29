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
    public class GameDataMemorySp : GameDataBase
    {
        private int _cardsNumber;
        private MemorySpDifficulty _gameDifficulty;
        private int _errorsLimit;
        private int _errorsNumber;
        private int _points;
        private MemorySpResult _gameResult;
        public GameDataMemorySp(string userName,
            DateTime gameDate,
            int cardsNumber,
            MemorySpDifficulty gameDifficulty,
            int errorsLimit,
            int errorsNumber,
            int point,
            MemorySpResult gameResult) 
            : base(Cnst.GAME_GUID_MEMORY_SP, userName, gameDate)
        {
            CardsNumber = cardsNumber;
            GameDifficulty = gameDifficulty;
            ErrorsLimit = errorsLimit;
            ErrorsNumber = errorsNumber;
            Points = point;
            GameResult = gameResult;
        }
        public int CardsNumber
        {
            get => _cardsNumber;
            set => SetProperty(ref _cardsNumber, value);
        }
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
        public int ErrorsNumber
        {
            get => _errorsNumber;
            set => SetProperty(ref _errorsNumber, value);
        }
        public int Points
        {
            get => _points;
            set => SetProperty(ref _points, value);
        }
        public MemorySpResult GameResult
        {
            get => _gameResult;
            set => SetProperty(ref _gameResult, value);
        }
    }
}
