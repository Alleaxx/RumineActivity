using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum PostSources
    {
        All, Topic, OnlyChat, NotChat
    }
    public abstract class PostSource : Named
    {
        public readonly PostSources Mode;
        public override string ToString() => Name;
        public int TopicId { get; set; } = 1;


        public static PostSource Create(PostSources modes)
        {
            switch (modes)
            {
                case PostSources.All:
                    return new PostSourceAll();
                case PostSources.NotChat:
                    return new PostSourceNotChat();
                case PostSources.OnlyChat:
                    return new PostSourceChats();
                case PostSources.Topic:
                    return new PostSourceTopic();
                default:
                    throw new Exception("Такого режима не существует");
            }
        }
        public static PostSource Create(int id)
        {
            return new PostSourceTopic(id);
        }
        protected PostSource(PostSources mode)
        {
            Mode = mode;
        }

        public abstract bool Filter(Post post, IEnumerable<Topic> topics);
        protected bool FilterOnlyChats(Post post, IEnumerable<Topic> topics)
        {
            var topic = topics.FirstOrDefault(t => t.ID == post.TopicID);
            return topic != null && topic.IsChat;
        }
    }

    public class PostSourceAll : PostSource
    {
        public PostSourceAll() : base(PostSources.All)
        {
            Name = "Весь форум";
        }
        public override bool Filter(Post post, IEnumerable<Topic> topics)
        {
            return true;
        }
    }
    public class PostSourceTopic : PostSource
    {
        public PostSourceTopic(int id = 1) : base(PostSources.Topic)
        {
            Name = "Конкретная тема";
            TopicId = id;
        }
        public override bool Filter(Post post, IEnumerable<Topic> topics)
        {
            return TopicId == post.TopicID;
        }
    }
    
    public class PostSourceChats : PostSource
    {
        public PostSourceChats() : base(PostSources.OnlyChat)
        {
            Name = "Только форумный чат";
        }
        public override bool Filter(Post post, IEnumerable<Topic> topics)
        {
            return FilterOnlyChats(post, topics);
        }
    }
    public class PostSourceNotChat : PostSource
    {
        public PostSourceNotChat() : base(PostSources.NotChat)
        {
            Name = "Все помимо чата";
        }
        public override bool Filter(Post post, IEnumerable<Topic> topics)
        {
            return FilterOnlyChats(post, topics);
        }
    }
}
