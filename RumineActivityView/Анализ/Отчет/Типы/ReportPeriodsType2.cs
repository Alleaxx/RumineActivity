using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{

    //НЕ УЧИТЫВАЕТ МНОЖЕСТВЕННЫЕ ТЕМЫ
    public class ReportPeriods2 : ReportCreator
    {
        public ReportPeriods2(IForumSource source, ReportOptions options) : base(source, options)
        {

        }

        //--Для общего количества постов или одной темы
        //находим самый близкий к началу периода
        //находим самый близкий к концу
        //делаем стандартную запись
        //узнаем % периода (напр 60%), умножаем значения на мод. (0ю6)
        //--Для нескольких тем
        //???
        protected override StatisticsReport Construct()
        {
            Period period = Options.Period;
            DateRange dateRange = Options.DateRange;

            List<Entry> newEntries = new List<Entry>();

            DateTime fromDate = Posts.First().Date;
            DateTime toDate = period.GetNextDate(fromDate);
            while (true)
            {
                DateRange range = new DateRange(fromDate, toDate);
                var posts = GetPosts(range);
                if (posts.FromNearestPost == posts.ToNearestPost)
                {
                    break;
                }

                Entry newEntry = new Entry(newEntries.Count, period.DateFormat, range, posts.ToNearestPost, posts.FromNearestPost, Options.TopicMode, posts.Mod);
                newEntries.Add(newEntry);

                fromDate = toDate;
                toDate = period.GetNextDate(fromDate);
            }
            return new StatisticsReport($"Отчет по периодам - {period.Name}", newEntries, Options);
        }

        public PostFraction GetPosts(DateRange range)
        {
            return GetPosts(Posts, range);
        }
        public PostFraction GetPosts(IEnumerable<Post> posts, DateRange range)
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
