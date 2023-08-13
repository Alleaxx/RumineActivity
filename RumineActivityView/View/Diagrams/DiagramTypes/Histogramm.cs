using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Гистограмма
    /// </summary>
    public class Histogram : DiagramChart
    {
        /// <summary>
        /// Коллекция прямоугольников для периодов
        /// </summary>
        public RectangleEntryObject[] Rectangles { get; private set; }

        public Histogram(ValuesViewConfig valuesConfig, StatisticsReport report, DiagramConfig options) : base(valuesConfig, report, options)
        {

        }

        protected override void CreateChart()
        {
            RectangleEntryObject prevEntry = null;
            List<RectangleEntryObject> rects = new List<RectangleEntryObject>();
            var entries = ValuesConfig.SortingEntriesSelected.Descending
                ? Report.Entries.OrderByDescending(ValuesConfig.SortingEntriesSelected.SortFunc).ToArray()
                : Report.Entries.OrderBy(ValuesConfig.SortingEntriesSelected.SortFunc).ToArray();
            for (int i = 0; i < entries.Length; i++)
            {
                Entry entry = entries[i];
                RectangleEntryObject newRect = RectangleEntryObject.CreateEntryRectangle(this, prevEntry, entry);
                rects.Add(newRect);
                prevEntry = newRect;
            }
            Rectangles = rects.ToArray();

            FillLegendLines();
            FillLegendLabels(Rectangles);
            ReduceLegendLabels();
        }
    }


}
