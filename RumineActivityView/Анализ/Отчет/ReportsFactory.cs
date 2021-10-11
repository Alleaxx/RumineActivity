using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public static class ReportsFactory
    {
        private static StatApp App { get; set; }
        public static void SetDefaultData(StatApp app)
        {
            App = app;
        }


        public static StatisticsReport CreateClassicReport(StatApp app)
        {
            return CreateReport(Reports.Periodical, new ReportCreatorOptions()
            {
                Period = Period.Create(Periods.Month)
            });
        }


        public static StatisticsReport CreateReport(Reports type, ReportCreatorOptions options)
        {
            return CreateReport(type, options, new ForumSource(App));
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
