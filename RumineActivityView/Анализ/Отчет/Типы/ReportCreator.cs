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
        protected ReportCreatorOptions Options { get; private set; }

        public bool IsEmptyReport => Posts.Length < 2;

        public ReportCreator(IForumSource source, ReportCreatorOptions options)
        {
            if (options != null)
            {
                Options = options;
            }
            else
            {
                Options = new ReportCreatorOptions();
            }
            
            Posts = Options.GetPostsSource(source);
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


        protected Entry[] SplitPosts()
        {
            return SplitPosts(new TimeSpan(0), true);
        }
        protected Entry[] SplitPosts(TimeSpan maxDifference, bool ignoreDifference = false)
        {
            List<Entry> entries = new List<Entry>();
            int prevIndexDiff = 1;
            for (int i = 1; i < Posts.Length; i++)
            {
                Post prevPost = Posts[i - prevIndexDiff];
                Post currPost = Posts[i];

                TimeSpan dateDiff = currPost.Date - prevPost.Date;
                if (ignoreDifference || dateDiff >= maxDifference)
                {
                    Entry newEntry = CreateFactEntry(entries.Count, currPost, prevPost);
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
                    Entry newEntry = CreateFactEntry(entries.Count, currPost, prevPost);
                    entries.Add(newEntry);
                }
            }
            return entries.ToArray();
        }

        private Entry CreateFactEntry(int index, Post newPost, Post oldPost)
        {
            return new Entry(index, newPost, oldPost, Options.Period.DateFormat, Options.TopicMode.Mode);
        }
    }
}
