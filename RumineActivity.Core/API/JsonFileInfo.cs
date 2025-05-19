using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RumineActivity.Core.API
{
    public interface IJsonFileInfo
    {
        string Path { get; }
        bool IsWeb { get; }
        IEnumerable<Post> GetPosts(string content);
    }
    public class JsonFileInfo : IJsonFileInfo
    {
        public string Path { get; private set; }
        public bool IsWeb { get; init; }
        private Func<string, IEnumerable<Post>> GetPostsFunc { get; set; }


        private JsonFileInfo(string path, bool isWeb, Func<string, IEnumerable<Post>> func)
        {
            Path = path;
            IsWeb = isWeb;
            GetPostsFunc= func;
        }
        public static JsonFileInfo FromObjectJson(string path, bool isWeb)
        {
            return new JsonFileInfo(path, isWeb, GetPostsFromObject);
        }

        public IEnumerable<Post> GetPosts(string content)
        {
            return GetPostsFunc.Invoke(content);
        }

        private static IEnumerable<Post> GetPostsFromObject(string content)
        {
            var allPosts = JsonSerializer.Deserialize<IEnumerable<Post>>(content);
            return allPosts;
        }
    }
}
