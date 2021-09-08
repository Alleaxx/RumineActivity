using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class Post
    {
        public override string ToString()
        {
            return $"Пост {ID}";
        }


        //*
        public int ID { get; set; } = -1;
        public DateTime Date { get; set; }

        //-
        public int TopicID { get; set; } = -1;
        public int TopicIndex { get; set; } = -1;

        //-
        public int Page => (int)Math.Floor((double)TopicIndex / 20);


        public string AutoLink => $"https://ru-minecraft.ru/forum/showtopic-{TopicID}/findpost-{ID}/";

        public Post()
        {

        }
        public Post(int id, DateTime date)
        {
            ID = id;
            Date = date;
        }
    }
}
