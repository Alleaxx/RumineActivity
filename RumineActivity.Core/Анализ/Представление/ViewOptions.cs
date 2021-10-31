using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class ViewOptions
    {
        public ViewRules Rules { get; private set; }

        public int RoundAccuracy { get; set; } = 2;

        public MeasureUnit MeasureUnit { get; set; }
        public MeasureMethod MeasureMethod { get; set; }
        private OutputValue OutValue { get; set; }


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
            
            string format = NumberFormatDouble();
            if(method == MeasureMethods.Total && MeasureUnit.Type == MeasureUnits.Messages)
            {
                format = NumberFormatInt();
            }

            return value.ToString(format);
        }

        private string ZeroFormat()
        {
            return ZeroFormat(RoundAccuracy);
        }
        private string ZeroFormat(int amount)
        {
            return string.Join("", Enumerable.Repeat("0", amount));
        }
       
        private string NumberFormatDouble()
        {
            return $"#,0.{ZeroFormat()}";
        }
        public string NumberFormatInt()
        {
            return $"#,0";
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
