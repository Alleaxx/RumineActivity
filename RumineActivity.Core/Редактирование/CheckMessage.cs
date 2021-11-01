using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class CheckMessage
    {
        public override string ToString()
        {
            return $"{Message} - {Error} - {Warning}";
        }

        public string Message { get; set; }
        public bool Error { get; set; }
        public bool Warning { get; set; }
        public string CssClass => Error ? "error" : Warning ? "warning" : "normal";
        public List<CheckMessage> Inner { get; private set; }

        public CheckMessage(bool error, bool warning, string msg)
        {
            Error = error;
            Warning = warning;
            Message = msg;
            Inner = new List<CheckMessage>();
        }
        public void Add(bool error, bool warning, string msg)
        {
            Inner.Add(new CheckMessage(error, warning, msg));
        }
    }
}
