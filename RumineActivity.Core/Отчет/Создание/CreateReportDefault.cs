using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    //Отчет по существующим записям
    public class CreateReportDefault : CreateReportCommand
    {
        public CreateReportDefault(IForum source, ReportCreatorOptions options) : base(source, options) { }
        protected override StatisticsReport Construct()
        {
            return new StatisticsReport($"Отчет по записям", SplitPosts(Options.Period.TimeInterval), Options);
        }
    }
}
