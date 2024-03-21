using Core.Interfaces.Logging;
using Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class LoggerHelper
    {
        internal const string DEFAULT_DATE_FORMAT = "yyyy-MM-dd";
        internal static Dictionary<string, ILogger> loggers;

        internal static void CheckDirectories(string logLocation, bool groupByDay)
        {
            if (groupByDay)
                logLocation = $"{logLocation}\\{DateTime.Now.ToString(DEFAULT_DATE_FORMAT)}";
            string logLocation_fullPath = Path.GetFullPath(logLocation);
            if (!Directory.Exists(logLocation_fullPath))
            {
                Directory.CreateDirectory(logLocation_fullPath);
            }
        }

        public static void InitLoggers(string logConfig)
        {
            LoggerInstanceCollection.InitLoggers(logConfig);
        }

        public static ILogger GetLogger(string loggerName)
        {
            return loggers.Where(p => p.Key == loggerName)?.Select(p => p.Value).FirstOrDefault();
        }

        public static ILogger GetSystemLogger()
        {
            return loggers.Where(p => p.Value.IsSystemLog)?.Select(p => p.Value).FirstOrDefault();
        }
    }
}
