using RumineActivity.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Logging
{
    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Log(new Log()
            {
                Message = message,
            });
        }

        public void Log(Log log)
        {
            Console.WriteLine($"{log.Date}: {log.Message}");
        }
    }
}
