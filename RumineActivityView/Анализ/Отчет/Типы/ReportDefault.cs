using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет по существующим записям
    public class ReportDefault : ReportCreator
    {
        public ReportDefault(IReportSource source, ReportOptions options) : base(source, options)
        {

        }
        public override StatisticsReport Create()
        {
            if (IsEmptyReport)
            {
                return new StatisticsReport();
            }
            else
            {
                var entries = SplitPosts(Options.DateInterval.TimeInterval);
                return new StatisticsReport($"Отчет по записям:\n {Options.DateRange.ToString("dd.MM.yyyy", "-")}", entries);
            }
        }

    }
}
