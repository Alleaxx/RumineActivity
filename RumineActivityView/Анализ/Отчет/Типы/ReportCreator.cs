using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    interface IReportCreator
    {
        StatisticsReport Create();
    }


    //Макет для создания отчетов на основе промежутков между двумя постами
    public abstract class ReportCreator : IReportCreator
    {
        protected Post[] Posts { get; private set; }
        protected Topic[] Topics { get; private set; }
        protected ReportOptions Options { get; private set; }

        public bool IsEmptyReport => Posts.Length < 2;

        public ReportCreator(IForumSource source, ReportOptions options)
        {
            if (options != null)
            {
                Options = options;
            }
            else
            {
                Options = new ReportOptions();
            }

            Posts = source.Posts
                .Where(p => Options.TopicMode.Filter(p, source.Topics))
                .Where(p => options.DateRange.IsDateInside(p.Date))
                .OrderBy(d => d.Date)
                .ToArray();
            Topics = source.Topics
                .OrderBy(d => d.ID)
                .ToArray();
        }
        public StatisticsReport Create()
        {
            if (IsEmptyReport)
            {
                return new StatisticsReport();
            }
            else
            {
                return Construct();
            }
        }
        protected abstract StatisticsReport Construct();


        protected IEnumerable<Entry> SplitPosts()
        {
            return SplitPosts(new TimeSpan(0), true);
        }
        protected IEnumerable<Entry> SplitPosts(TimeSpan maxDifference, bool ignoreDifference = false)
        {
            string dateFormat = Options.Period.DateFormat;

            List<Entry> entries = new List<Entry>();
            int prevIndexDiff = 1;
            for (int i = 1; i < Posts.Length; i++)
            {
                Post prevPost = Posts[i - prevIndexDiff];
                Post currPost = Posts[i];

                TimeSpan dateDiff = currPost.Date - prevPost.Date;
                if (ignoreDifference || dateDiff >= maxDifference)
                {
                    Entry newEntry = new Entry(dateFormat, currPost, prevPost, Options.TopicMode);
                    entries.Add(newEntry);
                    prevIndexDiff = 1;
                }
                else
                {
                    prevIndexDiff++;
                }

                bool last = i == Posts.Length - 1;
                if(last && !entries.Any())
                {
                    Entry newEntry = new Entry(dateFormat, currPost, prevPost, Options.TopicMode);
                    entries.Add(newEntry);
                }

            }
            return entries;
        }
    }


}
