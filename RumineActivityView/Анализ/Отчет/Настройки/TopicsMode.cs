using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum TopicsModes
    {
        All, Topic, OnlyChat, NotChat
    }
    public class TopicsMode
    {
        public override string ToString() => Name;

        public string Name { get; private set; }
        public TopicsModes Mode { get; private set; }
        public int TopicId { get; set; } = 1;

        public TopicsMode(TopicsModes mode)
        {
            Mode = mode;
            switch (mode)
            {
                case TopicsModes.All:
                    Name = "Весь форум";
                    break;
                case TopicsModes.NotChat:
                    Name = "Все помимо чата";
                    break;
                case TopicsModes.OnlyChat:
                    Name = "Только чат";
                    break;
                case TopicsModes.Topic:
                    Name = "Конкретная тема";
                    break;
            }
        }
        public TopicsMode(int topic) : this(TopicsModes.Topic)
        {
            TopicId = topic;
        }

        public bool Filter(Post post, IEnumerable<Topic> topics)
        {
            var topic = topics.FirstOrDefault(t => t.ID == post.TopicID);
            switch (Mode)
            {
                case TopicsModes.All:
                    return true;
                case TopicsModes.NotChat:
                case TopicsModes.OnlyChat:
                    if (topic.IsChat.HasValue && topic.IsChat.Value)
                        return true;
                    else
                        return false;
                case TopicsModes.Topic:
                    return TopicId == post.TopicID;
            }
            return true;
        }
    }
}
