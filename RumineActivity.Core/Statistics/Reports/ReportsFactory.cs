using RumineActivity.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public interface IReportsFactory
    {
        StatisticsReport CreateReport(ConfigurationReport options);
    }
    public class ReportsFactory : IReportsFactory
    {
        private readonly IActivityApi API;

        public ReportsFactory(IActivityApi api)
        {
            API = api;
        }

        public StatisticsReport CreateReport(ConfigurationReport options)
        {
            var forum = API.GetForum();
            return new ReportConstructor(forum, options).Create();
        }
    }
}
