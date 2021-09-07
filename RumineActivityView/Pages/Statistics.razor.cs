using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView.Pages
{
    public partial class Statistics : StatComponent
    {
        private ReportOptions Options { get; set; } = new ReportOptions();
        private StatisticsReport Report { get; set; } = ReportsFactory.CreateClassicReport();
        private IEnumerable<Topic> TopicOptions =>
            Options.TopicMode.Topics.Select(id => StatApp.App.Topics.FirstOrDefault(t => t.ID == id)).Where(t => t != null);



        private Reports ReportType { get; set; } = Reports.Periodical;

        private void CreateReport()
        {
            Report = ReportsFactory.CreateReport(ReportType, Options);
        }

        public YearMonthDateSelector DateInterval { get; set; } = new YearMonthDateSelector(0, 0);

        //Представление числовых данных




        private TopicsMode[] TopicModes { get; set; } = Enum.GetValues<TopicsModes>().Select(u => new TopicsMode(u)).ToArray();
        private MeasureUnit[] Units { get; set; } = Enum.GetValues<MeasureUnits>().Select(u => new MeasureUnit(u)).ToArray();
        private MeasureMethod[] Methods { get; set; } = Enum.GetValues<MeasureMethods>().Select(u => new MeasureMethod(u)).ToArray();
        private DateInterval[] Intervals { get; set; } = Enum.GetValues<Dates>().Select(d => new DateInterval(d)).ToArray();
        private int[] Years { get; set; } = new int[] { 0, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021 };
        private int[] Months { get; set; } = Enumerable.Range(0, 13).ToArray();
        private (int from, int to)[] Days(int year, int month)
        {
            var days = new (int from, int to)[]
            {
                (0,0),(1,7),(7,14),(14,21),(21,28)
            };
            return days;
        }


        const string SelectedClass = "selected";
        const string SelectableClass = "selectable";
        private string GetCssClass(MeasureUnit unit) => unit.Unit == View.MeasureUnit.Unit ? SelectedClass : SelectableClass;
        private string GetCssClass(MeasureMethod method) => method.Method == View.MeasureMethod.Method ? SelectedClass : SelectableClass;
        private string GetCssClass(DateInterval method) => method.Type == Options.DateInterval.Type ? SelectedClass : SelectableClass;
        private string GetCssClass(TopicsMode topic) => topic.Mode == Options.TopicMode.Mode ? SelectedClass : SelectableClass;
        private string GetCssClassYear(int year) => year == DateInterval.Year ? SelectedClass : SelectableClass;
        private string GetCssClassMonth(int month) => month == DateInterval.Month ? SelectedClass : SelectableClass;
        private string GetCssClassDay((int from, int to) day) => day.from == DateInterval.Day.from && day.to == DateInterval.Day.to ? SelectedClass : SelectableClass;
        private void Set(MeasureUnit unit)
        {
            View.MeasureUnit = unit;
        }
        private void Set(MeasureMethod method)
        {
            View.MeasureMethod = method;
        }
        private void Set(TopicsMode mode)
        {
            Options.TopicMode = mode;
            CreateReport();
        }
        private void Set(DateInterval interval)
        {
            Options.DateInterval = interval;
            CreateReport();
        }
        private void SetTopicMode(ChangeEventArgs e)
        {
            if (e.Value is string str)
            {
                Options.TopicMode = TopicModes.Where(t => t.Mode.ToString() == str).First();
            }
        }
        private void AddTopic(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value.ToString(), out int id))
            {
                Options.TopicMode.Topics.Add(id);
                CreateReport();
            }
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
    }
}
