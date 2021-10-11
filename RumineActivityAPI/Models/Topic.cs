using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityAPI.Models
{
    public class Topic
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChat { get; set; }

        public List<Post> Messages { get; set; }
    }
}
