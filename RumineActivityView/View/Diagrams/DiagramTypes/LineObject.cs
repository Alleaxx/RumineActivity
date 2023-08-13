using RumineActivity.Core;
using RumineActivity.View.Diagrams.SVG;
using RumineActivity.View.Diagrams;

namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Объект линии
    /// </summary>
    public class LineObject : DiagramObject
    {
        public Line Line { get; protected set; }

        /// <summary>
        /// Создание линии легенды
        /// </summary>
        public LineObject(DiagramChart chart, double mod, double value, Period period) : base(chart)
        {
            TitleValue = chart.ValuesConfig.FormatValue(value, period);
            double height = ((1 - mod) * MaxAllowedHeight);
            double y = height;

            double xStart = 5;
            double xEnd = chart.DiagramConfig.Width;

            Line = new Line()
            {
                X1 = xStart,
                X2 = xEnd,
                Y1 = y,
                Y2 = y
            };
        }
    }
}
