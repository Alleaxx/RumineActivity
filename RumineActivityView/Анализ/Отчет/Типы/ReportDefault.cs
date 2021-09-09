using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет по существующим записям
    public class ReportDefault : ReportCreator
    {
        public ReportDefault(IReportSource source, ReportOptions options) : base(source, options)
        {

        }
        public override StatisticsReport Create()
        {
            if (IsEmptyReport)
            {
                return new StatisticsReport();
            }
            else
            {
                var entries = SplitPosts(Options.DateInterval.TimeInterval);
                return new StatisticsReport($"Отчет по записям", entries, Options);
            }
        }

    }
    
    //Отчет по существующим записям
    public class ReportDefault2 : ReportCreator
    {
        public ReportDefault2(IReportSource source, ReportOptions options) : base(source, options)
        {

        }
        public override StatisticsReport Create()
        {
            if (IsEmptyReport)
            {
                return new StatisticsReport();
            }
            else
            {
                TimeSpan maxDiffence = Options.DateInterval.TimeInterval;
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
                    if (last && !entries.Any())
                    {
                        Entry newEntry = new Entry(currPost, prevPost, Options.TopicMode, Options.DateInterval.DateFormat);
                        entries.Add(newEntry);
                    }

                }
                return new StatisticsReport($"Отчет по записям", entries, Options);
            }
        }

    }
}
