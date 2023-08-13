using RumineActivity.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public interface IReportsFactory
    {
        Task<StatisticsReport> CreateReport(ConfigurationReport options);
    }
    public class ReportsFactory : IReportsFactory
    {
        private readonly IActivityApi API;
        public ReportsFactory(IActivityApi api)
        {
            API = api;
        }

        public async Task<StatisticsReport> CreateReport(ConfigurationReport options)
        {
            var forum = await API.GetForum();
            return new ReportConstructor(forum, options).Create();
        }
        public async Task<StatisticsReport> CreateEmptyReport()
        {
            return new StatisticsReport(Array.Empty<Entry>(), Array.Empty<Entry>(), new ConfigurationReport(), "Нулевой отчет");
        }
    }
}
