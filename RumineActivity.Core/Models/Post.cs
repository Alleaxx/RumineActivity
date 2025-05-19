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
