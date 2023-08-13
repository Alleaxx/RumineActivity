using RumineActivity.Core.Models;
using RumineActivity.Core.Models.SQL;
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

        public IEnumerable<Post> GetPosts(string content)
        {
            return GetPostsFunc.Invoke(content);
        }


        private JsonFileInfo(string path, bool isWeb, Func<string, IEnumerable<Post>> func)
        {
            Path = path;
            IsWeb = isWeb;
            GetPostsFunc= func;
        }
        public static JsonFileInfo FromSqlJson(string path, bool isWeb)
        {
            return new JsonFileInfo(path, isWeb, GetPostsFromSql);
        }
        public static JsonFileInfo FromObjectJson(string path, bool isWeb)
        {
            return new JsonFileInfo(path, isWeb, GetPostsFromObject);
        }


        private static IEnumerable<Post> GetPostsFromSql(string content)
        {
            var sqlReport = JsonSerializer.Deserialize<SqlExportReport>(content);
            var allPosts = sqlReport.rows.Select(r => CreatePostFromRow(r)).ToList();
            return allPosts;
        }
        private static Post CreatePostFromRow(SqlExportRow row)
        {
            try
            {
                int postID = Convert.ToInt32(row.ElementAt(0).ToString());
                DateTime postDate = DateTime.Parse(row.ElementAt(1).ToString());

                int topicID = Convert.ToInt32(row.ElementAt(2).ToString());
                int authorID = Convert.ToInt32(row.ElementAt(4).ToString());

                string topicName = $"{row.ElementAt(3)}";
                string authorNick = $"{row.ElementAt(5)}";


                return new Post()
                {
                    Id = postID,
                    Date = postDate,
                    TopicId = topicID,
                    AuthorID = authorID,
                    TopicTitle = topicName,
                    AuthorNick = authorNick
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        private static IEnumerable<Post> GetPostsFromObject(string content)
        {
            var allPosts = JsonSerializer.Deserialize<IEnumerable<Post>>(content);
            return allPosts;
        }
    }
}
