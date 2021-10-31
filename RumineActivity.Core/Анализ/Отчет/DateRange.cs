using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    interface IDateRange
    {
        DateTime From { get; }
        DateTime To { get; }
    }
    //Временные рамки
    public class DateRange : IDateRange
    {
        public string ToString(string format = "", string sep = "-")
        {
            return $"{From.ToString(format)} {sep} {To.ToString(format)}";
        }


        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public TimeSpan Diff => To - From;

        public bool IsEmpty => From == To;
        public double DaysDifference => Diff.TotalDays;


        public DateRange()
        {

        }
        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
        public DateRange(Post newer, Post older)
        {
            To = newer.Date;
            From = older.Date;
        }



        //Получать % интервала в другом интервале
        public double GetFractionOfRange(DateRange range)
        {
            if(IsEmpty || range.IsEmpty)
            {
                return 0;
            }

            if (IsOutsideOfRange(range))
            {
                return 0;
            }
            else if (IsInsideOfRange(range))
            {
                return 1;
            }
            else
            {
                return CountModOfRange(range);
            }
        }

        private double CountModOfRange(DateRange range)
        {
            DateTime countFrom = range.From > From ? range.From : From;
            DateTime countTo = range.To < To ? range.To : To;

            TimeSpan compareDifference = countTo - countFrom;
            return compareDifference.TotalDays / DaysDifference;
        }

        public bool IsDateInside(DateTime date) => From <= date && date <= To;
        public bool IsOutsideOfRange(DateRange range)
        {
            return range.To < From || range.From > To;
        }
        public bool IsInsideOfRange(DateRange range)
        {
            return range.From < From && range.To > To;
        }
        public bool IsIntersectedWithRange(DateRange range)
        {
            return !IsOutsideOfRange(range);
        }
    }

}
