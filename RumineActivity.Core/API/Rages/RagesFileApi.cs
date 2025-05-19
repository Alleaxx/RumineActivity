using RumineActivity.Core.Logging;
using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RumineActivity.Core.API
{
    public class RagesFileApi : IRagesApi
    {
        private readonly IJsonFileInfo FileInfo;
        private readonly HttpClient Client;
        private readonly ILogger Logger;

        private string FilePath => FileInfo.Path;

        private IEnumerable<Rage> Rages { get; set; }

        public event Action OnLoaded;
        public bool? IsLoaded { get; private set; }

        public RagesFileApi(HttpClient client, ILogger logger, IJsonFileInfo fileInfo = null)
        {
            FileInfo = fileInfo;
            this.Client = client;
            this.Logger = logger;
        }

        public async Task LoadDataAsync()
        {
            if (IsLoaded.HasValue)
            {
                return;
            }

            IsLoaded = false;
            Rages = await LoadRagesFromFileAsync();
            IsLoaded = true;
            OnLoaded?.Invoke();
        }

        public IEnumerable<Rage> GetRages()
        {
            return Rages;
        }

        private async Task<IEnumerable<Rage>> LoadRagesFromFileAsync()
        {
            Logger.Log($"Читаем файл {FilePath}");

            string jsonContent = !FileInfo.IsWeb ? File.ReadAllText(FilePath) : await Client.GetStringAsync(FilePath);

            Logger.Log($"Файл {FilePath} прочитан и десериализован");

            return JsonSerializer.Deserialize<IEnumerable<Rage>>(jsonContent);
        }
    }
}
