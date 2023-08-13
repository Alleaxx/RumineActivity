using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core.Comparisons
{
    public enum CompareDiffFormats
    {
        Relative,
        Absolute
    }
    public abstract class CompareDiffFormat : EnumType<CompareDiffFormats>
    {
        public static CompareDiffFormat Create(CompareDiffFormats compare)
        {
            switch (compare)
            {
                case CompareDiffFormats.Relative:
                    return new CompareFormatRelative();
                case CompareDiffFormats.Absolute:
                    return new CompareFormatAbsolute();
                default:
                    throw new Exception($"Преобразование типа {compare} создать нельзя");
            }
        }

        protected CompareDiffFormat(CompareDiffFormats values) : base(values) { }
        public abstract string GetValue(ICompareProp prop);
    }

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