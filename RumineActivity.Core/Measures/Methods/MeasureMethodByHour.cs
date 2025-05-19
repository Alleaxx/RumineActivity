using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
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
