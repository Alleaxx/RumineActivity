using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RumineActivityAPI.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } = -1;
        public DateTime Date { get; set; }

        public int TopicId { get; set; } = -1;

        [JsonIgnore]
        public Topic Topic { get; set; }
        public int TopicIndex { get; set; } = -1;
    }
}
