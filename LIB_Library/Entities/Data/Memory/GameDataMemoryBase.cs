using LIB.Entities.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Entities.Data.Base.GameEnums;

namespace LIB.Entities.Data.Memory
{
    public class GameDataMemoryBase : GameDataBase
    {
        private int _cardsNumber;
        private int _errorsNumber;
        private int _points;
        private MemoryResult _gameResult;
        public GameDataMemoryBase()
        {
        }
        public GameDataMemoryBase(string gameGUID, 
            string userName, 
            DateTime gameDate,
            int cardsNumber,
            int errorNumber,
            int points,
            MemoryResult gameResult) 
            : base(gameGUID, userName, gameDate)
        {
            CardsNumber = cardsNumber;
            ErrorsNumber = errorNumber;
            Points = points;
            GameResult = gameResult;
        }

        public int CardsNumber
        {
            get => _cardsNumber;
            set => SetProperty(ref _cardsNumber, value);
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
        public MemoryResult GameResult
        {
            get => _gameResult;
            set => SetProperty(ref _gameResult, value);
        }
    }
}
