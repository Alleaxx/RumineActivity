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
        public DateRange DateRange { get; set; } 
        public Period Period { get; set; }
        public TopicsMode TopicMode { get; set; } 
        public bool EmptyPeriodsEnabled { get; set; }

        public ReportOptions()
        {
            DateRange = new DateRange(FoundationDate, DateTime.Now);
            Period = new Period(Periods.Month);
            TopicMode = new TopicsMode(TopicsModes.All);
            EmptyPeriodsEnabled = false;
        }
        public ReportOptions(TopicsMode mode) : this()
        {
            TopicMode = mode;
        }
    }
}
