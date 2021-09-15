using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{

    public interface ICompareProp
    {
        string Name { get; }
        double GetModDiff();
        double GetTotalDiff();
        bool CompareEqual();
        void SetCompare(ICompareProp compare);
    }
    public class CompareProperty<T> : Named, ICompareProp
    {
        public T Value { get; private set; }
        protected T ValueCompareWith { get; private set; }


        public virtual double GetModDiff() => 0;
        public virtual double GetTotalDiff() => 0;


        public string Format { get; set; }

        public CompareProperty(T source)
        {
            Value = source;
        }
        public void SetCompare(ICompareProp compare)
        {
            if (compare is CompareProperty<T> prop)
            {
                ValueCompareWith = prop.Value;
            }
        }

        public virtual bool CompareEqual() => ValueCompareWith != null ? Value.Equals(ValueCompareWith) : true;
    }
    public class DoubleProperty : CompareProperty<double>
    {
        public override string ToString()
        {
            return Value.ToString(Format);
        }

        public override double GetModDiff()
        {
            return Value / ValueCompareWith;
        }
        public override double GetTotalDiff()
        {
            return Value - ValueCompareWith;
        }

        public DoubleProperty(string name, string format, double source) : base(source)
        {
            Name = name;
            Format = format;
        }
    }
    public class DateProperty : CompareProperty<DateTime>
    {
        public override string ToString()
        {
            return Value.ToString(Format);
        }

        public override double GetTotalDiff()
        {
            return (Value - ValueCompareWith).TotalDays;
        }
        public override double GetModDiff()
        {
            return 0;
        }

        public DateProperty(string name, DateTime date) : base(date)
        {
            Name = name;
            Format = "dd-MM-yyyy";
        }
    }
}
