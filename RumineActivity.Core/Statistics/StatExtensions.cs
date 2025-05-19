using RumineActivity.Core.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public static class StatExtensions
    {
        /// <summary>
        /// Преобразовать записи отчета в CSV-строку
        /// </summary>
        public static string GetCSVString(this StatisticsReport report, MeasureMethods method, string sep = ";")
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Index");
            sb.Append(sep);
            sb.Append("PeriodTitle");
            sb.Append(sep);
            sb.Append("DateStart");
            sb.Append(sep);
            sb.Append("DateEnd");
            sb.Append(sep);
            sb.Append("PagesTotal");
            sb.Append(sep);
            sb.Append($"PagesAverage{method}");
            sb.Append(sep);
            sb.Append("PostsTotal");
            sb.Append(sep);
            sb.Append($"PostsAverage{method}");
            sb.Append(sep);
            sb.Append("Fraction");
            sb.Append(sep);
            sb.Append("Accuracy");
            sb.Append("\n");
            foreach (var entry in report.Entries)
            {
                sb.Append(entry.Index);
                sb.Append(sep);
                sb.Append(entry.Range.GetName(report.Period.EntryDateFunc));
                sb.Append(sep);
                sb.Append(entry.FromDate.ToString());
                sb.Append(sep);
                sb.Append(entry.ToDate.ToString());
                sb.Append(sep);
                sb.Append(entry.GetValue(MeasureMethods.Total, MeasureUnits.Pages).ToString("0.00"));
                sb.Append(sep);
                sb.Append(entry.GetValue(method, unit: MeasureUnits.Pages).ToString("0.00"));
                sb.Append(sep);
                sb.Append(entry.GetValue(MeasureMethods.Total, MeasureUnits.Messages));
                sb.Append(sep);
                sb.Append(entry.GetValue(method, unit: MeasureUnits.Messages).ToString("0.00"));
                sb.Append(sep);
                sb.Append(entry.FractionMode);
                sb.Append(sep);
                sb.Append(entry.PostBorders.Accuracy.Name);
                sb.Append("\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Получить более детальное доступное деление для отчета 
        /// </summary>
        public static Period? GetDeeperPeriod(this StatisticsReport report)
        {
            return EnumValues.PeriodsList.OrderByDescending(p => p.TimeInterval)
                .FirstOrDefault(p => report.DateRangeAll.IsOkWithPeriod(p) && p.TimeInterval < report.Period.TimeInterval && p.Type != Periods.Own);
        }
        
        /// <summary>
        /// Получить менее детальное доступное деление для отчета
        /// </summary>
        public static Period? GetHeigherPeriod(this StatisticsReport report)
        {
            return EnumValues.PeriodsList.OrderBy(p => p.TimeInterval)
                .FirstOrDefault(p => report.DateRangeAll.IsOkWithPeriod(p) && p.TimeInterval > report.Period.TimeInterval && p.Type != Periods.Own);
        }

        /// <summary>
        /// Получить число разрядов
        /// </summary>
        public static int GetTens(double val)
        {
            int counter = 1;
            while (val / Math.Pow(10.0, counter) >= 1)
            {
                counter++;
            }
            return counter;
        }
        public static int GetZeros(double val)
        {
            int counter = 1;
            while (val * Math.Pow(10.0, counter) < 1)
            {
                counter++;
            }
            return counter;
        }
    }
}
