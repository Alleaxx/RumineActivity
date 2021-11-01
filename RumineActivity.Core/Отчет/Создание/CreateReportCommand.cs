using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    interface IReportCommand
    {
        StatisticsReport Create();
    }


    //Макет для создания отчетов на основе промежутков между двумя постами
    public abstract class CreateReportCommand : IReportCommand
    {
        protected Post[] Posts { get; private set; }
        protected ReportCreatorOptions Options { get; private set; }

        public bool IsEmptyReport => Posts.Length < 2;

        public CreateReportCommand(IForum source, ReportCreatorOptions options)
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
            return new Entry(index, newPost, oldPost, Options);
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

    }
    class FactStrategy : ReportStrategy
    {

    }
    class PeriodicalStrategy : ReportStrategy
    {

    }
}
