using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет с РАВНЫМИ периодами, достраивает недостающие данные
    public class ReportPeriods : ReportCreator
    {
        public ReportPeriods(IReportSource source, ReportOptions options) : base(source, options)
        {

        }
        public override StatisticsReport Create()
        {
            if (IsEmptyReport)
            {
                return new StatisticsReport();
            }

            var dateInterval = Options.DateInterval;
            var dateRange = Options.DateRange;
            var splittedEntries = SplitPosts(new TimeSpan(0));

            List<Entry> newEntries = new List<Entry>();

            DateTime fromDate = Posts.First().Date;
            DateTime toDate = new DateTime();
            do
            {
                Entry entry = dateInterval.GetNextEntry(fromDate);
                DateRange range = entry.Range;
                toDate = range.To;

                var innerEntries = splittedEntries.Select(e => new EntryDateRangeFraction(range, e)).Where(f => f.Fraction > 0);

                entry.Value = innerEntries.Sum(PostsWritten);
                double PostsWritten(EntryDateRangeFraction obj)
                {
                    Entry e = obj.Entry;
                    double fraction = obj.Fraction;

                    if (fraction == 1)
                        return e.Value;
                    else
                        return e.Value * fraction;
                }
                newEntries.Add(entry);

                fromDate = toDate;
            }
            while (dateRange.IsDateInside(toDate));

            return new StatisticsReport($"Отчет по периодам", newEntries, Options);
        }
    }

}
