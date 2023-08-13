using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public static class RumineValues
    {
        public static readonly DateTime FoundationDate = new DateTime(2011, 7, 27);
        public static DateTime EndDate => new DateTime(DateTime.Now.Year, 12, 31);


        public static readonly int MaximumMessagesPerDayAbsolute = 1000000;
    }
}
