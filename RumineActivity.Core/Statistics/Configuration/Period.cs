using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core.Measures;

namespace RumineActivity.Core
{
    //Период
    public enum Periods
    {
        Day, Week, Month, Season, Quarter, Year, Own
    }
    public abstract class Period : EnumType<Periods>
    {
        public TimeSpan TimeInterval { get; protected set; }

        public Func<DateRange, string> ChartDateFunc { get; protected set; }
        public Func<DateRange, string> EntryDateFunc { get; protected set; }


        public string NameReport { get; protected set; }
        public string NameCategory { get; protected set; }

        public double Days
        {
            get => days;
            set
            {
                if(value <= 0 || value > 3650)
                {
                    return;
                }

                days = value;
                TimeInterval = new TimeSpan((int)days, 0, 0, 0, 0);
                if(Type == Periods.Own)
                {
                    NameReport = $"Еже-{Days}-дневная";
                    NameCategory = $"По {Days} дн.";
                }
            }
        }
        private double days;

        public static Period Create(Periods period)
        {
            switch (period)
            {
                case Periods.Day:
                    return new PeriodDay();
                case Periods.Week:
                    return new PeriodWeek();
                case Periods.Month:
                    return new PeriodMonth();
                case Periods.Season:
                    return new PeriodSeason();
                case Periods.Quarter:
                    return new PeriodQuarter();
                case Periods.Year:
                    return new PeriodYear();
                default:
                    return new PeriodOwn();
            }
        }


        protected Period(Periods period) : base(period)
        {
            days = TimeInterval.TotalDays;
        }
        public virtual DateTime GetNextDate(DateTime date)
        {
            return date + TimeInterval;
        }
    }

}
