using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public interface IReportsFactory
    {
        Task<StatisticsReport> CreateReport(Reports type, ReportCreatorOptions options);
        Task<StatisticsReport> CreateClassicReport();
    }
    public class ReportsFactory : IReportsFactory
    {
        private readonly IActivityApi API;
        public ReportsFactory(IActivityApi api)
        {
            API = api;
        }


        public async Task<StatisticsReport> CreateClassicReport()
        {
            return await CreateReport(Reports.Periodical, new ReportCreatorOptions()
            {
                Period = Period.Create(Periods.Month)
            });
        }
        public async Task<StatisticsReport> CreateReport(Reports type, ReportCreatorOptions options)
        {
            switch (type)
            {
                case Reports.Fact:
                    return new CreateReportDefault(await API.GetForum(), options).Create();
                case Reports.Periodical:
                    return new CreateReportPeriods(await API.GetForum(), options).Create();
                default:
                    throw new Exception("Отчет неизвестного типа");
            }
        }
    }
}
