using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class ReportOptions
    {
        public static readonly DateTime FoundationDate = new DateTime(2011, 7, 27);
        //Конкретные даты
        public DateRange DateRange { get; set; } = new DateRange(FoundationDate, DateTime.Now);
        public DateInterval DateInterval { get; set; } = new DateInterval(Dates.Month);
        public TopicsMode TopicMode { get; set; } = new TopicsMode(TopicsModes.All);

        public ReportOptions()
        {

        }
        public ReportOptions(TopicsMode mode)
        {
            TopicMode = mode;
        }
    }
}
