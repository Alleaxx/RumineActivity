using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RumineActivity.Core
{
    public interface IPostEditor
    {
        string Link { get; set; }
        string TopicInfo { get; set; }
        Task SendPost();

        CheckMessage CheckResult { get; }
        CheckMessage AddingResult { get; }
        bool IsOk { get; }
    }
    public class PostEditor : IPostEditor
    {
        private readonly IActivityApi Api;
        private async Task<IEnumerable<Post>> Posts()
        {
            return (await Api.GetForum()).Posts;
        }
        
        
        //Информация о посте
        public string Link
        {
            get => link;
            set
            {
                link = value;
                AnalyzeLink(value);
            }
        }
        private string link;
        public string TopicInfo
        {
            get => topicInfo;
            set
            {
                topicInfo = value;
                AnalyzeInfo(value);
            }
        }
        private string topicInfo;

        //Анализ информации
        private async Task AnalyzeLink(string link)
        {
            if (GetNumberFrom("showtopic-", link) is int topic)
            {
                EditingPost.TopicId = topic;
            }
            if (GetNumberFrom("findpost-", link) is int post)
            {
                EditingPost.Id = post;
            }
            if (GetNumberFrom("message-", link) is int message)
            {
                EditingPost.Id = message;
            }
            await CheckEditingPost();
        }
        private static int? GetNumberFrom(string pattern, string text)
        {
            Regex regPostId = new Regex($"{pattern}[0-9]*");
            Match match = regPostId.Match(text);
            if (match.Success)
            {
                string postId = match.Value.Replace(pattern, "");
                if (int.TryParse(postId, out int id))
                {
                    return id;
                }
            }
            return null;
        }
        private async Task AnalyzeInfo(string info)
        {
            string[] splittedInfo = info.Split("|");
            string indexText = splittedInfo.First().Trim().Replace("#","");
            string dateText = splittedInfo.Last().Trim();

            //Позиция в теме
            Regex topicIndexReg = new Regex("[0-9]*");
            Match topicIndexMatch = topicIndexReg.Match(indexText);
            if (topicIndexMatch.Success && int.TryParse(topicIndexMatch.Value, out int topicIndex))
            {
                EditingPost.TopicIndex = topicIndex;
            }

            //Дата поста
            string format = "dd.MM.yyyy";
            dateText = dateText.Replace("-", "");
            dateText = dateText.Replace("Сегодня", DateTime.Now.ToString(format));
            dateText = dateText.Replace("Вчера", DateTime.Now.AddDays(-1).ToString(format));
            if (DateTime.TryParse(dateText, out DateTime date))
            {
                EditingPost.Date = date;
            }
            await CheckEditingPost();
        }
        public bool IsOk => !CheckResult.Error;

        //Результаты
        private Post EditingPost { get; set; }
        public CheckMessage CheckResult { get; private set; }
        private async Task CheckEditingPost()
        {
            CheckResult = new CheckMessage(false, false, "");
            await CheckPostProperties(CheckResult);

            bool error = CheckResult.Inner.Exists(r => r.Error);
            bool warning = CheckResult.Inner.Exists(r => r.Warning);
            bool safe = !error && !warning;

            CheckResult.Warning = warning;
            CheckResult.Error = error;
            CheckResult.Message = error ? "Есть ошибки" : warning ? "Есть предупреждения" : "Всё отлично";
        }
        private async Task CheckPostProperties(CheckMessage result)
        {
            var posts = await Posts();
            if (EditingPost.Id < 0)
            {
                result.Add(true, true, "Нет ID");
            }
            if(posts.Any(p => p.Id == EditingPost.Id))
            {
                result.Add(true, false, "Пост уже есть в базе");
            }
            if (EditingPost.Date == new DateTime())
            {
                result.Add(true, true, "Нет даты");
            }
            if (EditingPost.TopicId < 0)
            {
                result.Add(true, true, "Нет темы");
            }
            if (EditingPost.TopicIndex < 0)
            {
                result.Add(true, true, "Нет позиции в теме");
            }
            if (EditingPost.Page < 0)
            {
                result.Add(true, true, "Нет страницы");
            }
        }



        public PostEditor(IActivityApi api)
        {
            Api = api;
            EditingPost = new Post();
            CheckResult = new CheckMessage(true, true, "Поста вообще нет!");
        }
        public async Task SendPost()
        {
            if (IsOk)
            {
                var result = await Api.Create(EditingPost);
                if (result != null)
                {
                    AddingResult = new CheckMessage(false, false, $"Сообщение {result.Id} ({result.TopicIndex}) отправлено!");
                    EditingPost = new Post();
                    Link = "";
                    TopicInfo = "";
                }
                else
                {
                    AddingResult = new CheckMessage(true, false, $"Не удалось сохранить пост");
                }
            }
        }

        public CheckMessage AddingResult { get; set; }
    }
}
