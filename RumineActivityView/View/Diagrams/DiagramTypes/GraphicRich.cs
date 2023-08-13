using RumineActivity.Core;
using RumineActivity.Core.Measures;
using System.Collections.Generic;
using System.Linq;

namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// График с накоплением
    /// </summary>
    public class GraphicRich : Graphic
    {
        public GraphicRich(ValuesViewConfig valuesConfig, StatisticsReport report, DiagramConfig options) : base(valuesConfig, report, options)
        {

        }

        protected override void CreateChart()
        {
            List<LineEntryObject> lines = new List<LineEntryObject>();

            LineEntryObject prevEntry = null;
            double totalSum = GetMaxedValue();
            double currentSum = 0;
            for (int i = 0; i < Report.Length; i++)
            {
                var currentEntry = Report.Entries[i];
                LineEntryObject newLine = LineEntryObject.CreateRichGraphicLine(this, totalSum, currentSum, prevEntry, currentEntry);
                lines.Add(newLine);
                prevEntry = newLine;
                currentSum += currentEntry.GetValue(MeasureMethods.Total, ValuesConfig.MeasureUnit);
            }
            LineEntryObject last = LineEntryObject.CreateRichGraphicEndLine(this, prevEntry);
            lines.Add(last);
            Lines = lines.ToArray();

            FillLegendLines();
            FillLegendLabels(Lines);
            ReduceLegendLabels();
        }
        public override double GetMaxedValue()
        {
            return Report.Entries.Select(e => e.GetValue(MeasureMethods.Total, ValuesConfig.MeasureUnit)).Sum();
        }
    }
}
