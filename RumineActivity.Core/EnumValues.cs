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
        //Измерение
        public static MeasureMethod[] Methods { get; private set; } = Enum.GetValues<MeasureMethods>().Select(u => MeasureMethod.Create(u)).ToArray();
        public static OutputValue[] Outs { get; private set; } = Enum.GetValues<PostOutputs>().Select(p => new OutputValue(p)).ToArray();
        public static MeasureUnit[] Units { get; private set; } = Enum.GetValues<MeasureUnits>().Select(u => new MeasureUnit(u)).ToArray();
        public static CompareDiffFormat[] CompareValues { get; private set; } = Enum.GetValues<CompareDiffFormats>().Select(v => CompareDiffFormat.Create(v)).ToArray();

        //Настройки отчета
        public static Period[] Periods { get; private set; } = Enum.GetValues<Periods>().Select(d => Period.Create(d)).ToArray();

    }
}
