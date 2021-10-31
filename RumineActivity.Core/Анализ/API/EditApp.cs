using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class EditApp
    {
        private readonly ActivityWebApi WebApi;

        public NewPost EditingPost { get; private set; }
        public List<Post> Posts { get; private set; } = new List<Post>();
        public List<Topic> Topics { get; private set; } = new List<Topic>();

        public EditApp(ActivityWebApi api)
        {
            WebApi = api;
            EditingPost = new NewPost(this);
            LoadData();
        }
        private async void LoadData()
        {
            var posts = await WebApi.GetAllPosts();
            var topics = await WebApi.GetAllTopics();

            if(posts != null && topics != null)
            {
                Posts = new List<Post>(posts);
                Topics = new List<Topic>(topics);
            }
        }


        public async Task Add(Post newPost)
        {
            var res = await WebApi.Create(newPost, WebApi.MsgPath());
            if (res != null)
            {
                AddLocal(newPost);
            }
        }
        public async Task Add(Topic newTopic)
        {
            var res = await WebApi.Create(newTopic, WebApi.TopicPath());
            if (res != null)
            {
                AddLocal(newTopic);
            }
        }

        public async Task Update(Post post)
        {
            await WebApi.Update(post, WebApi.MsgPath());
        }
        public async Task Update(Topic topic)
        {
            await WebApi.Update(topic, WebApi.TopicPath());
        }

        public async Task Remove(Post post)
        {
            var res = await WebApi.Delete(post, WebApi.MsgPath(), post.Id);
            if (res != null)
            {
                RemoveLocal(post);
            }
        }
        public async Task Remove(Topic topic)
        {
            var res = await WebApi.Delete(topic, WebApi.TopicPath(), topic.Id);
            if (res != null)
            {
                RemoveLocal(topic);
            }
        }


        private void AddLocal(Post post)
        {
            Posts.Add(post);
        }
        private void AddLocal(Topic topic)
        {
            Topics.Add(topic);
        }
        private void RemoveLocal(Post post)
        {
            Posts.Remove(post);
        }
        private void RemoveLocal(Topic topic)
        {
            Topics.Remove(topic);
        }

    }
}
