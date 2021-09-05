using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public abstract class Rule
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public abstract bool Check(Entry entry);
    }

    public class RuleRanged : Rule
    {
        public double Min { get; set; }
        public double Max { get; set; }
        public override bool Check(Entry entry) => entry.ValueRelative >= Min && entry.ValueRelative < Max;
        public RuleRanged()
        {

        }
        public RuleRanged(string name, string color)
        {

        }
    }
}
