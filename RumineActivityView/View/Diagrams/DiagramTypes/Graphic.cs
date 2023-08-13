using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RumineActivity.Core;
namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Обычный график, используется как основного для графика с накоплением
    /// </summary>
    public class Graphic : DiagramChart
    {
        /// <summary>
        /// Коллекция линий для периодов
        /// </summary>
        public LineEntryObject[] Lines { get; protected set; }

        public Graphic(ValuesViewConfig valuesConfig, StatisticsReport report, DiagramConfig options) : base(valuesConfig, report, options) { }

        protected override void CreateChart()
        {
            List<LineEntryObject> lines = new List<LineEntryObject>();

            LineEntryObject prevEntry = null;
            for (int i = 0; i < Report.Length; i++)
            {
                var currentEntry = Report.Entries[i];
                LineEntryObject newLine = LineEntryObject.CreateGraphicLine(this, prevEntry, currentEntry);
                lines.Add(newLine);
                prevEntry = newLine;
            }
            FillLegendLines();
            Lines = lines.ToArray();
        }

        /// <summary>
        /// Построить график по конечным точкам маршрута
        /// </summary>
        public string CreatePathAttribute()
        {
            StringBuilder sb = new StringBuilder(Lines.Length * 4 + 10);
            sb.Append($"M {DiagramConfig.WidthLegend} {DiagramConfig.HeightChart}");
            foreach (var entryLine in Lines)
            {
                sb.Append(entryLine.Point.PathAttr);
            }
            return sb.ToString();
        }
    }
}
