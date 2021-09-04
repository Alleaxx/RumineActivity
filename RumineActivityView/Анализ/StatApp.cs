using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class StatApp
    {
        private static readonly StatApp app = new StatApp();
        public static StatApp App => app;

        public bool OfflineMode { get; set; } = true;
        public NewPost EditingPost { get; private set; }


        public List<Post> Posts { get; private set; }
        public List<Topic> Topics { get; private set; }

        public void Add(Post newPost)
        {
            Posts.Add(newPost);
        }
        public void Add(Topic newTopic)
        {
            Topics.Add(newTopic);
        }

        public void Remove(Post post)
        {
            Posts.Remove(post);
        }
        public void Remove(Topic topic)
        {
            Topics.Remove(topic);
        }

        public StatApp()
        {
            EditingPost = new NewPost();
            Posts = new List<Post>();
            Topics = new List<Topic>();

            if (OfflineMode)
            {
                Add(new Topic() { ID = 15361, IsChat = true, Name = "Форумный чат" });
                Add(new Post() { ID = 1239050, Date = new DateTime(2016, 8, 3, 14, 52, 0), TopicID = 15361, TopicIndex = 1 });
                Add(new Post() { ID = 1326742, Date = new DateTime(2018, 5, 28, 8, 32, 0), TopicID = 15361, TopicIndex = 79981 });
                Add(new Post() { ID = 1458637, Date = new DateTime(2021, 9, 4, 16, 33, 0), TopicID = 15361, TopicIndex = 186609 });
                Add(new Post() { ID = -1, TopicID = -1, TopicIndex = -1, Date = new DateTime(2021, 01, 01, 01, 01, 0) });
            }
        }
    }
}
