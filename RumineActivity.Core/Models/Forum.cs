using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core.Models
{
    public interface IForum
    {
        public IEnumerable<Post> Posts { get; }
        public IEnumerable<Topic> Topics { get; }
    }
    public class Forum : IForum
    {
        public IEnumerable<Post> Posts { get; init; }
        [Legacy]
        public IEnumerable<Topic> Topics { get; init; }
        [Legacy]
        public IEnumerable<User> Users { get; init; }
        public Forum(IEnumerable<Post> posts, IEnumerable<Topic> topics, IEnumerable<User> users)
        {
            Posts = posts;
            Topics = topics;
            Users = users ?? new List<User>();
        }
    }
}
