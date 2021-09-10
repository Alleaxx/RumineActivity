using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет об активности. Коллекция записей 'За период написано сообщений'
    public class StatisticsReport
    {
        //Данные
        public string Name { get; private set; }
        public Entry[] Entries { get; private set; }
        public DateRange DateRange { get; private set; }
        public DateInterval Interval { get; private set; }
        public DateRange FirstLastPost { get; private set; }

        //Статистика по данным
        public bool IsEmpty => Entries == null || !Entries.Any();
        public int Length => Entries.Length;

        public double SumValue { get; private set; }
        public double AverageValue { get; private set; }
        public Entry MostInactive { get; private set; }
        public Entry MostActive { get; private set; }

        public DateTime FirstPost { get; set; }
        public DateTime LastPost { get; set; }


        public StatisticsReport() : this("Нулевой отчет", Array.Empty<Entry>(), new ReportOptions())
        {

        }
        public StatisticsReport(string name, IEnumerable<Entry> entries, ReportOptions options)
        {
            Name = name;
            Entries = entries.ToArray();
            DateRange = options.DateRange;
            Interval = options.DateInterval;

            SumValue = IsEmpty ? 0 : Entries.Sum(e => e.PostsDefault);
            AverageValue = IsEmpty ? 0 : Entries.Average(e => e.PostsDefault);

            MostInactive = Entries.Where(e => e.PostsRelative > 0).OrderBy(e => e.PostsRelative).FirstOrDefault();
            MostActive = Entries.Where(e => e.PostsRelative > 0).OrderBy(e => e.PostsRelative).LastOrDefault();
            if (IsEmpty)
            {
                FirstLastPost = new DateRange();
            }
            else
            {
                FirstLastPost = new DateRange(Entries.Min(e => e.Range.From), Entries.Max(e => e.Range.To));
            }
        }
    }
}




