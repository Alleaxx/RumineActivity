using RumineActivity.Core.Comparisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Comparisons
{

    public class DoubleProperty : CompareProperty<double>
    {
        public override double GetModDiff()
        {
            return Value / ValueCompareWith;
        }
        public override double GetTotalDiff()
        {
            return Value - ValueCompareWith;
        }

        public DoubleProperty(object source, double property, Func<double, string> formatFunc = null) : base(source, property, formatFunc)
        {

        }
    }
}
