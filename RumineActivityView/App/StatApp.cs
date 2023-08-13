using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using RumineActivity.Core;
using RumineActivity.Core.API;

namespace RumineActivity.View
{
    public interface IStatApp
    {
        event Action<StatisticsReport> OnLoadEnded;

        ReportsCollection ReportsCollection { get;}
        ReportConfigEditor ReportConfigEditor { get; }
        ValuesViewConfig ValuesViewConfig { get; }

        Task InitReports();
        bool? IsLoaded { get; }

        DateTime LastUpdateInfo { get; }
        string VersionInfo { get;  }
    }
    public class StatApp : IStatApp
    {
        private readonly IReportsFactory ReportsFactory;
        private readonly IActivityApi API;
        private readonly ILocalStorageService LocalStorageService;
        private readonly IJsonService JsonService;

        public event Action<StatisticsReport> OnLoadEnded;

        public DateTime LastUpdateInfo { get; init; } = new DateTime(2023, 12, 7);
        public string VersionInfo { get; init; } = "0.95";

        public ReportsCollection ReportsCollection { get; init; }
        public ReportConfigEditor ReportConfigEditor { get; set; }
        public ValuesViewConfig ValuesViewConfig { get; private set; }

        public bool? IsLoaded { get; private set; }

        public StatApp(IReportsFactory reportsFactory, IActivityApi api, ILocalStorageService localStorage, IJsonService jsonService)
        {
            API = api;
            ReportsFactory = reportsFactory;
            this.LocalStorageService = localStorage;
            this.JsonService = jsonService;
            ValuesViewConfig = new ValuesViewConfig(jsonService, localStorage);
            ReportsCollection = new ReportsCollection(ValuesViewConfig, reportsFactory, api);
            ReportConfigEditor = new ReportConfigEditor(this);
        }
        public async Task InitReports()
        {
            IsLoaded = false;
            //await API.LoadData();
            //var classicConfig = new ConfigurationReport();
            //await ReportsCollection.AddReport(classicConfig, true);
            await ValuesViewConfig.LoadSettings();
            IsLoaded = true;
            OnLoadEnded?.Invoke(ReportsCollection.SelectedReport);
        }
    }

}
