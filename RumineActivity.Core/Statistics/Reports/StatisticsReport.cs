using RumineActivity.Core.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    /// <summary>
    /// Отчет об активности. Коллекция записей 'За период написано сообщений'
    /// </summary>
    public class StatisticsReport
    {
        public string Name { get; private set; }

        public ConfigurationReport Configuration { get; protected set; }
        public DateRange DateRangeAll => Configuration.DateRange;
        public Period Period => Configuration.Period;

        /// <summary>
        /// Периодические записи
        /// </summary>
        public IReadOnlyList<Entry> Entries { get; private set; }

        /// <summary>
        /// Дополнительные фактические записи, которые не вписывают в указанные периоды
        /// </summary>
        public IReadOnlyList<Entry> AdditionalEntries { get; private set; }

        public DateRange DateRangePosts { get; private set; }


        /// <summary>
        /// Статистика по данным
        /// </summary>
        public bool IsEmpty => Entries == null || !Entries.Any();
        public int Length => Entries.Count;


        /// <summary>
        /// статистика по количеству написанных постов по записям
        /// </summary>
        public double GetTotalValue(MeasureUnits unit)
        {
            return Entries.Sum(e => e.GetValueTotal(unit)) + AdditionalEntries.Where(e => !e.IsOuterPartial).Sum(e => e.GetValueTotal(unit));
        }
        public double GetAverageDayValue(MeasureUnits unit)
        {
            return Entries.Average(e => e.GetValueDayAverage(unit));
        }


        //записи с самым низким и высоким значением средней активности
        public Entry? MostInactiveTotal { get; private set; }
        public Entry? MostActiveTotal { get; private set; }
        public Entry? MostInactiveAverage { get; private set; }
        public Entry? MostActiveAverage { get; private set; }


        public StatisticsReport(IReadOnlyList<Entry> entries, IReadOnlyList<Entry> additionalEntries, ConfigurationReport config)
        {
            Name = config.GetReportName();

            Entries = entries;
            AdditionalEntries = additionalEntries;

            Configuration = config;
            CountStatistics();
        }


        private void CountStatistics()
        {
            if (IsEmpty)
            {
                SetEmptyStat();
            }
            else
            {
                SetStat();
            }
        }
        private void SetEmptyStat()
        {
            DateRangePosts = new DateRange();
        }
        private void SetStat()
        {
            MostInactiveAverage = Entries
                .Where(e => e.PostsWrittenAverage > 0)
                .OrderBy(e => e.PostsWrittenAverage)
                .FirstOrDefault();
            MostActiveAverage = Entries
                .Where(e => e.PostsWrittenAverage > 0)
                .OrderBy(e => e.PostsWrittenAverage)
                .LastOrDefault();
            MostInactiveTotal = Entries
                .Where(e => e.PostsWrittenAverage > 0)
                .OrderBy(e => e.PostsWrittenTotal)
                .FirstOrDefault();
            MostActiveTotal = Entries
                .Where(e => e.PostsWrittenAverage > 0)
                .OrderBy(e => e.PostsWrittenTotal)
                .LastOrDefault();
            DateRangePosts = new DateRange(Entries.Min(e => e.FromDate), Entries.Max(e => e.ToDate));
        }
    }
}




