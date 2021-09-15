using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class ViewOptions
    {
        public ViewRules Rules { get; private set; }

        public int RoundAccuracy { get; set; } = 2;

        public MeasureUnit MeasureUnit { get; set; }
        public MeasureMethod MeasureMethod { get; set; }
        public OutputValue OutValue { get; set; }


        public string FormatEntry(Entry entry)
        {
            return FormatEntry(entry, MeasureMethod);
        }
        public string FormatEntry(Entry entry, MeasureMethods method)
        {
            double value = entry.GetPosts(method, MeasureUnit);
            if (value == 0)
            {
                return "???";
            }
            else
            {
                return value.ToString(NumberFormat());
            }
        }


        public string ZeroFormat()
        {
            return string.Join("", Enumerable.Repeat("0", RoundAccuracy));
        }
        public string NumberFormat()
        {
            return $"#,0.{ZeroFormat()}";
        }


        public ViewOptions()
        {
            Rules = new ViewRules();
            MeasureUnit = new MeasureUnit(MeasureUnits.Messages);
            MeasureMethod = MeasureMethod.Create(MeasureMethods.ByDay);
            OutValue = new OutputValue(PostOutputs.PeriodDifference);
        }
    }
}
