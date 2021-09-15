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
                Console.WriteLine("Создание отчета");
                return Construct();
            }
        }
        protected abstract StatisticsReport Construct();


        protected Entry[] SplitPosts()
        {
            return SplitPosts(new TimeSpan(0));
        }
        protected Entry[] SplitPosts(TimeSpan maxDifference)
        {
            bool ignoreDifference = maxDifference.Ticks == 0;
            List<Entry> entries = new List<Entry>();
            int prevIndexDiff = 1;
            for (int i = 1; i < Posts.Length; i++)
            {
                bool isLast = i == Posts.Length - 1;
                Post prevPost = Posts[i - prevIndexDiff];
                Post currPost = Posts[i];
                DateRange range = new DateRange(currPost, prevPost);

                if (ignoreDifference || range.Diff >= maxDifference || isLast)
                {
                    AddEntry(prevPost, currPost);
                }
                else
                {
                    prevIndexDiff++;
                }
            }
            return entries.ToArray();


            void AddEntry(Post prevPost, Post currPost)
            {
                Entry newEntry = CreateFactEntry(entries.Count, currPost, prevPost);
                entries.Add(newEntry);
                prevIndexDiff = 1;
            }
        }

        private Entry CreateFactEntry(int index, Post newPost, Post oldPost)
        {
            return new Entry(index, newPost, oldPost, Options.Period.DateFormat, Options.TopicMode.Mode);
        }
    }



    interface IReportInfo
    {
        Post[] Posts { get; }
        DateRange DateRange { get; }
        Period Period { get; }
        PostSource Source { get; }

    }
    //Перенести опции отчета в создатель полностью
    //Выделить стратегию создания отчетов

    interface IReportStrategy
    {
        StatisticsReport CreateReport(ReportCreatorOptions options, Post[] posts);
    }
    abstract class ReportStrategy
    {
        protected Entry[] SplitPosts(Post[] posts)
        {
            return SplitPosts(posts,new TimeSpan(0));
        }
        protected Entry[] SplitPosts(Post[] Posts, TimeSpan maxDifference)
        {
            List<Entry> entries = new List<Entry>();
            int prevIndexDiff = 1;
            for (int i = 1; i < Posts.Length; i++)
            {
                Post prevPost = Posts[i - prevIndexDiff];
                Post currPost = Posts[i];

                TimeSpan dateDiff = currPost.Date - prevPost.Date;
                if (dateDiff >= maxDifference)
                {
                    Entry newEntry = CreateFactEntry(null,entries.Count, currPost, prevPost);
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
                    Entry newEntry = CreateFactEntry(null,entries.Count, currPost, prevPost);
                    entries.Add(newEntry);
                }
            }
            return entries.ToArray();


        }
        private Entry CreateFactEntry(ReportCreatorOptions Options,int index, Post newPost, Post oldPost)
        {
            return new Entry(index, newPost, oldPost, Options.Period.DateFormat, Options.TopicMode.Mode);
        }

    }
    class FactStrategy : ReportStrategy
    {

    }
    class PeriodicalStrategy : ReportStrategy
    {

    }
}
