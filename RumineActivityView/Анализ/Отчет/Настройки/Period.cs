using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Период
    public enum Periods
    {
        Day, Week, Month, Year, Own
    }
    public class Period : EnumType<Periods>
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

        public Period(Periods period) : base(period)
        {
            switch (period)
            {
                case Periods.Day:
                    Name = "День";
                    TimeInterval = new TimeSpan(1, 0, 0, 0, 0);
                    DateFormat = "dd.MM.yyyy";
                    break;
                case Periods.Week:
                    Name = "Неделя";
                    TimeInterval = new TimeSpan(7, 0, 0, 0, 0);
                    DateFormat = "dd MMMM yyyy";
                    break;
                case Periods.Month:
                    Name = "Месяц";
                    TimeInterval = new TimeSpan(30, 0, 0, 0, 0);
                    DateFormat = "MMMM yyyy";
                    break;
                case Periods.Year:
                    Name = "Год";
                    TimeInterval = new TimeSpan(365, 0, 0, 0, 0);
                    DateFormat = "yyyy год";
                    break;
                case Periods.Own:
                    Name = "Свой";
                    TimeInterval = new TimeSpan(14, 0, 0, 0, 0);
                    DateFormat = "dd MMMM yyyy";
                    break;
            }
            days = TimeInterval.TotalDays;
        }
        
        public DateTime GetNextDate(DateTime date)
        {
            switch (Type)
            {
                case Periods.Month:
                    DateTime nextMonth = date.AddMonths(1);
                    return new DateTime(nextMonth.Year, nextMonth.Month, 1);
                case Periods.Year:
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
                    return new DateRange(ReportCreatorOptions.FoundationDate, DateTime.Now);
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
