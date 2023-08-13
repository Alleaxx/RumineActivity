using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    public enum MeasureMethods
    {
        Total,
        ByHour,
        ByDay,
        ByMonth,
    }

    public abstract class MeasureMethod : EnumType<MeasureMethods>
    {
        public static MeasureMethod Create(MeasureMethods method)
        {
            switch (method)
            {
                case MeasureMethods.ByMonth:
                    return new MeasureMethodByMonth();
                case MeasureMethods.ByHour:
                    return new MeasureMethodByHour();
                case MeasureMethods.ByDay:
                    return new MeasureMethodByDay();
                default:
                    return new MeasureMethodTotal();
            }
        }

        protected MeasureMethod(MeasureMethods method) : base(method) { }
        public abstract double GetValue(double all, DateRange range);
    }
    public class MeasureMethodTotal : MeasureMethod
    {
        public MeasureMethodTotal() : base(MeasureMethods.Total)
        {
            Name = "Всего за период";
        }
        public override double GetValue(double all, DateRange range)
        {
            return all;
        }
    }

    public class MeasureMethodByMonth : MeasureMethod
    {
        public MeasureMethodByMonth() : base(MeasureMethods.ByMonth)
        {
            Name = "в среднем за месяц";
        }
        public override double GetValue(double all, DateRange range)
        {
            return all / range.DaysDifference * 30;
        }
    }

    public class MeasureMethodByDay : MeasureMethod
    {
        public MeasureMethodByDay() : base(MeasureMethods.ByDay)
        {
            Name = "в среднем в день";
        }
        public override double GetValue(double all, DateRange range)
        {
            return all / range.DaysDifference;
        }
    }

    public class MeasureMethodByHour : MeasureMethod
    {
        public MeasureMethodByHour() : base(MeasureMethods.ByHour)
        {
            Name = "в среднем в час";
        }
        public override double GetValue(double all, DateRange range)
        {
            return all / range.Diff.TotalHours;
        }
    }
}
