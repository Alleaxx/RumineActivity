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

}