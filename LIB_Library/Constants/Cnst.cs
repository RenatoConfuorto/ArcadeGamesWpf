using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Constants
{
    public class Cnst
    {
        #region User Data
        public const string ApplicationFolderDataName = "ArcadeMania";
        public const string UserFileName = "_user";
        public static readonly string ApplicationFolderDataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\{ApplicationFolderDataName}";
        public static readonly string ApplicationFolderUsers = $"{ApplicationFolderDataPath}\\Users";
        public static readonly string UserGameDataFileName = "GameData.db";
        public static readonly string UserGameDataLocation = ApplicationFolderUsers + "\\{0}\\" + UserGameDataFileName; //{0} => userName
        #endregion

        #region Game GUID's
        public const string GAME_GUID_TRIS_SP       = "694ba78b-ea2b-4ce2-aeee-b9108d18d992";
        public const string GAME_GUID_TRIS_MP       = "0b5ac759-3f63-4ff6-a806-b826509b0f08";
        public const string GAME_GUID_SUPER_TRIS_MP = "3a372a70-0d32-45df-8be2-12798c56baf3";
        public const string GAME_GUID_MEMORY_SP     = "1936e138-aec7-468e-a7c1-1b992b435db9";
        public const string GAME_GUID_MEMORY_MP     = "68438254-9ea9-4510-8215-78e664795d29";
        #endregion
    }
}
