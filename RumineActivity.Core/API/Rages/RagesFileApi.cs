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

        private IEnumerable<Rage> Rages => rages ?? ragesDebug;
        private IEnumerable<Rage> rages;
        private IEnumerable<Rage> ragesDebug;

        public event Action OnLoaded;
        public bool? IsLoaded { get; private set; }

        public RagesFileApi(HttpClient client, ILogger logger, IJsonFileInfo fileInfo = null)
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
            rages = await LoadRagesFromFile();
            IsLoaded = true;
            OnLoaded?.Invoke();
        }
        public async Task<IEnumerable<Rage>> GetRages()
        {
            return Rages;
        }

        private async Task<IEnumerable<Rage>> LoadRagesFromFile()
        {
            Logger.Log($"Читаем файл {FilePath}");

            string jsonContent = !FileInfo.IsWeb ? File.ReadAllText(FilePath) : await Client.GetStringAsync(FilePath);

            Logger.Log($"Файл {FilePath} прочитан и десериализован");

            return JsonSerializer.Deserialize<IEnumerable<Rage>>(jsonContent);
        }
    }
}
