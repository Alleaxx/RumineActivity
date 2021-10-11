using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class ActivityWebApi
    {
        public string Path { get; set; }
        private JsonSerializerOptions JsonOptions { get; set; }
        private readonly HttpClient Client;
        public ActivityWebApi(string path, HttpClient client)
        {
            Path = path;
            Client = client;
            JsonOptions = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
        }


        public string MsgPath()
        {
            return $"{Path}/Messages";
        }
        public string TopicPath()
        {
            return $"{Path}/Topics";
        }

        public async Task<IEnumerable<T>> Get<T>(string path)
        {
            try
            {
                var responce = await Client.GetAsync(path);
                string json = await responce.Content.ReadAsStringAsync();

                IEnumerable<T> collection = JsonSerializer.Deserialize<IEnumerable<T>>(json, JsonOptions);
                Console.WriteLine($"Коллекция получена из API");
                return collection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось получить коллекцию:");
                Console.WriteLine(ex.Message);
            }
            return default;
        }
        public async Task<T> Create<T>(T post, string path)
        {
            try
            {
                var responce = await Client.PostAsync(path, GetContent(post));
                if (responce.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Элемент {post} успешно добавлен или уже существует по API");
                    return post;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось создать элемент:");
                Console.WriteLine(ex.Message);
            }
            return default;
        }
        public async Task<T> Update<T>(T post, string path)
        {

            try
            {
                var responce = await Client.PutAsync(path, GetContent(post));
                if (responce.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Элемент {post} успешно обновлен по API");
                    return post;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось обновить элемент:");
                Console.WriteLine(ex.Message);
            }
            return default;
        }
        public async Task<T> Delete<T>(T post, string path, object pathId)
        {
            string deletePath = $"{path}/{pathId}";
            try
            {
                var responce = await Client.DeleteAsync(deletePath);
                if (responce.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Элемент {post} успешно удален по API");
                    return post;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Не удалось удалить элемент:");
                Console.WriteLine(ex.Message);
            }
            return default;
        }
        private StringContent GetContent<T>(T obj)
        {
            string content = JsonSerializer.Serialize(obj, JsonOptions);
            return new StringContent(content, Encoding.UTF8, "application/json");
        }

        public async Task<IEnumerable<Post>> GetAllPosts()
        {
            return await Get<Post>(MsgPath());
        }
        public async Task<IEnumerable<Topic>> GetAllTopics()
        {
            return await Get<Topic>(TopicPath());
        }
    }
}
