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
}
