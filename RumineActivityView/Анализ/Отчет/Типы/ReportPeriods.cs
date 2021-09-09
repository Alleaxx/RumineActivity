using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет с РАВНЫМИ периодами, достраивает недостающие данные
    public class ReportPeriods : ReportCreator
    {
        public ReportPeriods(IReportSource source, ReportOptions options) : base(source, options)
        {

        }
        public override StatisticsReport Create()
        {
            if (IsEmptyReport)
            {
                return new StatisticsReport();
            }

            var dateInterval = Options.DateInterval;
            var dateRange = Options.DateRange;
            var splittedEntries = SplitPosts(new TimeSpan(0));

            List<Entry> newEntries = new List<Entry>();

            DateTime fromDate = Posts.First().Date;
            DateTime toDate = new DateTime();
            do
            {
                Entry entry = dateInterval.GetNextEntry(fromDate);
                DateRange range = entry.Range;
                toDate = range.To;

                var innerEntries = splittedEntries.Select(e => new EntryDateRangeFraction(range, e)).Where(f => f.Fraction > 0);

                double written = innerEntries.Sum(PostsWritten);
                //entry.PostsDefault = innerEntries.Sum(PostsWritten);
                double PostsWritten(EntryDateRangeFraction obj)
                {
                    Entry e = obj.Entry;
                    double fraction = obj.Fraction;

                    if (fraction == 1)
                        return e.PostsDefault;
                    else
                        return e.PostsDefault * fraction;
                }
                newEntries.Add(entry);

                fromDate = toDate;
            }
            while (dateRange.IsDateInside(toDate));

            return new StatisticsReport($"Отчет по периодам", newEntries, Options);
        }
    }
    
    //НЕ УЧИТЫВАЕТ МНОЖЕСТВЕННЫЕ ТЕМЫ
    public class ReportPeriods2 : ReportCreator
    {
        public ReportPeriods2(IReportSource source, ReportOptions options) : base(source, options)
        {

        }

        //--Для общего количества постов или одной темы
        //находим самый близкий к началу периода
        //находим самый близкий к концу
        //делаем стандартную запись
        //узнаем % периода (напр 60%), умножаем значения на мод. (0ю6)
        //--Для нескольких тем
        //???
        public override StatisticsReport Create()
        {
            if (IsEmptyReport)
            {
                return new StatisticsReport();
            }
            DateInterval dateInterval = Options.DateInterval;
            DateRange dateRange = Options.DateRange;

            List<Entry> newEntries = new List<Entry>();

            DateTime fromDate = Posts.First().Date;
            DateTime toDate = dateInterval.GetNextDate(fromDate);
            while (true)
            {
                DateRange range = new DateRange(fromDate, toDate);
                var posts = GetPosts(range);
                if (posts.FromNearestPost == posts.ToNearestPost)
                {
                    break;
                }
                
                Entry newEntry = new Entry(range,posts.ToNearestPost, posts.FromNearestPost, Options.TopicMode, Options.DateInterval.DateFormat, posts.Mod);
                newEntry.SeparateDates = false;
                newEntry.Name = fromDate.ToString(dateInterval.DateFormat);
                newEntries.Add(newEntry);

                fromDate = toDate;
                toDate = dateInterval.GetNextDate(fromDate);
            }
            return new StatisticsReport($"Отчет по периодам 2", newEntries, Options);
        }

        public PostFraction GetPosts(DateRange range)
        {
            return GetPosts(Posts, range);
        }
        public PostFraction GetPosts(IEnumerable<Post> posts,DateRange range)
        {
            Post minBefore = posts.LastOrDefault(p => p.Date <= range.From);
            //Post minAfter = posts.FirstOrDefault(p => p.Date >= range.From);
            Post maxAfter = posts.FirstOrDefault(p => p.Date >= range.To) ?? posts.Last();
            //Post maxBefore = posts.LastOrDefault(p => p.Date <= range.To);

            Post min = minBefore, max = maxAfter;

            return new PostFraction(min, max, range);

        }
    }
    public class PostFraction
    {
        public override string ToString() => $"{FromNearestPost} ({FromNearestPost.Date:dd.MM.yy}) {ToNearestPost} ({ToNearestPost.Date:dd.MM.yy}) - {Mod} для исходного {SourceRange}";
        public DateRange SourceRange { get; set; }
        public Post FromNearestPost { get; set; }
        public Post ToNearestPost { get; set; }
        public double Mod { get; set; } = 1;

        public PostFraction(Post a, Post b, DateRange sourceRange)
        {

            FromNearestPost = a;
            ToNearestPost = b;
            SourceRange = sourceRange;

            //Настоящий промежуток может быть БОЛЬШЕ требуемого, но никогда меньшего
            DateRange realRange = new DateRange(b, a);
            Mod = realRange.GetFraction(sourceRange);
            if (double.IsNaN(Mod))
            {
                Mod = 0;
            }
        }
    }
}
