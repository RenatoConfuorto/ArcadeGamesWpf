using LIB.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Helpers
{
    public class GameFoldersHelper
    {
        /// <summary>
        /// Checks if the folder for saving data exists, if not creates it
        /// </summary>
        /// <returns>true if the directory exists, false if it's been created</returns>
        public static bool CheckDirectoryData()
        {
            bool result = false;
            string folderPath = Cnst.ApplicationFolderDataPath;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            else result = true;
            return result;
        }
    }
}
