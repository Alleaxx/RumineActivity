using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    //АПИ с промежуточным хранением данных
    public class ActivityStorageApi : IActivityApi
    {
        public override string ToString()
        {
            return $"Промежуточный {(UseApiChangeData ? "nx" : "1x")} - {Main}";
        }

        private readonly IActivityApi Main;

        //Обращаться к АПИ с изменением данных
        private bool UseApiChangeData { get; set; }

        private List<Post> Posts { get; set; }
        private List<Topic> Topics { get; set; }

        public ActivityStorageApi(IActivityApi main, bool useConfirm)
        {
            Main = main;
            UseApiChangeData = useConfirm;
            SetDefaultData();
            Load();
        }
        private async Task Load()
        {
            var info = await Main.GetForum();
            if(info != null)
            {
                Posts = new List<Post>(info.Posts);
                Topics = new List<Topic>(info.Topics);
            }
        }
        private void SetDefaultData()
        {
            Posts = new List<Post>();
            Topics = new List<Topic>();
            Topics.Add(new Topic() { Id = 1, Name = "Ой!" });
            Posts.Add(new Post() { Id = 1, TopicId = 1, TopicIndex = 1, Date = DateTime.Now.AddDays(-365) });
            Posts.Add(new Post() { Id = 25, TopicId = 1, TopicIndex = 25, Date = DateTime.Now.AddDays(-200) });
            Posts.Add(new Post() { Id = 75, TopicId = 1, TopicIndex = 75, Date = DateTime.Now.AddDays(-100) });
            Posts.Add(new Post() { Id = 100, TopicId = 1, TopicIndex = 100, Date = DateTime.Now });
        }

        public async Task<Post> Create(Post post)
        {
            if (UseApiChangeData)
            {
                var result = await Main.Create(post);
                if (result == null)
                {
                    return default;
                }
            }
            Posts.Add(post);
            return post;
        }
        public async Task<Topic> Create(Topic topic)
        {
            if (UseApiChangeData)
            {
                var result = await Main.Create(topic);
                if (result == null)
                {
                    return default;
                }
            }
            Topics.Add(topic);
            return topic;
        }


        public async Task<Post> Delete(Post post)
        {
            if (UseApiChangeData)
            {
                var result = await Main.Delete(post);
                if (result == null)
                {
                    return default;
                }
            }
            Posts.Remove(post);
            return post;
        }
        public async Task<Topic> Delete(Topic topic)
        {
            if (UseApiChangeData)
            {
                var result = await Main.Delete(topic);
                if (result == null)
                {
                    return default;
                }
            }
            Topics.Remove(topic);
            return topic;
        }

        public async Task<IForumSource> GetForum()
        {
            return new ForumSource(Posts, Topics);
        }
        public async Task<IForumSource> GetForum(DateRange range)
        {
            return await GetForum();
        }

        public async Task<Post> Update(Post post)
        {
            if (UseApiChangeData)
            {
                var result = await Main.Update(post);
                if (result == null)
                {
                    return default;
                }
            }
            return post;
        }
        public async Task<Topic> Update(Topic topic)
        {
            if (UseApiChangeData)
            {
                var result = await Main.Update(topic);
                if (result == null)
                {
                    return default;
                }
            }
            return topic;
        }
    }
}
