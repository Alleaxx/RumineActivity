using RumineActivity.Core.Comparisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Comparisons
{

    public class DateRangeProperty : CompareProperty<DateRange>
    {
        public override double GetTotalDiff()
        {
            return Value != null && ValueCompareWith != null ? (Value.From - ValueCompareWith.From).TotalDays : 0;
        }
        public override double GetModDiff()
        {
            return 0;
        }

        public DateRangeProperty(object source, DateRange property, Func<DateRange, string> formatFunc = null) : base(source, property, formatFunc)
        {
            FormatModDiffFunc = entry => $"-";
            FormatTotalDiffFunc = entry => $"{GetSymbol(GetTotalDiff())}{GetTotalDiff():#,0} дн.";
        }
    }
}
