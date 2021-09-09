using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum Reports
    {
        Fact, Periodical, PeriodicalLegacy
    }
    public class ReportType : EnumType<Reports>
    {
        public ReportType(Reports reports) : base(reports)
        {
            switch (reports)
            {
                case Reports.Fact:
                    Name = "Фактический";
                    break;
                case Reports.Periodical:
                    Name = "Периодический";
                    break;
                case Reports.PeriodicalLegacy:
                    Name = "Периодический (устаревший)";
                    break;
            }
        }
        public static ReportType[] AllValues => Enum.GetValues<Reports>().Select(r => new ReportType(r)).ToArray();
    }


    public static class ReportsFactory
    {
        public static StatisticsReport CreateClassicReport()
        {
            return CreateReport(Reports.PeriodicalLegacy, new ReportOptions()
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
                case Reports.PeriodicalLegacy:
                    //return new ReportPeriods(new ReportSourceApp(), options).Create();
                case Reports.Periodical:
                    return new ReportPeriods2(new ReportSourceApp(), options).Create();
                default:
                    throw new Exception("Отчет неизвестного типа");
            }
        }
    }
}
