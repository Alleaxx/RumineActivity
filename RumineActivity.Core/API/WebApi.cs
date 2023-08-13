using RumineActivity.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RumineActivity.Core.API
{
    public abstract class WebApi
    {
        protected readonly string Path;
        private readonly JsonSerializerOptions JsonOptions;
        private readonly HttpClient Client;
        private readonly ILogger Logger;

        public WebApi(HttpClient client, ILogger logger, string path = "https://localhost:44341")
        {
            Path = path;
            Client = client;
            Logger = logger;
            JsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }

        protected async Task<IEnumerable<T>> Get<T>(string path)
        {
            try
            {
                var responce = await Client.GetAsync(path);
                string json = await responce.Content.ReadAsStringAsync();

                IEnumerable<T> collection = JsonSerializer.Deserialize<IEnumerable<T>>(json, JsonOptions);
                return collection;
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            return default;
        }
    }
}
