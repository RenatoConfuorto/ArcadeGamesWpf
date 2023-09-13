using LIB.Constants;
using LIB.Entities.Data.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Entities.Data.Tris
{
    public class GameDataTrisMp : GameDataTrisBase
    {
        private string _opponentName;

        public GameDataTrisMp(string userName, 
            DateTime gameDate, 
            bool userHasWon,
            string opponentName) 
            : base(Cnst.GAME_GUID_TRIS_MP,
                  userName, 
                  gameDate, 
                  userHasWon)
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
