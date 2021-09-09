using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum TopicsModes
    {
        All, OnlyChat, NotChat, Topics
    }
    public class TopicsMode
    {
        public override string ToString() => Name;

        public string Name { get; private set; }
        public TopicsModes Mode { get; private set; }
        public bool AllForum { get; private set; }

        public bool OnlyChat { get; private set; }
        public bool OnlyNonChat { get; private set; }
        public int TopicId { get; set; } = 1;

        public TopicsMode(TopicsModes mode)
        {
            Mode = mode;
            Name = "Режим темы";
            AllForum = false;
            switch (mode)
            {
                case TopicsModes.All:
                    Name = "Весь форум";
                    AllForum = true;
                    break;
                case TopicsModes.NotChat:
                    Name = "Все помимо чата";
                    OnlyNonChat = true;
                    break;
                case TopicsModes.OnlyChat:
                    Name = "Только чат";
                    OnlyChat = true;
                    break;
                case TopicsModes.Topics:
                    Name = "Конкретная тема";
                    break;
            }
        }
        public TopicsMode(int topic) : this(TopicsModes.Topics)
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
                    if (topic.IsChat.HasValue && topic.IsChat.Value)
                        return false;
                    else
                        return true;
                case TopicsModes.OnlyChat:
                    if (topic.IsChat.HasValue && topic.IsChat.Value)
                        return true;
                    else
                        return false;
                case TopicsModes.Topics:
                    return TopicId == post.TopicID;
            }
            return true;
        }
    }
}
