using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public static class ReportsFactory
    {
        public static StatisticsReport CreateClassicReport()
        {
            return CreateReport(Reports.Periodical, new ReportCreatorOptions()
            {
                Period = new Period(Periods.Month)
            });
        }


        public static StatisticsReport CreateReport(Reports type, ReportCreatorOptions options)
        {
            return CreateReport(type, options, new ForumSourceApp());
        }
        public static StatisticsReport CreateReport(Reports type, ReportCreatorOptions options, IForumSource source)
        {
            switch (type)
            {
                case Reports.Fact:
                    return new ReportDefault(source, options).Create();
                case Reports.Periodical:
                    return new ReportPeriods(source, options).Create();
                default:
                    throw new Exception("Отчет неизвестного типа");
            }
        }
    }
}
