using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
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
}
