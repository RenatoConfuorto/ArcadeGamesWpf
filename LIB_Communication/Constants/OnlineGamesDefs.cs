using LIB_Com.Entities;
using LIB_Com.Entities.OnlineGameSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB_Com.Constants
{
    public class OnlineGamesDefs
    {
        #region Controls names
        public const string ONLINE_TRIS_SETTINGS = "OnlineTrisSettings";
        #endregion

        #region Game Settings Def Values
        public const int ONLINE_TRIS_DEF_PLAYER_TIME = 15;
        #endregion

        public static readonly List<OnlineGame> Games = new List<OnlineGame>()
        {
            new OnlineGame()
            {
                NameDisplay = "Tris",
                //SettingControlName = ONLINE_TRIS_SETTINGS, // No Control for Tris game
                GameId = 1,
                GameSettings = new OnlineTrisSettings()
                {
                    PlayersTime = ONLINE_TRIS_DEF_PLAYER_TIME
                }
            }
        };
    }
}
