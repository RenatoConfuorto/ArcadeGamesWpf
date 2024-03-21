using Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Logging
{
    internal class LoggerCtorParam
    {
        public LoggingCnst.LogLevel LogLevel { get; set; }
        public string LogName { get; set; }
        public string LogLocation { get; set; }
        public bool GroupByDay { get; set; }
        public bool IsSystemLog { get; set; }
    }
}
