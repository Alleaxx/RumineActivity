using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class Topic
    {
        public override string ToString() => $"Тема '{Name}' [{ID}]";


        public int ID { get; set; }
        public string Name { get; set; }
        public bool? IsChat { get; set; }
    }
}
