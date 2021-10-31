using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core.Measures;

namespace RumineActivity.Core
{
    public enum CompareViews
    {
        Relative, Absolute
    }
    public abstract class CompareView : EnumType<CompareViews>
    {
        public static CompareView Create(CompareViews compare)
        {
            switch (compare)
            {
                case CompareViews.Relative:
                    return new CompareRelative();
                case CompareViews.Absolute:
                    return new CompareAbsolute();
                default:
                    throw new Exception($"Преобразование типа {compare} создать нельзя");
            }
        }

        protected CompareView(CompareViews values) : base(values) { }
        public abstract string GetValue(ICompareProp prop);
    }
}

namespace RumineActivity.Core.Measures
{
    class CompareRelative : CompareView
    {
        public CompareRelative() : base(CompareViews.Relative)
        {
            Name = "Проценты";
        }
        public override string GetValue(ICompareProp prop)
        {
            return prop.GetModDiff().ToString("0%");
        }
    }
    class CompareAbsolute : CompareView
    {
        public CompareAbsolute() : base(CompareViews.Absolute)
        {
            Name = "Значения";
        }
        public override string GetValue(ICompareProp prop)
        {
            return prop.GetTotalDiff().ToString("+#,0;-#,0; 0");
        }
    }
}