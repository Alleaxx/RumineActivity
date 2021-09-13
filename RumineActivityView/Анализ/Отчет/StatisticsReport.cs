using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет об активности. Коллекция записей 'За период написано сообщений'
    public class StatisticsReport
    {
        public ReportType Type { get; private set; }

        //Данные
        public string Name { get; private set; }
        public Entry[] Entries { get; private set; }
        public DateRange DateRangeAll { get; private set; }
        public DateRange DateRangePosts { get; private set; }
        public Period Period { get; private set; }

        //Статистика по данным
        public bool IsEmpty => Entries == null || !Entries.Any();
        public int Length => Entries.Length;

        public double SumValue { get; private set; }
        public double AverageValue { get; private set; }
        public Entry MostInactive { get; private set; }
        public Entry MostActive { get; private set; }

        public StatisticsReport() : this("Нулевой отчет", Array.Empty<Entry>(), new ReportCreatorOptions())
        {
            Type = new ReportType(Reports.Empty);
        }
        public StatisticsReport(string name, IEnumerable<Entry> entries, ReportCreatorOptions options)
        {
            Name = name;
            Entries = entries.ToArray();
            DateRangeAll = options.DateRange;
            Period = options.Period;

            SumValue = IsEmpty ? 0 : Entries.Sum(e => e.PostsDefault);
            AverageValue = IsEmpty ? 0 : Entries.Average(e => e.PostsDefault);

            MostInactive = Entries.Where(e => e.PostsRelative > 0).OrderBy(e => e.PostsRelative).FirstOrDefault();
            MostActive = Entries.Where(e => e.PostsRelative > 0).OrderBy(e => e.PostsRelative).LastOrDefault();
            if (IsEmpty)
            {
                DateRangePosts = new DateRange();
            }
            else
            {
                DateRangePosts = new DateRange(Entries.Min(e => e.Range.From), Entries.Max(e => e.Range.To));
            }
        }
    }
}




