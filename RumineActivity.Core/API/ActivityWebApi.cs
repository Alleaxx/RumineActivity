using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public interface IActivityApi
    {
        Task<Post> Create(Post post);
        Task<Topic> Create(Topic topic);
        Task<Post> Update(Post post);
        Task<Topic> Update(Topic topic);
        Task<Post> Delete(Post post);
        Task<Topic> Delete(Topic topic);
        Task<IForum> GetForum();
        Task<IForum> GetForum(DateRange range);
    }
    public class ActivityWebApi : WebApi, IActivityApi
    {
        public override string ToString()
        {
            return $"Web-Api источник {Path}";
        }

        private readonly string MsgPath;
        private readonly string TopicPath;
        public ActivityWebApi(HttpClient client, string path = "https://localhost:44341") : base(client, path)
        {
            MsgPath = $"{Path}/Messages";
            TopicPath = $"{Path}/Topics";
        }
        public async Task<Post> Create(Post post)
        {
            return await Create(post, MsgPath);
        }
        public async Task<Topic> Create(Topic topic)
        {
            return await Create(topic, TopicPath);
        }


        public async Task<Post> Update(Post post)
        {
            return await Update(post, MsgPath);
        }
        public async Task<Topic> Update(Topic topic)
        {
            return await Update(topic, TopicPath);
        }


        public async Task<Post> Delete(Post post)
        {
            return await Delete(post, MsgPath, post.Id);
        }
        public async Task<Topic> Delete(Topic topic)
        {
            return await Delete(topic, TopicPath, topic.Id);
        }



        private async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await Get<Post>(MsgPath);
        }
        private async Task<IEnumerable<Topic>> GetAllTopics()
        {
            return await Get<Topic>(TopicPath);
        }

        public async Task<IForum> GetForum()
        {
            var posts = await GetAllPosts();
            var topics = await GetAllTopics();
            bool success = posts != null && topics != null;

            return success ? new Forum(posts, topics) : default;
        }
        public async Task<IForum> GetForum(DateRange range)
        {
            return await GetForum();
        }
    }

    public class ActivityWebStorageLoggedApi : ActivityStorageApi
    {
        public ActivityWebStorageLoggedApi(HttpClient client) : base(new ActivityLoggedApi(new ActivityWebApi(client)), false)
        {

        }
    }
}
