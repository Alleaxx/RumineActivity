using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
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
}
