using RumineActivity.Core.Comparisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Comparisons
{

    public class DateProperty : CompareProperty<DateTime>
    {
        public override double GetTotalDiff()
        {
            return (Value - ValueCompareWith).TotalDays;
        }
        public override double GetModDiff()
        {
            return 0;
        }


        public DateProperty(object source, DateTime property, Func<DateTime, string> formatFunc = null) : base(source, property, formatFunc)
        {
            FormatModDiffFunc = entry => $"-";
        }
    }
}
