using RumineActivity.Core.API;
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
    public class ActivityFileApi : IActivityApi
    {
        private readonly IJsonFileInfo FileInfo;
        private readonly HttpClient Client;
        private readonly ILogger Logger;

        private string FilePath => FileInfo.Path;

        //Создается при первом обращении
        private Forum Forum => forum ?? forumDebug;

        private Forum forum;
        private Forum forumDebug;

        public event Action OnLoaded;

        public bool? IsLoaded { get; private set; }

        public ActivityFileApi(HttpClient client, ILogger logger, IJsonFileInfo fileInfo = null)
        {
            FileInfo = fileInfo;
            this.Client = client;
            this.Logger = logger;
        }


        //Процесс чтения файла и заполнения данными
        public async Task LoadData()
        {
            if (IsLoaded.HasValue)
            {
                return;
            }

            IsLoaded = false;
            forum = await LoadForumFromFile();
            IsLoaded = true;
            OnLoaded?.Invoke();
        }
        //Реализация
        public async Task<IForum> GetForum()
        {
            return Forum;
        }
        public async Task<IForum> GetForum(DateRange range)
        {
            return new Forum(Forum.Posts.Where(p => range.IsDateInside(p.Date)), Forum.Topics, Forum.Users);
        }

        private async Task<Forum> LoadForumFromFile()
        {
            Logger.Log($"Читаем файл {FilePath}");

            string jsonContent = !FileInfo.IsWeb ? System.IO.File.ReadAllText(FilePath) : await Client.GetStringAsync(FilePath);

            Logger.Log($"Файл {FilePath} прочитан и десериализован");

            var allPosts = FileInfo.GetPosts(jsonContent);
            var groupedTopics = allPosts.GroupBy(p => p.TopicId);
            var groupedUsers = allPosts.GroupBy(p => p.AuthorID);

            List<Topic> allTopics = new List<Topic>();
            List<User> allUsers = new List<User>();

            foreach (var topic in groupedTopics)
            {
                var newTopic = new Topic()
                {
                    Id = topic.First().TopicId,
                    Name = topic.First().TopicTitle
                };
                if (newTopic.Name != null && newTopic.Name.Contains("чат"))
                {
                    newTopic.IsChat = true;
                }
                allTopics.Add(newTopic);
            }
            foreach (var user in groupedUsers)
            {
                var newUser = new User()
                {
                    UserID = user.First().AuthorID,
                    Nick = user.First().AuthorNick
                };
                allUsers.Add(newUser);
            }
            Logger.Log($"Форум создан, {allPosts.Count()} постов, {allTopics.Count} тем, {allUsers.Count} юзеров");
            return new Forum(allPosts, allTopics, allUsers);
        }

        private void SetDebugForum()
        {
            var Posts = new List<Post>();
            var Topics = new List<Topic>();
            var Users = new List<User>();
            Topics.Add(new Topic() { Id = 1, IsChat = true, Name = "Форумный чат" });
            Topics.Add(new Topic() { Id = 2, IsChat = false, Name = "Не чат" });

            Posts.Add(new Post() { Id = 0, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 9, 1) });
            Posts.Add(new Post() { Id = 50, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 9, 15) });
            Posts.Add(new Post() { Id = 75, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 9, 30) });
            Posts.Add(new Post() { Id = 100, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 10, 10) });
            Posts.Add(new Post() { Id = 110, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 10, 20) });
            Posts.Add(new Post() { Id = 115, TopicId = 1, TopicIndex = 1, Date = new DateTime(2012, 10, 30) });

            forumDebug = new Forum(Posts, Topics, Users);
        }

    }
}
