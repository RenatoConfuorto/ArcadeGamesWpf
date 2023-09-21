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
    public class GameDataTrisSp : GameDataTrisBase
    {
        public GameDataTrisSp(string userName, 
            DateTime gameDate,
            TrisResults gameResults) 
            : base(Cnst.GAME_GUID_TRIS_SP, 
                  userName, 
                  gameDate, 
                  gameResults)
        {
        }
    }
}
