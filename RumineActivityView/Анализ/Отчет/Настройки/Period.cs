using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivityView.Measures;

namespace RumineActivityView
{
    //Период
    public enum Periods
    {
        Day, Week, Month, Year, Own
    }
    public abstract class Period : EnumType<Periods>
    {
        public TimeSpan TimeInterval { get; protected set; }
        public string DateFormat { get; protected set; }

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

namespace RumineActivityView.Measures
{
    class PeriodDay : Period
    {
        public PeriodDay() : base(Periods.Day)
        {
            Name = "День";
            TimeInterval = new TimeSpan(1, 0, 0, 0, 0);
            DateFormat = "dd.MM.yyyy";
        }
    }

    class PeriodWeek : Period
    {
        public PeriodWeek() : base(Periods.Week)
        {
            Name = "Неделя";
            TimeInterval = new TimeSpan(7, 0, 0, 0, 0);
            DateFormat = "dd MMMM yyyy";
        }
    }

    class PeriodMonth : Period
    {
        public PeriodMonth() : base(Periods.Month)
        {
            Name = "Месяц";
            TimeInterval = new TimeSpan(30, 0, 0, 0, 0);
            DateFormat = "MMMM yyyy";
        }
        public override DateTime GetNextDate(DateTime date)
        {
            DateTime nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1);
        }
    }

    class PeriodYear : Period
    {
        public PeriodYear() : base(Periods.Year)
        {
            Name = "Год";
            TimeInterval = new TimeSpan(365, 0, 0, 0, 0);
            DateFormat = "yyyy год";
        }
        public override DateTime GetNextDate(DateTime date)
        {
            return new DateTime(date.Year + 1, 1, 1);
        }
    }

    class PeriodOwn : Period
    {
        public PeriodOwn() : base(Periods.Own)
        {
            Name = "Свой";
            TimeInterval = new TimeSpan(14, 0, 0, 0, 0);
            DateFormat = "dd MMMM yyyy";
        }
    }
}
