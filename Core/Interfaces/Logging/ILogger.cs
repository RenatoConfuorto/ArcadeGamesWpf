using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Logging
{
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// The level of log
        /// </summary>
        LoggingCnst.LogLevel LogLevel { get; }
        /// <summary>
        /// Name of the log file
        /// </summary>
        string LogName { get; }
        /// <summary>
        /// The path used by the log writer to position the file
        /// </summary>
        string LogLocation { get; }
        /// <summary>
        /// If is the main application log
        /// </summary>
        bool IsSystemLog { get; }

        #region Write Log Methods
        /// <summary>
        /// Writes a log without log level
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool LogAnonym(string message);
        /// <summary>
        /// Write a log with a log level defined by the user
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool WriteLog(LoggingCnst.LogLevel level, string message, [CallerMemberName]string methodName = null);
        /// <summary>
        /// Writes a log with the DEBUG log level (1)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool LogDebug(string message, [CallerMemberName]string methodName = null);
        /// <summary>
        /// Writes a log with the INFO log level (2)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool LogInfo(string message, [CallerMemberName]string methodName = null);
        /// <summary>
        /// Writes a log with the WARN log level (3)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool LogWarn(string message, [CallerMemberName]string methodName = null);
        /// <summary>
        /// Writes a log with the ERROR log level (4)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool LogError(string message, [CallerMemberName]string methodName = null);
        /// <summary>
        /// Writes a log with the FATAL log level (5)
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool LogFatal(string message, [CallerMemberName]string methodName = null);
        /// <summary>
        /// Writes a log with all the details of the exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        bool LogException(Exception ex, LoggingCnst.LogLevel logLevel = LoggingCnst.LogLevel.ERROR, [CallerMemberName]string methodName = null);
        #endregion
    }
}
