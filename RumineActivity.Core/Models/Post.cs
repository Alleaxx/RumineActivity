using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RumineActivity.Core.Models
{
    /// <summary>
    /// Модель форумного сообщения
    /// </summary>
    public class Post
    {
        public override string ToString()
        {
            return $"Пост {Id}";
        }


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } = -1;
        public DateTime Date { get; set; }

        [Legacy]
        [JsonIgnore]
        public Topic Topic { get; set; }
        [Legacy]
        [JsonIgnore]
        public User Author { get; set; }

        [Legacy]
        [JsonIgnore]
        public int TopicId { get; set; } = -1;
        [Legacy]
        [JsonIgnore]
        public string TopicTitle { get; set; }
        [Legacy]
        [JsonIgnore]
        public int TopicIndex { get; set; } = -1;
        [Legacy]
        [JsonIgnore]
        public int AuthorID { get; set; } = -1;
        [Legacy]
        [JsonIgnore]
        public string AuthorNick { get; set; }
        [Legacy]
        [JsonIgnore]
        public int Page => (int)Math.Floor(TopicIndex / 20.0);

        [JsonIgnore]
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
}
