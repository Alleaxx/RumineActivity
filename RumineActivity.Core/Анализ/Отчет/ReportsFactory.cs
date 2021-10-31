using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public static class ReportsFactory
    {
        private static IForumSource Source { get; set; }
        //private static StatApp App { get; set; }
        public static void SetDefaultData(IForumSource source)
        {
            Source = source;
        }


        public static StatisticsReport CreateClassicReport()
        {
            return CreateReport(Reports.Periodical, new ReportCreatorOptions()
            {
                Period = Period.Create(Periods.Month)
            });
        }


        public static StatisticsReport CreateReport(Reports type, ReportCreatorOptions options)
        {
            return CreateReport(type, options, Source /*new ForumSource(App)*/);
        }
        public static StatisticsReport CreateReport(Reports type, ReportCreatorOptions options, IForumSource source)
        {
            switch (type)
            {
                case Reports.Fact:
                    return new CreateReportDefault(source, options).Create();
                case Reports.Periodical:
                    return new CreateReportPeriods(source, options).Create();
                default:
                    throw new Exception("Отчет неизвестного типа");
            }
        }
    }
}
