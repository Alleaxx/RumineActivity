using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Запись 'За период написано сообщений'
    public class Entry
    {
        public override string ToString() => $"{Name} | {PostsDefault}";

        public DateRange Range { get; private set; }
        public string Name { get; set; }


        public bool SeparateDates { get; set; } = false;
        //xxxx
        public Entry(DateRange range, string name)
        {
            Name = name;
            Range = range;
            
            Posts[PostsSource.AllForum].Add(PostsType.Period, 0);
            Posts[PostsSource.AllForum].Add(PostsType.Total, 0);
            Posts[PostsSource.Topics].Add(PostsType.Period, 0);
            Posts[PostsSource.Topics].Add(PostsType.Total, 0);
            Posts[PostsSource.Difference].Add(PostsType.Period, 0);
        }

        //Периодический отчет
        public Entry(DateRange date,Post newer, Post older, TopicsMode topicMode, string dateFormat = "", double mod = 1) : this(newer, older, topicMode, dateFormat, mod)
        {
            Range = date;
        }
        //Фактический / периодический отчет
        public Entry(Post newer, Post older, TopicsMode topicMode, string dateFormat = "", double mod = 1)
        {
            SeparateDates = true;
            Range = new DateRange(newer, older);
            Name = Range.ToString(dateFormat);

            //ValueTotal = newer.ID;
            //ValueTopicTotal = newer.TopicIndex;


            //double forumChatAllowance = topicMode.OnlyNonChat ? 0.15 : 0.85;
            //int postsDifference = newer.ID - older.ID;
            //if (!topicMode.AllForum)
            //{
            //    bool anyIndexUnknown = newer.TopicIndex <= 0 || older.TopicIndex <= 0;
            //    if (!anyIndexUnknown && newer.TopicIndex - older.TopicIndex >= 0)
            //    {
            //        postsDifference = newer.TopicIndex - older.TopicIndex;
            //    }
            //    else
            //    {
            //        postsDifference = (int)(postsDifference * forumChatAllowance);
            //    }
            //}
            //PostsDefault = postsDifference * mod;



            double postsDifferenceAll = (newer.ID - older.ID) * mod;
            double postsDifferenceTopic = (Math.Max(1, newer.TopicIndex - older.TopicIndex)) * mod;

            Posts[PostsSource.AllForum].Add(PostsType.Period, postsDifferenceAll);
            Posts[PostsSource.AllForum].Add(PostsType.Total, newer.ID);
            Posts[PostsSource.Topics].Add(PostsType.Period, postsDifferenceTopic);
            Posts[PostsSource.Topics].Add(PostsType.Total, newer.TopicIndex);
            Posts[PostsSource.Difference].Add(PostsType.Period, postsDifferenceAll - postsDifferenceTopic);
            Posts[PostsSource.Difference].Add(PostsType.Total, newer.ID - newer.TopicIndex);

            switch (topicMode.Mode)
            {
                case TopicsModes.OnlyChat:
                case TopicsModes.Topic:
                case TopicsModes.NotChat:
                    EntryUse = PostsSource.Topics;
                    break;
                default:
                    EntryUse = PostsSource.AllForum;
                    break;

            }
        }

        private PostsSource EntryUse { get; set; } = PostsSource.AllForum;
        private Dictionary<PostsSource, Dictionary<PostsType, double>> Posts { get; set; } = new Dictionary<PostsSource, Dictionary<PostsType, double>>
        {
            [PostsSource.AllForum] = new Dictionary<PostsType, double>(),
            [PostsSource.Topics] = new Dictionary<PostsType, double>(),
            [PostsSource.Difference] = new Dictionary<PostsType, double>()
        };

        public double PostsDefault => GetPosts(EntryUse, PostsType.Period, MeasureMethods.Total);
        public double PostsRelative => GetPosts(EntryUse, PostsType.Period, MeasureMethods.ByDay);
        public double GetPosts(PostsType type, MeasureMethods methods)
        {
            return GetPosts(EntryUse, type, methods);
        }
        private double GetPosts(PostsSource source,PostsType posts, MeasureMethods interval)
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
    public enum PostsSource
    {
        AllForum, Topics, Difference
    }
    public enum PostsType
    {
        Total, Period
    }

    //За период написано сообщений суммарно
    //За период написано сообщений в темах
    //За период написана разница в суммарно и темах

    //Относительно для тем
    //Относительно для всего
    //Относительно для разницы

    //К последней дате написано сообщений всего
    //К последней дате написано сообщений в темах

}
