using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет по существующим записям
    public class ReportDefault : ReportCreator
    {
        public ReportDefault(IForumSource source, ReportCreatorOptions options) : base(source, options)
        {

        }
        protected override StatisticsReport Construct()
        {
            var entries = SplitPosts(Options.Period.TimeInterval);
            return new StatisticsReport($"Отчет по записям", entries, Options);
        }
    }
}
