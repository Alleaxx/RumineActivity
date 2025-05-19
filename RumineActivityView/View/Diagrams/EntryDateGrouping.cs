using RumineActivity.Core;
using RumineActivity.View.Diagrams;

namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Описание группировки записей диаграммы по дате
    /// </summary>
    public class EntryDateGrouping
    {
        public override string ToString()
        {
            return $"{Level} - {Title} ({EntriesLimit})";
        }

        #region Описание группировок

        /// <summary>
        /// 2012 год
        /// </summary>
        public static readonly EntryDateGrouping YearGrouping = new EntryDateGrouping()
        {
            Title = "По году",
            Level = 6,
            GroupingFunc = entries => entries.GroupBy(e => e.Entry.FromDate.ToString("yyyy год")),
            EntriesLimit = 30,
            AllowedPeriods = new Periods[] { Periods.Week, Periods.Month, Periods.Quarter, Periods.Year }
        };

        /// <summary>
        /// 1-е п. 2012
        /// </summary>
        public static readonly EntryDateGrouping HalfYearGrouping = new EntryDateGrouping()
        {
            Title = "По полугодию",
            Level = 5,
            GroupingFunc = entries => entries.GroupBy(e => $"{DateExtensions.DefineHalfYear(e.Entry.FromDate)}-е п. {e.Entry.FromDate:yy}"),
            AllowedPeriods = [],

        };

        /// <summary>
        /// IV кв. 2012
        /// </summary>
        public static readonly EntryDateGrouping QuartedGrouping = new EntryDateGrouping()
        {
            Title = "По кварталу",
            Level = 4,
            GroupingFunc = entries => entries.GroupBy(e => $"{DateExtensions.DefineQuarterSymbol(e.Entry.FromDate)} кв. {e.Entry.FromDate:yy}"),
            AllowedPeriods = [Periods.Quarter],
        };

        /// <summary>
        /// Лето 12
        /// </summary>
        public static readonly EntryDateGrouping SeasonGrouping = new EntryDateGrouping()
        {
            Title = "По сезону",
            Level = 4,
            GroupingFunc = entries => entries.GroupBy(e => $"{DateExtensions.DefineSeasonText(e.Entry.FromDate)}"),
            AllowedPeriods = new Periods[] { Periods.Day, Periods.Week, Periods.Month }
        };
        
        /// <summary>
        /// Дек 12
        /// </summary>
        public static readonly EntryDateGrouping MonthShortGrouping = new EntryDateGrouping()
        {
            Title = "По месяцу (кратко)",
            Level = 3,
            GroupingFunc = entries => entries.GroupBy(e => $"{e.Entry.FromDate.Month.GetMonthName("MMM")} {e.Entry.FromDate:yy}"),
            EntriesLimit = 18,
            AllowedPeriods = new Periods[] { Periods.Day, Periods.Week, Periods.Month }
        };
        
        /// <summary>
        /// Декабрь 2012
        /// </summary>
        public static readonly EntryDateGrouping MonthFullGrouping = new EntryDateGrouping()
        {
            Title = "По месяцу (полному)",
            Level = 2,
            GroupingFunc = entries => entries.GroupBy(e => $"{e.Entry.FromDate:MMMM yy}"),
            EntriesLimit = 12,
            AllowedPeriods = new Periods[] { Periods.Day, Periods.Week, Periods.Month }

        };

        /// <summary>
        /// 10-19 дек 2012
        /// </summary>
        public static readonly EntryDateGrouping DayMonthGrouping = new EntryDateGrouping()
        {
            Title = "По трети месяца",
            Level = 1,
            GroupingFunc = entries => entries.GroupBy(e => $"{DateExtensions.DefineDayMonthPartString(e.Entry.FromDate)} {e.Entry.FromDate.Month.GetMonthName("MMM")} {e.Entry.FromDate:yy}"),
            AllowedPeriods = new Periods[] { Periods.Day }
        };

        /// <summary>
        /// 9 дек
        /// </summary>
        public static readonly EntryDateGrouping DayGrouping = new EntryDateGrouping()
        {
            Title = "По дням",
            Level = 0,
            EntriesLimit = 31,
            GroupingFunc = entries => entries.GroupBy(e => $"{e.Entry.FromDate.Day}"),
            AllowedPeriods = new Periods[] { Periods.Day }
        };

        /// <summary>
        /// Сохраняет оригинальное имя периода
        /// </summary>
        public static readonly EntryDateGrouping DefaultGrouping = new EntryDateGrouping()
        {
            Title = "Стандартная группировка",
            Level = -1,
            GroupingFunc = entries => entries.GroupBy(e => e.Entry.GetNameChart())
        };

        #endregion

        public string Title { get; private set; }
        public int Level { get; private set; }

        public Func<IEnumerable<DiagramEntryObject>, IEnumerable<IGrouping<string, DiagramEntryObject>>> GroupingFunc { get; private set; }
        private int EntriesLimit {  get; set; }
        private Periods[] AllowedPeriods { get; set; }


        private EntryDateGrouping()
        {
            Level = 10;
            EntriesLimit = 18;
            AllowedPeriods = Enum.GetValues<Periods>().ToArray();
        }

        public bool CheckEntries(IEnumerable<DiagramEntryObject> entries)
        {
            var first = entries.First();
            if (!AllowedPeriods.Contains(first.Entry.Period.Type))
            {
                return false;
            }

            var grouped = GroupingFunc(entries).ToArray();
            return grouped.Count() <= EntriesLimit && grouped.Count() >= 3;
        }
    }
}
