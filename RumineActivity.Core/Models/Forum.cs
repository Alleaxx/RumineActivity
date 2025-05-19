using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core.Models
{
    public interface IForum
    {
        public IEnumerable<Post> Posts { get; }
    }
    public class Forum : IForum
    {
        public IEnumerable<Post> Posts { get; init; }

        public Forum(IEnumerable<Post> posts)
        {
            Posts = posts;
        }
    }
}
