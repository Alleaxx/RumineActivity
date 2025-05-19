using RumineActivity.Core.Comparisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RumineActivity.Core.Comparisons
{

    class CompareFormatAbsolute : CompareDiffFormat
    {
        public CompareFormatAbsolute() : base(CompareDiffFormats.Absolute)
        {
            Name = "Значения";
        }
        public override string GetValue(ICompareProp prop)
        {
            return prop.GetTotalDiff().ToString("+#,0;-#,0; 0");
        }
    }
}
