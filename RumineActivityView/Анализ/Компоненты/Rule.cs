using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class Rule
    {
        public string Name { get; set; }
        public string Color { get; set; }
        protected Predicate<Entry> CheckMethod { get; set; }

        public bool Check(Entry entry)
        {
            if (CheckMethod != null)
                return CheckMethod.Invoke(entry);
            else
                return true;
        }

        public Rule()
        {
            CheckMethod = EveryEntry;
        }
        private bool EveryEntry(Entry e) => true;
    }
    public class RuleRanged : Rule
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public RuleRanged(string name, string color, double from, double to)
        {
            CheckMethod = RangeCheck;
            Name = name;
            Color = color;
            Min = from;
            Max = to;
        }
        private bool RangeCheck(Entry entry) => entry.ValuePerDay / 20 >= Min && entry.ValuePerDay / 20 < Max;
    }
}
