using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum Reports
    {
        Fact, Periodical
    }
    public static class ReportsFactory
    {
        public static StatisticsReport CreateClassicReport()
        {
            return CreateReport(Reports.Periodical, new ReportOptions()
            {
                DateInterval = new DateInterval(Dates.Month)
            });
        }
        public static StatisticsReport CreateReport(Reports type, ReportOptions options)
        {
            switch (type)
            {
                case Reports.Fact:
                    return new ReportDefault(new ReportSourceApp(), options).Create();
                case Reports.Periodical:
                    return new ReportPeriods(new ReportSourceApp(), options).Create();
                default:
                    throw new Exception("Отчет неизвестного типа");
            }
        }
    }
}
