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
    public class GameDataTrisMp : GameDataTrisBase
    {
        private string _opponentName;

        public GameDataTrisMp(string userName, 
            DateTime gameDate,
            TrisResults gameResults,
            string opponentName) 
            : base(Cnst.GAME_GUID_TRIS_MP,
                  userName, 
                  gameDate, 
                  gameResults)
        {
            OpponentName = opponentName;
        }

        public string OpponentName
        {
            get => _opponentName;
            set => SetProperty(ref _opponentName, value);
        }
    }
}
