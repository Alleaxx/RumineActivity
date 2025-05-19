using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
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
}
