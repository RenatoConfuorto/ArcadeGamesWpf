using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public class LoggingCnst
    {
        public enum LogLevel
        {
            ALL = 0, 
            DEBUG = 1, 
            INFO = 2, 
            WARN = 3, 
            ERROR = 4, 
            FATAL = 5, 
            OFF = 7
        }
    }
}
