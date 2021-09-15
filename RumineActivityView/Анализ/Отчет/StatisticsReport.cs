using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет об активности. Коллекция записей 'За период написано сообщений'
    public class StatisticsReport : Named
    {
        public ReportType Type { get; private set; }

        //Данные
        public IReadOnlyList<Entry> Entries { get; private set; }
        public DateRange DateRangeAll { get; private set; }
        public DateRange DateRangePosts { get; private set; }
        public Period Period { get; private set; }

        //Статистика по данным
        public bool IsEmpty => Entries == null || !Entries.Any();
        public int Length => Entries.Count;

        public double SumValue { get; private set; }
        public double AverageValue { get; private set; }
        public Entry MostInactive { get; private set; }
        public Entry MostActive { get; private set; }

        public StatisticsReport() : this("Нулевой отчет", Array.Empty<Entry>(), new ReportCreatorOptions())
        {
            Type = new ReportType(Reports.Empty);
        }
        public StatisticsReport(string name, IReadOnlyList<Entry> entries, ReportCreatorOptions options)
        {
            Name = name;
            Entries = entries;
            DateRangeAll = options.DateRange;
            Period = options.Period;

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
            SumValue = 0;
            AverageValue = 0;
            DateRangePosts = new DateRange();
        }
        private void SetStat()
        {
            MostInactive = Entries.Where(e => e.PostsAverage > 0).OrderBy(e => e.PostsAverage).FirstOrDefault();
            MostActive = Entries.Where(e => e.PostsAverage > 0).OrderBy(e => e.PostsAverage).LastOrDefault();

            SumValue = Entries.Sum(e => e.PostsWritten);
            AverageValue = Entries.Average(e => e.PostsWritten);
            DateRangePosts = new DateRange(Entries.Min(e => e.Range.From), Entries.Max(e => e.Range.To));
        }
    }
}




