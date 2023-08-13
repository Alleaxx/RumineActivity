using RumineActivity.ConsoleApp;
using RumineActivity.Core.Logging;
using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RumineActivity
{
    /// <summary>
    /// Перевод информации из текстового файла формата в список сообщений.
    /// Выделяет первый и последний пост в каждом дне
    /// Сохраняет
    /// </summary>
    /// 1508514 | 6 декабря 2023 - 14:33
    /// 1508515 | 6 декабря 2023 - 14:34

    public class JsLogFileAnalyze
    {
        private readonly ILogger Logger;
        private readonly string FilePath;
        private readonly Dictionary<int, Post> PostsDictionary;


        public JsLogFileAnalyze(ILogger logger, string path)
        {
            this.Logger = logger;
            FilePath = path;
            PostsDictionary = new Dictionary<int, Post>();
        }
        public async Task Start()
        {
            string text = await File.ReadAllTextAsync(FilePath);
            string[] lines = text.Split('\n');
            foreach (string line in lines)
            {
                var post = ProcessLineToPost(line);
                if (post != null && !PostsDictionary.ContainsKey(post.Id))
                {
                    PostsDictionary.Add(post.Id, post);
                }

            }

            var groupedPosts = ForumAnalyze.GetPostsGroupedByDay(PostsDictionary.Values, PostsDictionary);
            var savedPosts = groupedPosts.SelectMany(g => new Post[] { g.minPost, g.maxPost }).OrderBy(p => p.Id);
            ForumAnalyze.SavePostsToFile(savedPosts, "MorePosts");
        }

        // Преобразует строк вида:
        // 1508514 | 6 декабря 2023 - 14:33
        // В объект сообщения
        private static Post ProcessLineToPost(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }

            var splitBy = line.Split('|');
            var idLine = splitBy[0].Trim();
            var dateLine = splitBy[1].Trim();

            int id = Convert.ToInt32(idLine);
            DateTime date = GetDate(dateLine);
            return new Post()
            {
                Id = id,
                Date = date
            };
        }
        private static DateTime GetDate(string line)
        {
            var splitted = line.Split(" ");
            if (line.Contains("Сегодня"))
            {
                var date = DateTime.Now;
                var day = date.Day;
                var month = date.Month;
                var year = date.Year;
                var time = splitted[1].Split(":");
                var hour = Convert.ToInt32(time[0]);
                var minute = Convert.ToInt32(time[1]);

                return new DateTime(year, month, day, hour, minute, 1);
            }
            else if (line.Contains("Вчера"))
            {
                var date = DateTime.Now.AddDays(-1);
                var day = date.Day;
                var month = date.Month;
                var year = date.Year;
                var time = splitted[1].Split(":");
                var hour = Convert.ToInt32(time[0]);
                var minute = Convert.ToInt32(time[1]);

                return new DateTime(year, month, day, hour, minute, 1);
            }
            else
            {
                var day = Convert.ToInt32(splitted[0].Trim());
                var month = GetMonthFromText(splitted[1].Trim());
                var year = Convert.ToInt32(splitted[2].Trim());
                var time = splitted[4].Split(":");
                var hour = Convert.ToInt32(time[0]);
                var minute = Convert.ToInt32(time[1]);

                return new DateTime(year, month, day, hour, minute, 1);
            }
        }
        private static int GetMonthFromText(string text)
        {
            if (text.Contains("января"))
            {
                return 1;
            }
            if (text.Contains("февраля"))
            {
                return 2;
            }
            if (text.Contains("марта"))
            {
                return 3;
            }
            else if (text.Contains("апреля"))
            {
                return 4;
            }
            else if (text.Contains("мая"))
            {
                return 5;
            }
            else if (text.Contains("июня"))
            {
                return 6;
            }
            else if (text.Contains("июля"))
            {
                return 7;
            }
            else if (text.Contains("августа"))
            {
                return 8;
            }
            else if (text.Contains("сентября"))
            {
                return 9;
            }
            else if (text.Contains("октября"))
            {
                return 10;
            }
            else if (text.Contains("ноября"))
            {
                return 11;
            }
            else if (text.Contains("декабря"))
            {
                return 12;
            }
            return 0;
        }

    }
}
