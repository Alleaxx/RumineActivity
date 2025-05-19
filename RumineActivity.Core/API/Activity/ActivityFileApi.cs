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
        public event Action OnLoaded;

        private readonly IJsonFileInfo FileInfo;
        private readonly HttpClient Client;
        private readonly ILogger Logger;

        private string FilePath => FileInfo.Path;

        private Forum Forum { get; set; }

        public bool? IsLoaded { get; private set; }

        public ActivityFileApi(HttpClient client, ILogger logger, IJsonFileInfo fileInfo = null)
        {
            FileInfo = fileInfo;
            this.Client = client;
            this.Logger = logger;
        }


        /// <summary>
        /// Чтение файла и установка данных
        /// </summary>
        public async Task LoadDataAsync()
        {
            if (IsLoaded.HasValue)
            {
                return;
            }

            IsLoaded = false;
            Forum = await LoadForumFromFileAsync();
            IsLoaded = true;
            OnLoaded?.Invoke();
        }

        public IForum GetForum()
        {
            return Forum;
        }
        public IForum GetForum(DateRange range)
        {
            return new Forum(Forum.Posts.Where(p => range.IsDateInside(p.Date)));
        }

        private async Task<Forum> LoadForumFromFileAsync()
        {
            Logger.Log($"Читаем файл {FilePath}");

            string jsonContent = !FileInfo.IsWeb ? File.ReadAllText(FilePath) : await Client.GetStringAsync(FilePath);

            Logger.Log($"Файл {FilePath} прочитан и десериализован");

            var allPosts = FileInfo.GetPosts(jsonContent);

            Logger.Log($"Форум создан, {allPosts.Count()} постов");
            return new Forum(allPosts);
        }
    }
}
