using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using RumineActivity.Core;
namespace RumineActivity.View
{
    public interface IStatApp
    {
        public StatisticsReport Report { get; set; }
        public ViewOptions ViewOptions { get; }
    }
    public class StatApp : IStatApp
    {
        private readonly IReportsFactory ReportsFactory;

        public StatisticsReport Report { get; set; }
        public ViewOptions ViewOptions { get; private set; }

        public StatApp(IReportsFactory reportsFactory)
        {
            ReportsFactory = reportsFactory;
            ViewOptions = new ViewOptions();
            LoadDataFromApi();
        }
        private async void LoadDataFromApi()
        {
            Report = await ReportsFactory.CreateReport(Reports.Periodical, new ReportCreatorOptions());
        }
    }

}
