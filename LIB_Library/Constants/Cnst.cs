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
        public static readonly string ApplicationFolderDataName = "ArcadeMania";
        public static readonly string ApplicationFolderDataPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + $"\\{ApplicationFolderDataName}";
        public static readonly string ApplicationFolderUsers = $"{ApplicationFolderDataPath}\\Users";
        #endregion
    }
}
