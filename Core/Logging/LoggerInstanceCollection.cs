using Core.Dependency;
using Core.Helpers;
using Core.Interfaces.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Unity;
using Unity.Resolution;
using static Core.Constants.LoggingCnst;
using static Core.Helpers.LoggerHelper;

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
                loggers = new Dictionary<string, ILogger>();
                CheckDirectories(collection.LogLocation, collection.GroupByDay);
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
                LogName = instance.LogName,
                LogLocation = logLocation,
                GroupByDay = groupByDay,
                IsSystemLog = isSystemLog
            };
            ILogger logger = _container.Resolve<ILogger>(new ParameterOverride("param", param));
            loggers.Add(instance.InstanceName, logger);
        }

        private static void CheckSystemLogger(string logLocation, bool groupByDay)
        {
            ILogger systemLogger = GetSystemLogger();
            if(systemLogger == null) 
            {
                InitLogger(logLocation, groupByDay, true, new LoggerInstance()
                {
                    LogLevel = (int)LogLevel.ALL,
                    LogName = "Application_log",
                    InstanceName = "System"
                });
            }
        }

    }
}
