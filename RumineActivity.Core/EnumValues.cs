using RumineActivity.Core.Comparisons;
using RumineActivity.Core.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public static class EnumValues
    {
        public static MeasureMethod[] Methods { get; private set; } = Enum.GetValues<MeasureMethods>()
            .Where(m => m != MeasureMethods.ByMonth)
            .Select(MeasureMethod.Create)
            .ToArray();
        public static OutputValue[] Outs { get; private set; } = Enum.GetValues<PostOutputs>()
            .Select(p => new OutputValue(p))
            .ToArray();
        public static MeasureUnit[] Units { get; private set; } = Enum.GetValues<MeasureUnits>()
            .Select(u => new MeasureUnit(u))
            .ToArray();
        public static CompareDiffFormat[] CompareValues { get; private set; } = Enum.GetValues<CompareDiffFormats>()
            .Select(v => CompareDiffFormat.Create(v))
            .ToArray();
        public static MaxValue[] MaximumValues { get; private set; } = Enum.GetValues<MaxValues>()
            .Select(m => new MaxValue(m))
            .ToArray();

        public static AccuracyRate[] AccuracyRatesList { get; private set; } = Enum.GetValues<AccuracyRates>()
            .Select(v => new AccuracyRate(v))
            .OrderBy(r => r.MaxErrorPosts)
            .ToArray();

        public static Period[] PeriodsList { get; private set; } = Enum.GetValues<Periods>()
            .Where(p => p != Periods.Season && p != Periods.Own )
            .Select(Period.Create)
            .ToArray();
    }
}
