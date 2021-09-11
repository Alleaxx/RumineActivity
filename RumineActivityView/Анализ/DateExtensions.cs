using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public static class DateExtensions
    {
        public static string MonthName(this int month)
        {
            return new DateTime(2011, month, 1).ToString("MMMM");
        }
        public static DateRange CreateRange(this DateTime date, DateTime toDate)
        {
            return new DateRange(date, toDate);
        }
    }
}
