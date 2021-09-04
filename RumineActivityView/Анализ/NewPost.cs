using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RumineActivityView
{
    public class NewPost
    {
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
        private void AnalyzeLink(string link)
        {
            if (GetNumberFrom("showtopic-", link) is int topic)
            {
                EditingPost.TopicID = topic;
            }
            if (GetNumberFrom("findpost-", link) is int post)
            {
                EditingPost.ID = post;
            }
            if (GetNumberFrom("message-", link) is int message)
            {
                EditingPost.ID = message;
            }
            CheckEditingPost();
        }
        private int? GetNumberFrom(string pattern, string text)
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
        private void AnalyzeInfo(string info)
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
            CheckEditingPost();
        }
        public bool IsOk => !CheckResult.Error;

        //Результаты
        public Post EditingPost { get; private set; }
        public OpResult CheckResult { get; private set; }
        public void CheckEditingPost()
        {
            CheckResult = new OpResult(false, false, "");
            CheckPostProperties(CheckResult);

            bool error = CheckResult.Inner.Exists(r => r.Error);
            bool warning = CheckResult.Inner.Exists(r => r.Warning);
            bool safe = !error && !warning;

            CheckResult.Warning = warning;
            CheckResult.Error = error;
            CheckResult.Message = error ? "Есть ошибки" : warning ? "Есть предупреждения" : "Всё отлично";
        }
        private void CheckPostProperties(OpResult result)
        {
            if (EditingPost.ID < 0)
            {
                result.Add(true, true, "Нет ID");
            }
            if (EditingPost.Date == new DateTime())
            {
                result.Add(true, true, "Нет даты");
            }
            if (EditingPost.TopicID < 0)
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



        public NewPost()
        {
            EditingPost = new Post();
            CheckResult = new OpResult(true, true, "Поста вообще нет!");
        }
        public void SendPost()
        {
            if (IsOk)
            {
                StatApp.App.Add(EditingPost);
                AddingResult = new OpResult(false, false, $"Сообщение {EditingPost.ID} отправлено (всего {StatApp.App.Posts.Count})");
                EditingPost = new Post();
                Link = "";
                TopicInfo = "";
            }
        }

        public OpResult AddingResult { get; set; }
    }


    public class OpResult
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public bool Warning { get; set; }
        public string CssClass => Error ? "error" : Warning ? "warning" : "normal";
        public List<OpResult> Inner { get; private set; } = new List<OpResult>();

        public OpResult(bool error, bool warning,string msg)
        {
            Error = error;
            Warning = warning;
            Message = msg;
        }
        public void Add(bool error, bool warning, string msg)
        {
            Inner.Add(new OpResult(error, warning, msg));
        }
    }
}
