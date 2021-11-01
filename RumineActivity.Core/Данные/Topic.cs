using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class Topic
    {
        public override string ToString() => $"Тема '{Name}' [{Id}]";


        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsChat { get; set; }


        public List<Post> Messages { get; set; }
    }
}
