using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
namespace RumineActivity.View
{
    public enum DisplayTypes
    {
        Values,
        Histogram,
        Graphic,
        BarChart,
        Table,
        Comparison,
        Debug
    }

    /// <summary>
    /// Вариант отображения отчета
    /// </summary>
    public class DisplayType : EnumType<DisplayTypes>
    {
        public string Icon { get; set; }
        private Predicate<StatisticsReport> CheckReportFunc { get; set; }


        public DisplayType(DisplayTypes diagram) : base(diagram)
        {
            CheckReportFunc = (report) => !report.IsEmpty;
            switch (diagram)
            {
                case DisplayTypes.Histogram:
                    Name = "Гистограмма";
                    Icon = "bar-chart-2.svg";
                    break;
                case DisplayTypes.Graphic:
                    Name = "График";
                    Icon = "trending-down.svg";
                    CheckReportFunc = (report) => false;
                    break;
                case DisplayTypes.BarChart:
                    Name = "График с накоплением";
                    Icon = "trending-up.svg";
                    break;
                case DisplayTypes.Values:
                    Name = "Список записей";
                    Icon = "list.svg";
                    break;
                case DisplayTypes.Table:
                    Name = "Таблица";
                    Icon = "grid.svg";
                    CheckReportFunc = (report) => report.Period.Type != Periods.Week;
                    break;
                case DisplayTypes.Comparison:
                    Name = "Сравнение";
                    Icon = "list.svg";
                    break;
                case DisplayTypes.Debug:
                    Name = "Отладочный";
                    Icon = "settings.svg";
                    CheckReportFunc = (report) => false;
                    break;
            }
        }

        public bool IsOkWithReport(StatisticsReport report)
        {
            return CheckReportFunc.Invoke(report);
        }
    }
}
