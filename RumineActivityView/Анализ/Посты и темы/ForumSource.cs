using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public interface IForumSource
    {
        public IEnumerable<Post> Posts { get; }
        public IEnumerable<Topic> Topics { get; }
    }
    public class ForumSourceApp : IForumSource
    {
        public IEnumerable<Post> Posts => StatApp.App.Posts;
        public IEnumerable<Topic> Topics => StatApp.App.Topics;
    }
    public class ForumSourceOwn : IForumSource
    {
        public ForumSourceOwn(IEnumerable<Post> posts, IEnumerable<Topic> topics)
        {
            Posts = posts;
            Topics = topics;
        }

        public IEnumerable<Post> Posts { get; private set; }
        public IEnumerable<Topic> Topics { get; private set; }
    }
}
