using Core.Dependency;
using Core.Helpers;
using Core.Interfaces.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Unity;
using Unity.Resolution;
using static Core.Constants.LoggingCnst;
//using static Core.Helpers.LoggerHelper;

namespace Core.Logging
{
    [Serializable]
    public class LoggerInstanceCollection
    {
        #region Loggers Data
        public string LogLocation { get; set; }
        public bool GroupByDay { get; set; }
        public int PurgingDay { get; set; }
        public string SystemLog_InstaceName { get; set; }
        public List<LoggerInstance> LoggerInstances { get; set; } 
        #endregion
        private static IUnityContainer _container = UnityHelper.Current.GetLocalContainer();

        internal LoggerInstanceCollection() { }

        internal static void InitLoggers(string config)
        {
            try
            {
                LoggerInstanceCollection collection = GetLoggerInstanceCollection(config);
                LoggerHelper.loggers = new Dictionary<string, ILogger>();
                LoggerHelper.CheckDirectories(collection.LogLocation, collection.GroupByDay);
                PurgeLogs(collection.LogLocation, collection.PurgingDay, collection.GroupByDay);
                foreach (LoggerInstance loggerInstance in collection.LoggerInstances)
                {
                    InitLogger(collection.LogLocation, 
                        collection.GroupByDay, 
                        String.Equals(collection.SystemLog_InstaceName, loggerInstance.InstanceName),
                        loggerInstance);
                }

                CheckSystemLogger(collection.LogLocation, collection.GroupByDay);
            }
            catch(Exception ex)
            {
                throw new Exception("Error in the initialization of the Logging configuration.\n" + ex.Message);
            }
        }

        private static LoggerInstanceCollection GetLoggerInstanceCollection(string config)
        {
            if (!config.Contains("<?xml version=\"1.0\"?>"))
                config = "<?xml version=\"1.0\"?>\n" + config;
            return (LoggerInstanceCollection)XmlSerializerBase.DeserializeObjectFromString(config, typeof(LoggerInstanceCollection));
        }

        private static void InitLogger(string logLocation,
            bool groupByDay,
            bool isSystemLog,
            LoggerInstance instance)
        {
            LogLevel logLevel;
            if(!Enum.TryParse(instance.LogLevel.ToString(), out logLevel))
            {
                logLevel = LogLevel.ALL;
            }
            LoggerCtorParam param = new LoggerCtorParam()
            {
                LogLevel = logLevel,
                LogName = CreateLogName(instance.LogName, !groupByDay),
                LogLocation = logLocation,
                GroupByDay = groupByDay,
                IsSystemLog = isSystemLog
            };
            ILogger logger = _container.Resolve<ILogger>(new ParameterOverride("param", param));
            LoggerHelper.loggers.Add(instance.InstanceName, logger);
        }

        private static void CheckSystemLogger(string logLocation, bool groupByDay)
        {
            ILogger systemLogger = LoggerHelper.GetSystemLogger();
            if(systemLogger == null) 
            {
                InitLogger(logLocation, groupByDay, true, new LoggerInstance()
                {
                    LogLevel = (int)LogLevel.ALL,
                    LogName = CreateLogName("Application_log", !groupByDay),
                    InstanceName = "System"
                });
            }
        }

        private static string CreateLogName(string nominalName, bool useDate)
        {
            string logName = nominalName;
            if (useDate)
            {
                logName = string.Format(nominalName + "_" + DateTime.Now.Date.ToString(LoggerHelper.DEFAULT_DATE_FORMAT));
            }
            return logName;
        }

        private static void PurgeLogs(string logLocation, int purgeDays, bool groupByDay)
        {
            if (purgeDays <= 0)
                return;
            DateTime thresholdData = DateTime.Now.Date.AddDays(-purgeDays);
            //Clear directories
            string[] directories = Directory.GetDirectories(logLocation);
            foreach(string dir in directories)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dir);
                if(dirInfo.CreationTime.Date < thresholdData)
                {
                    Directory.Delete(dir, true);
                }
            }

            //Clear files
            string[] files = Directory.GetFiles(logLocation);
            foreach(string file in files)
            {
                FileInfo fInfo = new FileInfo(file);
                if(fInfo.CreationTime.Date < thresholdData)
                {
                    File.Delete(file);
                }
            }
        }
    }
}
