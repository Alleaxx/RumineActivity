using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public interface IForumSource
    {
        public IEnumerable<Post> Posts { get; }
        public IEnumerable<Topic> Topics { get; }
    }
    public class ForumSource : IForumSource
    {
        public IEnumerable<Post> Posts { get; init; }
        public IEnumerable<Topic> Topics { get; init; }
        public ForumSource(IEnumerable<Post> posts, IEnumerable<Topic> topics)
        {
            Posts = posts;
            Topics = topics;
        }
    }


}
