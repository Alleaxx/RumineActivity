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

        public static DateTime NextDate(this DateTime date, Periods period)
        {
            switch (period)
            {
                case Periods.Month:
                    DateTime nextMonth = date.AddMonths(1);
                    return new DateTime(nextMonth.Year, nextMonth.Month, 1);
                case Periods.Year:
                    return new DateTime(date.Year + 1, 1, 1);
                default:
                    return date;
            }
        }
        public static DateTime NextDate(this DateTime date, Period period)
        {
            switch (period.Type)
            {
                case Periods.Month:
                    DateTime nextMonth = date.AddMonths(1);
                    return new DateTime(nextMonth.Year, nextMonth.Month, 1);
                case Periods.Year:
                    return new DateTime(date.Year + 1, 1, 1);
                default:
                    return date + period.TimeInterval;
            }
        }
    }
}
