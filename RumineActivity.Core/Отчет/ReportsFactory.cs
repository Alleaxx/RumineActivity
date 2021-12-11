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
            var forum = await API.GetForum(options.DateRange);
            return type switch
            {
                Reports.Fact => new CreateReportDefault(forum, options).Create(),
                Reports.Periodical => new CreateReportPeriods(forum, options).Create(),
                _ => throw new Exception("Отчет неизвестного типа"),
            };
        }
    }
}
