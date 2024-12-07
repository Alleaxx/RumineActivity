using RumineActivity.Core.Logging;
using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RumineActivity.Core.API
{
    public class ActivityWebApi : WebApi, IActivityApi
    {
        public override string ToString()
        {
            return $"Web-Api источник {Path}";
        }

        private readonly string MsgPath;
        private readonly string TopicPath;

        public event Action OnLoaded;

        public bool? IsLoaded => true;

        public ActivityWebApi(HttpClient client, ILogger logger, string path = "https://localhost:44341") : base(client, logger, path)
        {
            MsgPath = $"{Path}/Messages";
            TopicPath = $"{Path}/Topics";
        }
        public async Task LoadData()
        {
            OnLoaded?.Invoke();
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

            return success ? new Forum(posts, topics, null) : default;
        }
        public async Task<IForum> GetForum(DateRange range)
        {
            return await GetForum();
        }
    }
}
