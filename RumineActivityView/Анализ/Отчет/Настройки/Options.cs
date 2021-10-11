using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class ReportCreatorOptions : IDateRange
    {
        public static readonly DateTime FoundationDate = new DateTime(2011, 7, 27);

        public DateRange DateRange { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public Period Period { get; set; }
        public PostSource PostSource { get; set; }
        public SourceTopics PostsTopic => PostSource as SourceTopics; 
        
        public bool EmptyPeriodsEnabled { get; set; }


        public ReportCreatorOptions()
        {
            DateRange = new DateRange(FoundationDate, DateTime.Now);
            Period = Period.Create(Periods.Month);
            PostSource = PostSource.Create(PostSources.All);
            EmptyPeriodsEnabled = false;
        }
        public ReportCreatorOptions(PostSource mode) : this()
        {
            PostSource = mode;
        }

        public Post[] GetPostsSource(IForumSource source)
        {
            return source.Posts
                .Where(p => PostSource.Filter(p, source.Topics))
                .Where(p => DateRange.IsDateInside(p.Date))
                .OrderBy(d => d.Date)
                .ToArray();
        }
    }
}
