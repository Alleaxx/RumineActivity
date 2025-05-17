using RumineActivity.Core;
using RumineActivity.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using RumineActivity.Core.Models;

namespace RumineActivity.ConsoleApp
{
    class ForumAnalyze
    {
        private readonly List<Post> Posts;

        public Dictionary<int,Post> PostsDictionary { get; set; }

        public ForumAnalyze(IForum forum)
        {
            Posts = new List<Post>(forum.Posts.OrderBy(p => p.Id));
            PostsDictionary = new Dictionary<int, Post>();

            foreach (var post in forum.Posts)
            {
                if (!PostsDictionary.ContainsKey(post.Id))
                {
                    PostsDictionary.Add(post.Id, post);
                }
            }
        }

        public void Analyze()
        {
            ClearErrorPosts();

            var grouped = GetPostsGroupedByDay(Posts, PostsDictionary);
            var newPosts = grouped.Select(g => g.minPost).Union(grouped.Select(g => g.maxPost)).OrderBy(p => p.Id);
            foreach (var post in newPosts)
            {
                Console.WriteLine();
                Console.Write($"[{post.Id}]".PadRight(10));
                Console.Write($"[{post.Date}]".PadRight(20));
            }

            SavePostsToFile(newPosts);
        }


        /// <summary>
        /// Синхронизировать порядок следования ID и дат постов
        /// </summary>
        private void ClearErrorPosts()
        {
            int i = 0;
            int removedPosts = 0;
            while (true)
            {
                Console.WriteLine($"Проверка № {i}");
                var errorPosts = GetPostsWithDateOrderError();
                foreach (var post in errorPosts)
                {
                    Posts.Remove(post);
                }
                removedPosts += errorPosts.Count();
                Console.WriteLine("Ошибочные сообщения удалены");
                if (errorPosts.Count() == 0)
                {
                    break;
                }
                i++;
            }

            Console.WriteLine($"Всего {removedPosts} сообщений удалено после {i} проверок");

        }
        private IEnumerable<Post> GetPostsWithDateOrderError()
        {
            var sortedPosts = Posts;

            List<Post> errorPosts = new List<Post>();
            Console.WriteLine();
            Console.WriteLine("---".PadRight(5) + "АНАЛИЗ" + "---".PadLeft(5));
            for (int i = 1; i < sortedPosts.Count; i++)
            {
                var post = sortedPosts[i];
                var prevPost = sortedPosts[i - 1];
                bool isOk = post.Date >= prevPost.Date;

                if (!isOk)
                {
                    Console.Write($"ТЕКУЩИЙ: ".PadRight(10));
                    Console.Write($"[{post.Id}]".PadRight(10));
                    Console.Write(post.Date.ToString().PadRight(25));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("Ошибка!".PadRight(10));
                    Console.Write($"{prevPost.Date} > {post.Date}".PadRight(50));
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"ПРЕДЫДУЩИЙ: ".PadRight(10));
                    Console.Write($"[{prevPost.Id}]".PadRight(10));
                    Console.Write(prevPost.Date.ToString().PadRight(25));
                    Console.WriteLine();

                    errorPosts.Add(prevPost);
                }
            }
            Console.WriteLine("---".PadRight(5) + "КОНЕЦ АНАЛИЗА" + "---".PadLeft(5));
            Console.WriteLine();

            Console.WriteLine($"{errorPosts.Count} ошибок найдено в порядке сообщений");
            return errorPosts;
        }
        
        //Выделить по два поста на каждый день с 27 июля 2011 по текущую дату: конечный и начальный
        public static List<(DateTime date, Post minPost, Post maxPost)> GetPostsGroupedByDay(IEnumerable<Post> Posts, Dictionary<int, Post> PostsDictionary)
        {
            DateTime startDate = new DateTime(2011, 7, 27);
            List<DateTime> dates = new List<DateTime>();
            DateTime newDate = startDate;
            while(newDate < DateTime.Now)
            {
                dates.Add(newDate.AddDays(1));
                newDate = newDate.AddDays(1);
            }

            List<(DateTime date, Post minPost, Post maxPost)> results = new List<(DateTime date, Post minPost, Post maxPost)>();
            int fullDays = 0;
            int oneMsgDay = 0;
            int zeroMsgDay = 0;
            foreach (var day in dates)
            {
                var nextDay = day.AddDays(1);
                var posts = Posts.Where(p => p.Date >= day && p.Date < nextDay);

                if (!posts.Any())
                {
                    Console.WriteLine($"Для {day:dd-MMM-yyyy} не нашлось ни одного сообщения");
                    zeroMsgDay++;
                    continue;
                }

                var minPost = posts.Min(p => p.Id);
                var maxPost = posts.Max(p => p.Id);
                if (minPost == maxPost)
                {
                    Console.WriteLine($"Для {day:dd-MMM-yyyy} нашлось только одно сообщение ({minPost})");
                    oneMsgDay++;
                }
                else
                {
                    fullDays++;
                }
                results.Add((day, PostsDictionary[minPost], PostsDictionary[maxPost]));
            }

            Console.WriteLine($"Итого: {fullDays} полных дней, {oneMsgDay} дней с одним постом, {zeroMsgDay} с нулем постов");
            return results;
        }

        //Сохранить итоговые посты в файл
        public static void SavePostsToFile(IEnumerable<Post> savedPosts,string fileName = "ForumPostsV5")
        {
            Console.WriteLine();
            Console.WriteLine("Сериализация указанных постов");
            var json = JsonSerializer.Serialize(savedPosts, new JsonSerializerOptions()
            {
                WriteIndented = true,
            });
            string FilePath = $"C:\\Users\\Alleaxx\\source\\repos\\Данные\\Статистика активности\\Обработанные данные\\{fileName}.json";
            Console.WriteLine($"Сохранение в {fileName}.json");
            File.WriteAllText(FilePath, json);
            Console.WriteLine("Файл сохранен, полный путь:");
            Console.WriteLine(FilePath);
        }
    }
}
