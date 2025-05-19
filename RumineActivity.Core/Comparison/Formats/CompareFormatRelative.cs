using RumineActivity.Core.Comparisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RumineActivity.Core.Comparisons
{
    class CompareFormatRelative : CompareDiffFormat
    {
        public CompareFormatRelative() : base(CompareDiffFormats.Relative)
        {
            Name = "Проценты";
        }
        public override string GetValue(ICompareProp prop)
        {
            return prop.GetModDiff().ToString("0%");
        }
    }
}
