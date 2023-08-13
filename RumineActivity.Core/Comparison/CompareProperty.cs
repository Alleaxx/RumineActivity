using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core.Comparisons
{
    public interface ICompareProp
    {
        object Source { get; }
        string GetFormattedValue();

        string GetFormattedCompareValue(CompareDiffFormats view);
        string GetFormattedTotalDiffValue();
        string GetFormattedModDiffValue();

        double GetModDiff();
        double GetTotalDiff();


        bool IsNoDifference();
        void SetValueCompareWith(ICompareProp compare);
    }
    public class CompareProperty<T> : ICompareProp
    {
        public override string ToString()
        {
            return GetFormattedValue();
        }

        public object Source { get; init; }
        public T Value { get; private set; }
        protected T ValueCompareWith { get; private set; }


        public virtual double GetModDiff()
        {
            return 0;
        }
        public virtual double GetTotalDiff()
        {
            return 0;
        }
        public virtual bool IsNoDifference()
        {
            return ValueCompareWith != null ? Value.Equals(ValueCompareWith) : true;
        }

        private Func<T, string> FormatFunc { get; init; }
        protected Func<double, string> FormatModDiffFunc { get; set; }
        protected Func<double, string> FormatTotalDiffFunc { get; set; }

        public string GetFormattedValue()
        {
            return FormatFunc.Invoke(Value);
        }
        public string GetFormattedModDiffValue()
        {
            var val = GetModDiff();
            return FormatModDiffFunc?.Invoke(val) ?? $"{val:0%}";
        }
        public string GetFormattedTotalDiffValue()
        {
            var val = GetTotalDiff();
            var intFormat = $"{GetSymbol(val)}{val:#,0}";
            var doubleFormat = $"{GetSymbol(val)}{val:#,0.00}";

            if(val >= 1 || val <= -1 || val == 0)
            {
                return FormatTotalDiffFunc?.Invoke(val) ?? intFormat;
            }
            else
            {
                return FormatTotalDiffFunc?.Invoke(val) ?? doubleFormat;
            }


        }
        protected string GetSymbol(double val)
        {
            return val > 0 ? "+" : string.Empty;
        }

        public CompareProperty(object source, T property, Func<T, string> formatFunc = null)
        {
            Source = source;
            Value = property;
            FormatFunc = (obj) => obj.ToString();
            if (formatFunc != null)
            {
                FormatFunc = formatFunc;
            }
        }
        public void SetValueCompareWith(ICompareProp compare)
        {
            if (compare is CompareProperty<T> prop)
            {
                ValueCompareWith = prop.Value;
            }
        }



        public string GetFormattedCompareValue(CompareDiffFormats view)
        {
            return view == CompareDiffFormats.Absolute ? GetFormattedTotalDiffValue() : GetFormattedModDiffValue();
        }
    }


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
    public class DateRangeProperty : CompareProperty<DateRange>
    {
        public override double GetTotalDiff()
        {
            return (Value.From - ValueCompareWith.From).TotalDays;
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
