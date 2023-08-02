using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Core.Helpers
{
    /// <summary>
    /// Methods collection to manipulate Paths
    /// </summary>
    public class PathHelper
    {
        /// <summary>
        /// Get the absolute path of the ModulesAssemblies folder or its childer
        /// </summary>
        /// <param name="folderNames">all the children folders to navigate</param>
        /// <returns></returns>
        public static string GetFullPathFromModulesAssembliesDir(params string[] folderNames)
        {
            string result = Environment.CurrentDirectory;
            if(folderNames.Length != 0) 
            {
                foreach(string name in folderNames)
                {
                    result += $"\\{name}";
                }
            }
            return result;
        }
    }
}
