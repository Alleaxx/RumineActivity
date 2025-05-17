using RumineActivity.Core;
using RumineActivity.Core.Measures;
using RumineActivity.View.Diagrams.SVG;

namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Объект линии для записи
    /// </summary>
    public class LineEntryObject : DiagramEntryObject
    {
        /// <summary>
        /// Точка - конечное "значение" для периода
        /// </summary>
        public Point Point { get; private set; }
        /// <summary>
        /// Линия - от конечного до начального значения периода
        /// </summary>
        public Line Line { get; private set; }


        /// <summary>
        /// Создание линии записи для обычного графика
        /// </summary>
        public static LineEntryObject CreateGraphicLine(DiagramChart chart, LineEntryObject prevLine, Entry entry)
        {
            return new LineEntryObject(chart, prevLine, entry);
        }
        private LineEntryObject(DiagramChart chart, LineEntryObject prevLine, Entry entry) : base(chart, entry)
        {
            double xStart;
            double xEnd;
            double yStart;
            double yEnd;

            bool isFirst = prevLine == null;
            if (isFirst)
            {
                Index = 0;

                xStart = chart.DiagramConfig.WidthLegend;
                xEnd = chart.DiagramConfig.WidthLegend + GetCountedWidth();
                yStart = MaxAllowedHeight;
                yEnd = MaxAllowedHeight - GetCountedHeight();
            }
            else
            {
                Index = prevLine.Index + 1;

                xStart = prevLine.Line.X2;
                xEnd = prevLine.Point.X + GetCountedWidth();
                yStart = prevLine.Line.Y2;
                yEnd = MaxAllowedHeight - GetCountedHeight();
            }

            Point = new Point()
            {
                X = xEnd,
                Y = yEnd
            };
            Line = new Line()
            {
                X1 = xStart,
                Y1 = yStart,
                X2 = xEnd,
                Y2 = yEnd
            };
        }

        /// <summary>
        /// Создание линии записи для насыщенного графика
        /// </summary>
        /// 
        public static LineEntryObject CreateRichGraphicLine(DiagramChart chart, double totalSum, double currentSum, LineEntryObject prevLine, Entry entry)
        {
            return new LineEntryObject(chart, totalSum, currentSum, prevLine, entry);
        }
        private LineEntryObject(DiagramChart chart, double totalSum, double currentSum, LineEntryObject prevLine, Entry entry) : base(chart, entry)
        {
            double xStart;
            double xEnd;
            double yStart;
            double yEnd;


            var thisValue = entry.GetValue(MeasureMethods.Total, chart.ValuesConfig.MeasureUnit);
            double heightStart = currentSum / totalSum * MaxAllowedHeight;
            double heightEnd = (currentSum + thisValue) / totalSum * MaxAllowedHeight;
            bool isFirst = prevLine == null;
            if (isFirst)
            {
                Index = 0;

                xStart = chart.DiagramConfig.WidthLegend;
                xEnd = chart.DiagramConfig.WidthLegend + GetCountedWidth();
                yStart = MaxAllowedHeight - heightStart;
                yEnd = MaxAllowedHeight - heightEnd;
            }
            else
            {
                Index = prevLine.Index + 1;

                xStart = prevLine.Line.X2;
                xEnd = prevLine.Line.X2 + GetCountedWidth();
                yStart = MaxAllowedHeight - heightStart;
                yEnd = MaxAllowedHeight - heightEnd;
            }

            Point = new Point()
            {
                X = xEnd,
                Y = yEnd
            };
            Line = new Line()
            {
                X1 = xStart,
                Y1 = yStart,
                X2 = xEnd,
                Y2 = yEnd
            };
            TitleValue = chart.ValuesConfig.FormatValue(thisValue + currentSum, Periods.Month, MeasureMethods.Total, ownFormat: "#,0");
        }

        /// <summary>
        /// Создание конечной линии для насыщенного графика
        /// </summary>
        public static LineEntryObject CreateRichGraphicEndLine(DiagramChart chart, LineEntryObject lastEntryLine)
        {
            return new LineEntryObject(chart, lastEntryLine);
        }
        private LineEntryObject(DiagramChart chart, LineEntryObject lastEntryLine) : base(chart, lastEntryLine.Entry)
        {
            Index = lastEntryLine.Index + 1;

            double xEnd = lastEntryLine.Point.X;
            double yEnd = chart.DiagramConfig.HeightChart;

            Point = new Point()
            {
                X = xEnd,
                Y = yEnd
            };
            Line = new Line()
            {
                X1 = lastEntryLine.Line.X2,
                Y1 = lastEntryLine.Line.Y2,
                X2 = xEnd,
                Y2 = yEnd
            };
        }

    }
}
