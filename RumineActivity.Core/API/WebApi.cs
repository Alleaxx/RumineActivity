using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RumineActivity.Core
{

    public abstract class WebApi
    {
        protected readonly string Path;
        private readonly JsonSerializerOptions JsonOptions;
        private readonly HttpClient Client;
        public WebApi(HttpClient client, string path = "https://localhost:44341")
        {
            Path = path;
            Client = client;
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
                Console.WriteLine(ex.Message);
            }
            return default;
        }
        protected async Task<T> Create<T>(T post, string path)
        {
            try
            {
                var responce = await Client.PostAsync(path, GetContent(post));
                if (responce.IsSuccessStatusCode)
                {
                    return post;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }
        protected async Task<T> Update<T>(T post, string path)
        {
            try
            {
                var responce = await Client.PutAsync(path, GetContent(post));
                if (responce.IsSuccessStatusCode)
                {
                    return post;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }
        protected async Task<T> Delete<T>(T post, string path, object pathId)
        {
            string deletePath = $"{path}/{pathId}";
            try
            {
                var responce = await Client.DeleteAsync(deletePath);
                if (responce.IsSuccessStatusCode)
                {
                    return post;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return default;
        }

        private StringContent GetContent<T>(T obj)
        {
            string content = JsonSerializer.Serialize(obj, JsonOptions);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }
    }
}
