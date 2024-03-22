using Core.Constants;
using Core.Interfaces.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Core.Helpers.LoggerHelper;

namespace Core.Logging
{
    internal class Logger : ILogger
    {
        public LoggingCnst.LogLevel LogLevel { get; private set; }
        public string LogName { get; private set; }
        public string LogFolderPath { get; private set; }
        public string LogLocation { get; private set; }
        private string _logLocationFullPath { get; set; }
        public bool IsSystemLog { get; private set; }
        private bool GroupByDay { get; set; }
        private DateTime initDateTime { get; set; }
        private StreamWriter sw;

        public Logger(LoggerCtorParam param)
            :this(param.LogLevel, param.LogName, param.LogLocation, param.GroupByDay, param.IsSystemLog)
        {
        }
        internal Logger(LoggingCnst.LogLevel logLevel, 
            string logName, 
            string logLocation,
            bool groupByDay,
            bool isSystemLog)
        {
            this.LogLevel = logLevel;
            this.LogName = logName;
            this.LogFolderPath = logLocation;
            this.GroupByDay = groupByDay;
            this.IsSystemLog = isSystemLog;
            InitLogger();
        }
        ~Logger() 
        {
            Dispose(disposing: false);
        }

        #region Private Methods
        private void InitLogger()
        {
            try
            {
                initDateTime = DateTime.Now;
                if (GroupByDay)
                {
                    LogLocation = $"{LogFolderPath}\\{initDateTime.ToString(DEFAULT_DATE_FORMAT)}";
                }
                else LogLocation = LogFolderPath;
                _logLocationFullPath = Path.GetFullPath($"{LogLocation}\\{LogName}.txt");
                sw = new StreamWriter(_logLocationFullPath, true);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, $"Error while initializing logger {LogName}.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void RestartLogger()
        {
            sw.Close();
            CheckDirectories(LogFolderPath, GroupByDay);
            InitLogger();
        }
        #endregion

        #region Log Write Methods
        private bool WriteLog(int level, string message, params object[] parameters)
        {
            if (level < (int)this.LogLevel)
                return false;

            if (this.initDateTime.Date < DateTime.Now.Date)
                RestartLogger();

            string log = $"[{GetLogAcronym(level)}] [{DateTime.Now}]  {message}";
            LogAnonym(log, parameters);
            return true;
        }
        public bool LogAnonym(string message, params object[] parameters)
        {
            if(parameters != null && parameters.Count() > 0)
            {
                message = string.Format(message, parameters);
            }
            sw.WriteLine(message);
            sw.Flush();
            return true;
        }
        public bool WriteLog(LoggingCnst.LogLevel level, string message, params object[] parameters) 
        {
            return WriteLog((int)level, message, parameters);
        }
        public bool LogDebug(string message, params object[] parameters) 
        {
            return WriteLog(LoggingCnst.LogLevel.DEBUG , message, parameters);
        }
        public bool LogInfo(string message, params object[] parameters) 
        {
            return WriteLog(LoggingCnst.LogLevel.INFO , message, parameters);
        }
        public bool LogWarn(string message, params object[] parameters) 
        {
            return WriteLog(LoggingCnst.LogLevel.WARN , message, parameters);
        }
        public bool LogError(string message, params object[] parameters) 
        {
            return WriteLog(LoggingCnst.LogLevel.ERROR , message, parameters);
        }
        public bool LogFatal(string message, params object[] parameters) 
        {
            return WriteLog(LoggingCnst.LogLevel.FATAL , message, parameters);
        }

        #region IDisposable interface
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    sw.Dispose();
                }
                disposed = true;
            }
        }
        #endregion
        private string GetLogAcronym(int level)
        {
            LoggingCnst.LogLevel lvl;
            Enum.TryParse(level.ToString() , out lvl);
            switch (lvl)
            {
                //case LoggingCnst.LogLevel.ALL:
                case LoggingCnst.LogLevel.DEBUG:
                    return "DEBUG";
                case LoggingCnst.LogLevel.INFO:
                    return "INFO ";
                case LoggingCnst.LogLevel.WARN:
                    return "WARN ";
                case LoggingCnst.LogLevel.ERROR:
                    return "ERROR";
                case LoggingCnst.LogLevel.FATAL:
                    return "FATAL";
                default: return "SYS  ";
                //case LoggingCnst.LogLevel.OFF:
            }
        }
        #endregion
    }
}
