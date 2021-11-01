using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public interface IDataEditor
    {
        List<Post> Posts { get; }


        Task Add(Post newPost);
        Task Add(Topic newTopic);


        Task Update(Post post);
        Task Update(Topic topic);


        Task Remove(Post post);
        Task Remove(Topic topic);

    }
    public class DataEditor : IDataEditor
    {
        private readonly IActivityApi Api;

        public List<Post> Posts { get; private set; } = new List<Post>();

        public DataEditor(IActivityApi api)
        {
            Api = api;
            LoadData();
        }
        private async void LoadData()
        {
            var info = await Api.GetForum();
            var posts = info.Posts;

            if(posts != null)
            {
                Posts = new List<Post>(posts);
            }
        }


        public async Task Add(Post newPost)
        {
            await Api.Create(newPost);
        }
        public async Task Add(Topic newTopic)
        {
            await Api.Create(newTopic);
        }

        public async Task Update(Post post)
        {
            await Api.Update(post);
        }
        public async Task Update(Topic topic)
        {
            await Api.Update(topic);
        }

        public async Task Remove(Post post)
        {
            var res = await Api.Delete(post);
        }
        public async Task Remove(Topic topic)
        {
            var res = await Api.Delete(topic);
        }
    }
}
