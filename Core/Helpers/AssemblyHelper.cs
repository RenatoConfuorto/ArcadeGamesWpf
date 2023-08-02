using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Core.Helpers
{
    /// <summary>
    /// Collection of methods to work with assemblies
    /// </summary>
    public class AssemblyHelper
    {
        /// <summary>
        /// Gets all the assemblies present in a folder
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Assembly[] LoadAssemblyFromPath(string path)
        {
            List<Assembly> result = new List<Assembly>();

            foreach(string dll in Directory.GetFiles(path, "*.dll"))
            {
                result.Add(Assembly.LoadFrom(dll));
            }

            return result.ToArray();
        }
        /// <summary>
        /// Gets all the assembly of the application
        /// </summary>
        /// <returns></returns>
        public static Assembly[] LoadApplicationAssemblies()
        {
            List<Assembly> result = new List<Assembly>();
            //get assemblies from folder
            result = LoadAssemblyFromPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).ToList();
            //get executin assembly
            result.Insert(0, Application.Current.GetType().Assembly);

            return result.ToArray();
        }
    }
}
