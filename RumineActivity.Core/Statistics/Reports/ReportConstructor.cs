using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    /// <summary>
    /// Конструктор отчетов
    /// </summary>
    public class ReportConstructor
    {
        /// <summary>
        /// Коллекция постов, упорядоченная по дате (возрастающая)
        /// </summary>
        private Post[] Posts { get; init; }
        private ConfigurationReport Config { get; init; }

        public ReportConstructor(IForum source, ConfigurationReport config)
        {
            Config = config ?? new ConfigurationReport();    
            Posts = Config.GetPostsSource(source);
        }

        public StatisticsReport Create()
        {
            List<Entry> periodicalEntries = new List<Entry>();
            List<Entry> factEntries = new List<Entry>();

            var ranges = CreateDateRanges();

            //Периодические записи
            int a = 0;
            foreach (var range in ranges)
            {
                var postBorders = Posts.GetBorders(range);
                var prevEntry = periodicalEntries.LastOrDefault();
                var newEntry = new Entry(a, postBorders, Config, prevEntry, true);
                periodicalEntries.Add(newEntry);
                a++;
            }

            //Фактические записи
            foreach (var entry in periodicalEntries)
            {
                bool isFirst = periodicalEntries.First() == entry;
                bool isLast = periodicalEntries.Last() == entry;
                if (entry.PostBorders.GetMissedBeforeFirst() > 0)
                {
                    var firstPost = entry.PostBorders.PostOuterBorders.PostX;
                    var lastPost = entry.PostBorders.PostInnerBorders.PostX;
                    Entry newEntry = CreateNewFactEntry(factEntries, firstPost, lastPost);
                    newEntry.IsOuterPartial = newEntry.Range.IsIntersectedWithRange(Config.DateRange);
                    factEntries.Add(newEntry);
                }
                if (isLast && entry.PostBorders.GetMissedAfterLast() > 0)
                {
                    var firstPost = entry.PostBorders.PostInnerBorders.PostY;
                    var lastPost = entry.PostBorders.PostOuterBorders.PostY;
                    Entry newEntry = CreateNewFactEntry(factEntries, firstPost, lastPost);
                    newEntry.IsOuterPartial = newEntry.Range.IsIntersectedWithRange(Config.DateRange);
                    factEntries.Add(newEntry);
                }
            }
            var sumPosts = periodicalEntries.Sum(e => e.PostsWrittenTotal) + factEntries.Sum(e => e.PostsWrittenTotal);
            foreach (var entry in factEntries.Union(periodicalEntries))
            {
                entry.FractionMode = entry.PostsWrittenTotal / sumPosts;
            }

            return new StatisticsReport(periodicalEntries.ToArray(), factEntries.ToArray(), Config);
        }

        private Entry CreateNewFactEntry(List<Entry> factEntries, Post firstPost, Post lastPost)
        {
            var dateRange = new DateRange(lastPost, firstPost);
            var postBorderInner = new PostRange(firstPost, lastPost, -1);
            var postBorderOuter = postBorderInner;
            var postBorders = new DateRangePostBorders()
            {
                DateRange = dateRange,
                PostInnerBorders = postBorderInner,
                PostOuterBorders = postBorderOuter
            };
            var newEntry = new Entry(factEntries.Count, postBorders, Config, null, false);
            return newEntry;
        }

        /// <summary>
        /// Создать временные рамки всех планируемых записей на основе настроек
        /// </summary>
        private DateRange[] CreateDateRanges()
        {
            var dateLimits = Config.DateRange;
            var period = Config.Period;

            List<DateRange> ranges = new List<DateRange>();
            var firstDate = dateLimits.From;

            var prevDate = firstDate;
            var nextDate = period.GetNextDate(prevDate);
            while(prevDate < dateLimits.To)
            {
                ranges.Add(new DateRange(prevDate, nextDate));
                prevDate = nextDate;
                nextDate = period.GetNextDate(prevDate);
                if (nextDate > dateLimits.To && dateLimits.To != prevDate)
                {
                    ranges.Add(new DateRange(prevDate, dateLimits.To));
                    break;
                }
            }

            return ranges.ToArray();
        }
    }
}
