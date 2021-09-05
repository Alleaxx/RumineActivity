using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Запись 'За период написано сообщений'
    public class Entry
    {
        public override string ToString() => $"{Name} | {Value}";

        public DateRange Range { get; private set; }
        public string Name { get; private set; }
        public double Value { get; set; }
        public double ValueRelative => Value / Range.Diff.TotalHours;


        private string Format { get; set; }
        public bool SeparateDates { get; set; } = false;
        public Entry(DateRange range, string name)
        {
            Name = name;
            Range = range;
        }
        public Entry(Post newer, Post older, TopicsMode topicMode, string dateFormat = "")
        {
            SeparateDates = true;
            Range = new DateRange(newer, older);
            Name = Range.ToString(dateFormat);

            double forumChatAllowance = topicMode.OnlyNonChat ? 0.15 : 0.85;
            int postsDifference = newer.ID - older.ID;
            if (!topicMode.AllForum)
            {
                bool anyIndexUnknown = newer.TopicIndex <= 0 || older.TopicIndex <= 0;
                if (!anyIndexUnknown && newer.TopicIndex - older.TopicIndex >= 0)
                {
                    postsDifference = newer.TopicIndex - older.TopicIndex;
                }
                else
                {
                    postsDifference = (int)(postsDifference * forumChatAllowance);
                }
            }
            Value = postsDifference;
        }


        public double ValuePerHour => Value / Range.Diff.TotalHours;
        public double ValuePerDay => Value / Range.Diff.TotalDays;
        public double ValuePerMonth => Value / (Range.Diff.TotalDays / 30);
    }

}
