using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Временные рамки
    public class DateRange
    {
        public override string ToString() => ToString();
        public string ToString(string format = "", string sep = "-")
        {
            return $"{From.ToString(format)} {sep} {To.ToString(format)}";
        }


        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public TimeSpan Diff => To - From;


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

        public bool IsZeroDiff => From == To;
        public bool IsSeparate(DateRange range) => GetFraction(range) == 0;     //полностью снаружи
        public bool IsCovered(DateRange range) => GetFraction(range) == 1;      //полностью внутри
        public bool IsDateInside(DateTime date) => From <= date && date <= To;  //полностью внутри
        public bool IsIntersected(DateRange range) => GetFraction(range) > 0;   //частично перекрывает

        //Получать % интервала в другом интервале
        public double GetFraction(DateRange range)
        {
            if (range.To < From || range.From > To)
            {
                return 0;
            }
            else if (range.From < From && range.To > To)
            {
                return 1;
            }
            else
            {
                DateTime countFrom = From;
                DateTime countTo = To;
                if (range.From > From)
                {
                    countFrom = range.From;
                }
                if (range.To < To)
                {
                    countTo = range.To;
                }
                TimeSpan compareDifference = countTo - countFrom;
                return compareDifference.TotalHours / Diff.TotalHours;
            }
        }
    }


    public class EntryDateRangeFraction
    {
        public readonly DateRange Source;
        public readonly Entry Entry;
        public readonly double Fraction;

        public EntryDateRangeFraction(DateRange source, Entry entry)
        {
            Source = source;
            Entry = entry;
            Fraction = Entry.Range.GetFraction(source);
        }
    }

}
