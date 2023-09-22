using LIB.Constants;
using LIB.Entities.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LIB.Entities.Data.Base.GameResults;

namespace LIB.Entities.Data.Tris
{
    public class GameDataSuperTrisMp : GameDataTrisBase
    {
        private string _opponentName;
        private int _startingTime;
        private int _remainingTime;
        private int _cellsWon;
        public GameDataSuperTrisMp(string userName, 
            DateTime gameDate, 
            TrisResults gameResults,
            string opponenetName,
            int remainingTime,
            int cellsWon) 
            : base(Cnst.GAME_GUID_SUPER_TRIS_MP, userName, gameDate, gameResults)
        {
            OpponentName = opponenetName;
            RemainingTime = remainingTime;
            CellsWon = cellsWon;
        }

        public string OpponentName
        {
            get => _opponentName;
            set => SetProperty(ref _opponentName, value);
        }
        public int StartingTime
        {
            get => _startingTime;
            set => SetProperty(ref _startingTime, value);
        }
        public int RemainingTime
        {
            get => _remainingTime;
            set => SetProperty(ref _remainingTime, value);
        }
        public int CellsWon
        {
            get => _cellsWon;
            set => SetProperty(ref _cellsWon, value);
        }
    }
}
