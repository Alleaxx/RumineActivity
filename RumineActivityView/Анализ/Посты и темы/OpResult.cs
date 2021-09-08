using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class OpResult
    {
        public override string ToString()
        {
            return $"{Message} - {Error} - {Warning}";
        }

        public string Message { get; set; }
        public bool Error { get; set; }
        public bool Warning { get; set; }
        public string CssClass => Error ? "error" : Warning ? "warning" : "normal";
        public List<OpResult> Inner { get; private set; } = new List<OpResult>();

        public OpResult(bool error, bool warning, string msg)
        {
            Error = error;
            Warning = warning;
            Message = msg;
        }
        public void Add(bool error, bool warning, string msg)
        {
            Inner.Add(new OpResult(error, warning, msg));
        }
    }
}
