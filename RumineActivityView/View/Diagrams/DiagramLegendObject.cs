using RumineActivity.View.Diagrams.SVG;
using RumineActivity.View.Diagrams;
using System.Collections.Generic;
using System.Linq;
using RumineActivity.Core;
using System;

namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Подпись и границы к периоду на горизонтальной оси
    /// </summary>
    public class DiagramLegendObject : DiagramObject
    {
        /// <summary>
        /// Левая граница периода
        /// </summary>
        public Line LeftBorder { get; private set; }

        /// <summary>
        /// Центральная точка между границами для текста
        /// </summary>
        public Point Center { get; private set; }

        /// <summary>
        /// Центральная точка между границами для текста НАД столбцом
        /// </summary>
        public Point CenterAboveRectangle { get; private set; }

        /// <summary>
        /// Правая граница периода
        /// </summary>
        public Line RightBorder { get; private set; }

        public DiagramLegendObject(DiagramChart chart, StatisticsReport report, IGrouping<string, DiagramEntryObject> objects, DiagramLegendObject previous) : base(chart)
        {
            TitleName = objects.Key;

            var ordered = objects;
            var firstObject = ordered.First();
            var lastObject = ordered.Last();

            //отображаем значение, если группировка по факту не действует
            if (firstObject.Entry == lastObject.Entry)
            {
                TitleValue = objects.First().TitleValue;
            }

            var dateFraction = new DateRange(firstObject.Entry.FromDate, lastObject.Entry.ToDate).DaysDifference / report.DateRangePosts.DaysDifference;
            bool isInverted = firstObject.Entry.FromDate > lastObject.Entry.FromDate;

            double xStart;
            double width = (firstObject.MaxAllowedWidth) * dateFraction;
            if (previous == null)
            {
                xStart = chart.DiagramConfig.WidthLegend;
            }
            else
            {
                xStart = previous.RightBorder.X2;
            }
            double xEnd = xStart + width;
            double yStart = chart.DiagramConfig.HeightChart;
            double yEnd = chart.DiagramConfig.HeightChart + 15;
            if(firstObject is RectangleEntryObject rectFirst && lastObject is RectangleEntryObject rectLast)
            {
                xStart = rectFirst.Filled.X;
                xEnd = rectLast.Filled.X + rectLast.Filled.Width;
            }
            if (isInverted)
            {
                var oldXStart = xStart;
                xStart = xEnd;
                xEnd = oldXStart;
            }


            LeftBorder = new Line()
            {
                X1 = xStart,
                X2 = xStart,
                Y1 = yStart,
                Y2 = yEnd
            };
            RightBorder = new Line()
            {
                X1 = xEnd,
                X2 = xEnd,
                Y1 = yStart,
                Y2 = yEnd
            };
            Center = new Point()
            {
                X = !isInverted ? xStart + Math.Abs(xEnd - xStart) / 2 : xEnd + Math.Abs(xEnd - xStart) / 2,
                Y = yStart + 15
            };

            bool startLineFromZero = false;
            if (startLineFromZero)
            {
                LeftBorder.Y1 = 0;
                RightBorder.Y1 = 0;
            }
        }

        /// <summary>
        /// Скрыть название, если ширина элемента меньше половины сравниваемого (соседнего) элемента
        /// </summary>
        public void HideTitlesIfSmall(DiagramLegendObject compareObject)
        {
            if(compareObject == null)
            {
                return;
            }
            var thisWidth = Math.Abs(RightBorder.X1 - LeftBorder.X1);
            var exampleWidth = Math.Abs(compareObject.RightBorder.X1 - compareObject.LeftBorder.X1);
            if (thisWidth < exampleWidth * 0.6)
            {
                HideTitles();
            }
        }

        /// <summary>
        /// Скрыть границы
        /// </summary>
        public void HideBorders()
        {
            LeftBorder = new Line()
            {
                X1 = 0,
                X2 = 0,
                Y1 = 0,
                Y2 = 0
            };
            RightBorder = new Line()
            {
                X1 = 0,
                X2 = 0,
                Y1 = 0,
                Y2 = 0
            };
        }

        /// <summary>
        /// Скрыть названия
        /// </summary>
        public void HideTitles()
        {
            TitleValue = string.Empty;
            TitleName = string.Empty;
        }
    }
}
