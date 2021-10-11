using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView.Pages
{
    public partial class Statistics : StatComponent
    {
        private ReportCreatorOptions Options { get; set; } = new ReportCreatorOptions();
        private StatisticsReport Report
        {
            get => App.Report;
            set
            {
                App.Report = value;
            }
        }


        private ReportType ReportType { get; set; } = new ReportType(Reports.Periodical);

        private void CreateReport()
        {
            Report = ReportsFactory.CreateReport(ReportType, Options);
        }

        public YearMonthDateSelector DateInterval { get; set; } = new YearMonthDateSelector(0, 0);

        //Представление числовых данных
        private static (int from, int to)[] Days(int year, int month)
        {
            var days = new (int from, int to)[]
            {
                (0,0),(1,7),(7,14),(14,21),(21,28)
            };
            return days;
        }

        private void Set(ReportType report)
        {
            ReportType = report;
            CreateReport();
        }
        private void Set(PostSource mode)
        {
            Options.PostSource = mode;
            CreateReport();
        }
        private void Set(Period period)
        {
            Options.Period = period;
            CreateReport();
        }



        private void SetYear(int year)
        {
            DateInterval.Year = year;
            UpdateDateRange();
        }
        private void SetMonth(int month)
        {
            DateInterval.Month = month;
            UpdateDateRange();
        }
        private void SetDay((int, int) day)
        {
            DateInterval.Day = day;
            UpdateDateRange();
        }
        private void UpdateDateRange()
        {
            Options.DateRange = DateInterval.Dates;
            CreateReport();
        }

        private ViewType ViewType { get; set; } = new ViewType(DViewTypes.Values);
        private void ChangedViewType(ViewType type)
        {
            ViewType = type;
        }

        public void Update(ChangeEventArgs e)
        {
            CreateReport();
        }
    }
}
