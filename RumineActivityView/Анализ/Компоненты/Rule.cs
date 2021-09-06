using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{

    public class ViewOptions
    {
        public List<Rule> Rules { get; private set; } = new List<Rule>();
        public Rule CheckEntry(Entry entry) => Rules.FirstOrDefault(r => r.Check(entry));
        public void Add(string name, string color, double from, double to)
        {
            Rules.Add(new RuleRanged(name, color, from, to));
        }


        public int RoundAccuracy { get; set; } = 2;

        public MeasureUnit MeasureUnit { get; set; } = new MeasureUnit(MeasureUnits.Messages);
        public MeasureMethod MeasureMethod { get; set; } = new MeasureMethod(MeasureMethods.Total);

        public ViewOptions()
        {
            Add("", "#E6E6E6", 0, 1.5);
            Add("", "#fffab5", 1.5, 5);
            Add("", "#BDECB6", 5, 15);
            Add("", "#C7DFFA", 15, 30);
            Add("", "#d8bee8", 30, 70);
            Add("", "#fccd6f", 70, 200);
            Rules.Add(new Rule());
        }
    }


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
