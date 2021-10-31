using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class Post
    {
        public override string ToString()
        {
            return $"Пост {Id}";
        }

        //*
        public int Id { get; set; } = -1;
        public DateTime Date { get; set; }

        //-
        public int TopicId { get; set; } = -1;
        public int TopicIndex { get; set; } = -1;

        public int Page => (int)Math.Floor((double)TopicIndex / 20);


        public string AutoLink => $"https://ru-minecraft.ru/forum/showtopic-{TopicId}/findpost-{Id}/";

        public Post()
        {

        }
        public Post(int id, DateTime date)
        {
            Id = id;
            Date = date;
        }
    }
    public class PostJson
    {
        public override string ToString()
        {
            return $"Пост {id}";
        }

        public int id { get; set; } = -1;
        public DateTime date { get; set; }

        public int topicID { get; set; } = -1;
        public int topicIndex { get; set; } = -1;
    }
}
