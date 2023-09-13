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
    }
}
