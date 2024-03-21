using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Logging
{
    /// <summary>
    /// Settings in the app.config for the loggers
    /// </summary>
    [Serializable]
    public class LoggerInstance
    {
        public string InstanceName { get; set; }
        public string LogName { get; set; }
        public int LogLevel { get; set; }
    }
}
