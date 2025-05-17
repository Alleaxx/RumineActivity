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

namespace RumineActivity.Core.Measures
{
    class PeriodDay : Period
    {
        public PeriodDay() : base(Periods.Day)
        {
            Name = "День";
            NameReport = "Ежедневная";
            NameCategory = "По дням";
            TimeInterval = new TimeSpan(1, 0, 0, 0, 0);

            EntryDateFunc = range => range.From.ToString("dd.MM.yyyy");
            ChartDateFunc = range => range.From.ToString("dd.MM.yy");
        }
    }

    class PeriodWeek : Period
    {
        public PeriodWeek() : base(Periods.Week)
        {
            Name = "Неделя";
            NameReport = "Еженедельная";
            NameCategory = "По неделям";
            TimeInterval = new TimeSpan(7, 0, 0, 0, 0);

            EntryDateFunc = range =>
            {
                int week = DateExtensions.DefineDayYearWeekIndex(range.From);
                bool sameMonth = range.From.Month == range.To.Month;
                string month = sameMonth ? $"{range.From:dd}-{range.To:dd} {range.From.Month.GetMonthName("MMM")}" : $"{range.From:dd} {range.From.Month.GetMonthName("MMM")} - {range.To:dd} {range.To.Month.GetMonthName("MMM")}";
                return $"{week} неделя {range.From:yyyy} ({month})";
            };
            ChartDateFunc = range => $"{DateExtensions.DefineDayYearWeekIndex(range.From)} нед {range.From:yy}";
        }

        public override DateTime GetNextDate(DateTime date)
        {
            int weekIndex = DateExtensions.DefineDayYearWeekIndex(date);
            int newWeekIndex = weekIndex;
            for (int i = 1; i <= 8; i++)
            {
                var newDate = date.AddDays(i);
                newWeekIndex = DateExtensions.DefineDayYearWeekIndex(newDate);
                if(weekIndex != newWeekIndex)
                {
                    return newDate;
                }
            }
            throw new Exception("Не удалось найти новую неделю! Приехали!");
        }
    }

    class PeriodMonth : Period
    {
        public PeriodMonth() : base(Periods.Month)
        {
            Name = "Месяц";
            NameReport = "Ежемесячная";
            NameCategory = "По месяцам";
            TimeInterval = new TimeSpan(30, 0, 0, 0, 0);

            EntryDateFunc = range => range.From.ToString("MMMM yyyy");
            ChartDateFunc = range => range.From.ToString("MMM yy");
        }
        public override DateTime GetNextDate(DateTime date)
        {
            DateTime nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1);
        }
    }

    class PeriodSeason : Period
    {
        public PeriodSeason() : base(Periods.Season)
        {
            Name = "Сезон";
            NameReport = "Ежесезонная";
            NameCategory = "По сезонам";
            TimeInterval = new TimeSpan(90, 0, 0, 0, 0);

            EntryDateFunc = range => $"{DateExtensions.SeasonToString(DateExtensions.DefineSeason(range.From))} {range.From:yyyy}";
            ChartDateFunc = range => $"{DateExtensions.SeasonToString(DateExtensions.DefineSeason(range.From))} {range.From:yy}";
        }

        //public override DateTime GetNextDate(DateTime date)
        //{
        //    int seasonIndex = DateExtensions.DefineQuarter(date);
        //    int newSeasonIndex = seasonIndex;
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        var newDate = date.AddMonths(i);
        //        newSeasonIndex = DateExtensions.DefineQuarter(newDate);
        //        if (seasonIndex != newSeasonIndex)
        //        {
        //            return newDate;
        //        }
        //    }
        //    throw new Exception("Не удалось найти новый квартал! Приехали!");
        //}
    }

    class PeriodQuarter : Period
    {
        public PeriodQuarter() : base(Periods.Quarter)
        {
            Name = "Квартал";
            NameReport = "Ежеквартальная";
            NameCategory = "По кварталам";
            TimeInterval = new TimeSpan(90, 0, 0, 0, 0);

            EntryDateFunc = range => $"{DateExtensions.DefineQuarterSymbol(range.From)} квартал {range.From:yyyy}";
            ChartDateFunc = range => $"{DateExtensions.DefineQuarterSymbol(range.From)} кв. {range.From:yy}";
        }
        public override DateTime GetNextDate(DateTime date)
        {
            int seasonIndex = DateExtensions.DefineQuarter(date);
            int newSeasonIndex = seasonIndex;
            for (int i = 1; i <= 12; i++)
            {
                var newDate = date.AddMonths(i);
                newSeasonIndex = DateExtensions.DefineQuarter(newDate);
                if (seasonIndex != newSeasonIndex)
                {
                    return newDate;
                }
            }
            throw new Exception("Не удалось найти новый квартал! Приехали!");
        }
    }

    class PeriodYear : Period
    {
        public PeriodYear() : base(Periods.Year)
        {
            Name = "Год";
            NameReport = "Ежегодная";
            NameCategory = "По годам";
            TimeInterval = new TimeSpan(365, 0, 0, 0, 0);

            EntryDateFunc = range => range.From.ToString("yyyy год");
            ChartDateFunc = range => range.From.ToString("yyyy");
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
            Name = "Свой период";
            NameReport = $"Ежедневная";
            NameCategory = "По 14 дней";
            TimeInterval = new TimeSpan(14, 0, 0, 0, 0);
            Days = 14;

            EntryDateFunc = range => $"{range.From:dd.MM.yyyy} - {range.To:dd.MM.yyyy}";
            ChartDateFunc = range => $"{range.From:dd.MM.yy}";
        }
    }
}
