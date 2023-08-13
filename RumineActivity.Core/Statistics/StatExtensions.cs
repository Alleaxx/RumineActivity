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
        public static string GetCSVString(this StatisticsReport report, MeasureMethods method, MeasureUnits unit, string sep = ";")
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Index");
            sb.Append(sep);
            sb.Append("Period");
            sb.Append(sep);
            sb.Append("PeriodStart");
            sb.Append(sep);
            sb.Append("PeriodEnd");
            sb.Append(sep);
            sb.Append("TotalWritten");
            sb.Append(sep);
            sb.Append("AverageWritten");
            sb.Append(sep);
            sb.Append("Mode");
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
                sb.Append(entry.GetValue(MeasureMethods.Total, unit));
                sb.Append(sep);
                sb.Append(entry.GetValue(method, unit));
                sb.Append(sep);
                sb.Append(entry.FractionMode);
                sb.Append(sep);
                sb.Append(entry.PostBorders.Accuracy.Name);
                sb.Append("\n");
            }

            return sb.ToString();
        }
        public static string GetTrend(this Entry entry)
        {
            if (entry.PreviousEntry == null)
            {
                return "?";
            }

            double difference = entry.PostsWrittenTotal / entry.PreviousEntry.PostsWrittenTotal;
            double diffMode;
            double border = 0.05;
            if (difference > 1)
            {
                diffMode = difference - 1;

                if (diffMode > border)
                {
                    return "🡥";
                }
                else
                {
                    return "~=";
                }
            }
            else if (difference < 1)
            {
                diffMode = 1 - difference;

                if (diffMode > border)
                {
                    return "🡦";
                }
                else
                {
                    return "~";
                }
            }
            else
            {
                return "=";
            }
        }

        public static Period? GetDeeperPeriod(this StatisticsReport report)
        {
            return EnumValues.Periods.OrderByDescending(p => p.TimeInterval)
                .FirstOrDefault(p => report.DateRangeAll.IsOkWithPeriod(p) && p.TimeInterval < report.Period.TimeInterval && p.Type != Periods.Own);
        }
        public static Period? GetHeigherPeriod(this StatisticsReport report)
        {
            return EnumValues.Periods.OrderBy(p => p.TimeInterval)
                .FirstOrDefault(p => report.DateRangeAll.IsOkWithPeriod(p) && p.TimeInterval > report.Period.TimeInterval && p.Type != Periods.Own);
        }


        public static double FindNearestMultiFive(double val, double precision = 2)
        {
            return Math.Round(val * precision) / precision;
        }
    }
}
