using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum MeasureMethods
    {
        Total, ByHour, ByDay, ByMonth,
    }
    public class MeasureMethod : EnumType<MeasureMethods>
    {
        public MeasureMethod(MeasureMethods method) : base(method)
        {
            switch (Type)
            {
                case MeasureMethods.ByDay:
                    Name = "в среднем в день";
                    break;
                case MeasureMethods.ByHour:
                    Name = "в среднем в час";
                    break;
                case MeasureMethods.ByMonth:
                    Name = "в среднем за месяц";
                    break;
                case MeasureMethods.Total:
                    Name = "Всего за период";
                    break;
            }
        }

        public double GetValue(Entry entry)
        {
            return GetValue(entry.PostsWritten, entry.Range);
        }
        public double GetValue(double all, DateRange range)
        {
            switch (Type)
            {
                default:
                    return all;
                case MeasureMethods.ByDay:
                    return all / range.DaysDifference;
                case MeasureMethods.ByMonth:
                    return all / range.DaysDifference * 30;
                case MeasureMethods.ByHour:
                    return all / range.Diff.TotalHours;
            }
        }
    }

}
