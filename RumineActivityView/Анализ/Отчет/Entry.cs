using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum PostSources
    {
        AllForum, Topics, Difference
    }
    //Запись 'За период написано сообщений'
    public class Entry
    {
        public override string ToString() => $"{Name} | {PostsDefault}";


        public DateRange Range { get; private set; }
        public string Name { get; private set; }
        public int Index { get; private set; }


        //Разделение дат в интерфейсе
        public bool SeparateDates { get; private set; } = false;
        public string DateFormat { get; private set; } = "dd MMMM yyyy";


        //Периодический отчет
        public Entry(int index,DateRange range, string format, TopicsMode mode)
        {
            Index = index;
            SeparateDates = false;
            Range = range;

            SetFormat(format);
            SetMode(mode);
        }

        //Периодический отчет Legacy
        public Entry(int index,string format, DateRange range, Post newer, Post older, TopicsMode topicMode, double mod)
        {
            Index = index;
            SeparateDates = false;
            Range = range;
            SetFormat(format);

            SetPostsDiff(newer, older, mod);
            SetMode(topicMode);
        }
        
        //Фактический отчет
        public Entry(int index, string format, Post newer, Post older, TopicsMode topicMode)
        {
            Index = index;
            SeparateDates = true;
            Range = new DateRange(newer, older);
            SetFormat(format);

            SetPostsDiff(newer, older, 1);
            SetMode(topicMode);
        }



        //Методы создания отчета
        private void SetPostsDiff(Post newer, Post older, double mod)
        {
            double postsDifferenceAll = (newer.ID - older.ID) * mod;
            double postsDifferenceTopic = (Math.Max(1, newer.TopicIndex - older.TopicIndex)) * mod;

            Posts[PostSources.AllForum][PostOutputs.PeriodDifference] = postsDifferenceAll;
            Posts[PostSources.AllForum][PostOutputs.PeriodEnd] = newer.ID;
            Posts[PostSources.Topics][PostOutputs.PeriodDifference] = postsDifferenceTopic;
            Posts[PostSources.Topics][PostOutputs.PeriodEnd] = newer.TopicIndex;
            Posts[PostSources.Difference][PostOutputs.PeriodDifference] = postsDifferenceAll - postsDifferenceTopic;
            Posts[PostSources.Difference][PostOutputs.PeriodEnd] = newer.ID - newer.TopicIndex;
        }
        private void SetMode(TopicsMode mode)
        {
            switch (mode.Mode)
            {
                case TopicsModes.OnlyChat:
                case TopicsModes.Topic:
                case TopicsModes.NotChat:
                    EntryUse = PostSources.Topics;
                    break;
                default:
                    EntryUse = PostSources.AllForum;
                    break;
            }
        }
        private void SetFormat(string dateFormat)
        {
            DateFormat = dateFormat;
            if (!SeparateDates)
            {
                Name = Range.From.ToString(dateFormat);
            }
            else
            {
                Name = Range.ToString(dateFormat);
            }
        }



        private PostSources EntryUse = PostSources.AllForum;
        private Dictionary<PostSources, Dictionary<PostOutputs, double>> Posts { get; set; } = new Dictionary<PostSources, Dictionary<PostOutputs, double>>
        {
            [PostSources.AllForum] = new Dictionary<PostOutputs, double>()
                { [PostOutputs.PeriodDifference] = 1, [PostOutputs.PeriodEnd] = 1 },
            [PostSources.Topics] = new Dictionary<PostOutputs, double>()
                { [PostOutputs.PeriodDifference] = 1, [PostOutputs.PeriodEnd] = 1 },
            [PostSources.Difference] = new Dictionary<PostOutputs, double>()
                { [PostOutputs.PeriodDifference] = 1, [PostOutputs.PeriodEnd] = 1 },
        };
        public void Set(PostOutputs type, double val)
        {
            Set(EntryUse, type, val);
        }
        private void Set(PostSources source, PostOutputs type, double val)
        {
            Posts[source][type] = val;
        }


        public double PostsDefault => GetPosts(EntryUse, PostOutputs.PeriodDifference, MeasureMethods.Total);
        public double PostsRelative => GetPosts(EntryUse, PostOutputs.PeriodDifference, MeasureMethods.ByDay);
        public double GetPosts(PostOutputs type, MeasureMethods methods)
        {
            return GetPosts(EntryUse, type, methods);
        }
        private double GetPosts(PostSources source, PostOutputs posts, MeasureMethods interval)
        {
            double val = Posts[source][posts];
            switch (interval)
            {
                case MeasureMethods.ByDay:
                    val /= Range.Diff.TotalDays;
                    break;
                case MeasureMethods.ByMonth:
                    val /= Range.Diff.TotalDays / 30;
                    break;
                case MeasureMethods.ByHour:
                    val /= Range.Diff.TotalHours;
                    break;
            }
            return val;
        }
    }
}
