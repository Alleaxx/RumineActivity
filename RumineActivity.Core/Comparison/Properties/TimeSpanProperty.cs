using RumineActivity.Core.Comparisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Comparisons
{
    public class TimeSpanProperty : CompareProperty<TimeSpan>
    {
        public override double GetTotalDiff()
        {
            return (Value - ValueCompareWith).TotalDays;
        }
        public override double GetModDiff()
        {
            return 0;
        }

        public TimeSpanProperty(object source, TimeSpan property, Func<TimeSpan, string> formatFunc) : base(source, property, formatFunc)
        {
            FormatModDiffFunc = entry => $"-";
        }
    }
}
