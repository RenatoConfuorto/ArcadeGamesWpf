using LIB.Entities.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Entities.Data.Base.GameResults;

namespace LIB.Entities.Data.Base
{
    public class GameDataTrisBase : GameDataBase
    {
        private TrisResults _gameResults;

        public GameDataTrisBase(string gameGUID, 
            string userName, 
            DateTime gameDate,
            TrisResults gameResults) 
            : base(gameGUID, userName, gameDate)
        {
            GameResults = gameResults;
        }

        public TrisResults GameResults
        {
            get => _gameResults;
            set => SetProperty(ref _gameResults, value);
        }
    }
}
