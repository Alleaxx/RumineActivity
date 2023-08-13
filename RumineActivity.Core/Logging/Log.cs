using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Logging
{
    public class Log
    {
        public DateTime Date { get; init; }
        public string Message { get; set; }

        public Log()
        {
            Date = DateTime.Now;
        }
    }
}
