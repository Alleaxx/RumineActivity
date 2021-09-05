using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public interface IReportSource
    {
        public IEnumerable<Post> Posts { get; }
        public IEnumerable<Topic> Topics { get; }
    }
    public class ReportSourceApp : IReportSource
    {
        public ReportSourceApp()
        {

        }

        public IEnumerable<Post> Posts => StatApp.App.Posts;
        public IEnumerable<Topic> Topics => StatApp.App.Topics;
    }
}
