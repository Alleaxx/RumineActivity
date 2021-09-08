using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Период
    public enum Dates
    {
        Day, Week, Month, Year, Own
    }
    public class DateInterval : EnumType<Dates>
    {
        public TimeSpan TimeInterval { get; private set; }
        public string DateFormat { get; private set; }

        public double Days
        {
            get => days;
            set
            {
                days = value;
                TimeInterval = new TimeSpan((int)days, 0, 0, 0, 0);
            }
        }
        private double days;

        public DateInterval(Dates period) : base(period)
        {
            switch (period)
            {
                case Dates.Day:
                    Name = "День";
                    TimeInterval = new TimeSpan(1, 0, 0, 0, 0);
                    DateFormat = "dd.MM.yyyy";
                    break;
                case Dates.Week:
                    Name = "Неделя";
                    TimeInterval = new TimeSpan(7, 0, 0, 0, 0);
                    DateFormat = "dd MMMM yyyy";
                    break;
                case Dates.Month:
                    Name = "Месяц";
                    TimeInterval = new TimeSpan(30, 0, 0, 0, 0);
                    DateFormat = "MMMM yyyy";
                    break;
                case Dates.Year:
                    Name = "Год";
                    TimeInterval = new TimeSpan(365, 0, 0, 0, 0);
                    DateFormat = "yyyy год";
                    break;
                case Dates.Own:
                    Name = "Свой";
                    TimeInterval = new TimeSpan(0, 0, 0, 0, 0);
                    DateFormat = "dd MMMM yyyy";
                    break;
            }
            days = TimeInterval.TotalDays;
        }

        public Entry GetNextEntry(DateTime date)
        {
            string name = date.ToString(DateFormat);
            DateRange range = new DateRange(date, GetNextDate(date));
            return new Entry(range, name);
        }
        public DateTime GetNextDate(DateTime date)
        {
            switch (Type)
            {
                case Dates.Month:
                    DateTime nextMonth = date.AddMonths(1);
                    return new DateTime(nextMonth.Year, nextMonth.Month, 1);
                case Dates.Year:
                    return new DateTime(date.Year + 1, 1, 1);
                default:
                    return date + TimeInterval;
            }
        }

    }


    //Указатель на год и месяц
    public class YearMonthDateSelector
    {
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public (int from, int to) Day { get; set; } = (0, 0);
        public DateRange Dates
        {
            get
            {
                if (Year == 0)
                {
                    return new DateRange(ReportOptions.FoundationDate, DateTime.Now);
                }
                else
                {
                    if (Month == 0)
                    {
                        return new DateRange(new DateTime(Year, 1, 1), new DateTime(Year, 12, 31));
                    }
                    else
                    {
                        if(Day.from == 0 || Day.to == 0)
                        {
                            return new DateRange(new DateTime(Year, Month, 1), new DateTime(Year, Month, 1).AddMonths(1)); /*DateTime.DaysInMonth(Year, Month)));*/
                        }
                        else
                        {
                            return new DateRange(new DateTime(Year, Month, Day.from), new DateTime(Year, Month, Day.to));
                        }
                    }
                }
            }
        }

        public YearMonthDateSelector(int year, int month)
        {
            Year = year;
            Month = month;
        }
    }

}
