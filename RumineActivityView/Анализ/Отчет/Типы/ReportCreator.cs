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

        public ReportCreator(IReportSource source, ReportOptions options)
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
        public abstract StatisticsReport Create();


        protected IEnumerable<Entry> SplitPosts(TimeSpan timeInterval)
        {
            TimeSpan maxDiffence = timeInterval;
            List<Entry> entries = new List<Entry>();
            int prevIndexDiff = 1;
            for (int i = 1; i < Posts.Length; i++)
            {
                Post prevPost = Posts[i - prevIndexDiff];
                Post currPost = Posts[i];

                TimeSpan dateDiff = currPost.Date - prevPost.Date;
                if (dateDiff >= maxDiffence)
                {
                    Entry newEntry = new Entry(currPost, prevPost, Options.TopicMode, Options.DateInterval.DateFormat);
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
                    Entry newEntry = new Entry(currPost, prevPost, Options.TopicMode, Options.DateInterval.DateFormat);
                    entries.Add(newEntry);
                }

            }
            return entries;
        }



    }


}
