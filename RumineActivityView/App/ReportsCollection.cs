using RumineActivity.Core;
using RumineActivity.Core.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.View
{
    public class ReportsCollection
    {
        private readonly IActivityApi API;
        private readonly IReportsFactory ReportsFactory;
        private readonly ValuesViewConfig Config;

        public event Action<StatisticsReport> OnReportsCollectionChanged;
        public event Action<StatisticsReport> OnReportsCompareCollectionChanged;

        /// <summary>
        /// Все созданные отчеты
        /// </summary>
        public IReadOnlyCollection<StatisticsReport> ActiveReports => Reports;

        /// <summary>
        /// Все отчеты, выделенные для сравнения
        /// </summary>
        public IReadOnlyCollection<StatisticsReport> CompareReports => ReportsForCompare;


        private List<StatisticsReport> Reports { get; init; }
        private List<StatisticsReport> ReportsForCompare { get; init; }

        public ReportsCollection(ValuesViewConfig config, IReportsFactory reportsFactory, IActivityApi aPI)
        {
            this.Config = config;
            ReportsFactory = reportsFactory;
            Reports = new List<StatisticsReport>();
            ReportsForCompare = new List<StatisticsReport>();
            API = aPI;
        }

        /// <summary>
        /// Добавляет отчет по конфигурации.
        /// Если отчет уже есть, то он просто выбирается
        /// </summary>
        public async Task<StatisticsReport> AddReportAsync(ConfigurationReport config)
        {
            string name = config.GetReportName();
            var reportFound = Reports.FirstOrDefault(r => r.Name == name);
            if (reportFound != null)
            {
                return reportFound;
            }

            if (!API.IsLoaded.HasValue)
            {
                await API.LoadDataAsync();
            }

            var newReport = ReportsFactory.CreateReport(config);
            Reports.Add(newReport);
            AddCompareReport(newReport);
            OnReportsCollectionChanged?.Invoke(newReport);
            return newReport;
        }

        /// <summary>
        /// Удаление отчета из списка
        /// </summary>
        public void RemoveReport(StatisticsReport report)
        {
            if(Reports.Count <= 1)
            {
                return;
            }

            Reports.Remove(report);
            RemoveCompareReport(report);
            OnReportsCollectionChanged?.Invoke(report);
        }


        public void AddCompareReport(StatisticsReport report)
        {
            if(!ReportsForCompare.Contains(report))
            {
                ReportsForCompare.Add(report);
                OnReportsCompareCollectionChanged?.Invoke(report);
            }
        }
        public void RemoveCompareReport(StatisticsReport report)
        {
            ReportsForCompare.Remove(report);
            OnReportsCompareCollectionChanged?.Invoke(report);
        }
    }
}
