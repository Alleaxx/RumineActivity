using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //График и линия графика с точками
    public class Graphic : DiagramChart
    {
        public EntryLine[] Lines { get; private set; }


        public Graphic(StatisticsReport report, DiagramSize options) : base(report, options) { }

        protected override void CreateChart()
        {
            List<EntryLine> lines = new List<EntryLine>();

            EntryLine prevEntry = new EntryLine(Size.HeightChart);
            for (int i = 0; i < Report.Length; i++)
            {
                var currentEntry = Report.Entries[i];
                EntryLine newLine = new EntryLine(prevEntry, currentEntry, this);
                lines.Add(newLine);
                prevEntry = newLine;
            }

            Lines = lines.ToArray();
        }
        public string CreatePathAttribute()
        {
            StringBuilder sb = new StringBuilder(Lines.Length * 4 + 10);
            sb.Append($"M 0 {Size.HeightChart}");
            foreach (var entryLine in Lines)
            {
                sb.Append(entryLine.Point.Path);
            }
            return sb.ToString();
        }
    }

    public class EntryLine : DiagramObject
    {
        public Point Point { get; private set; }
        public Line Line { get; private set; }

        public EntryLine(double maxHeight)
        {
            Point = new Point()
            {
                X = 0
            };
            Line = new Line()
            {
                X2 = 0,
                Y2 = (int)maxHeight
            };
        }
        public EntryLine(EntryLine prev, Entry entry, DiagramChart chart) : base(prev, entry, chart)
        {
            int height = CountHeight();
            int y = height;

            int width = CountWidth();
            int x = width + prev.Point.X;

            Point = new Point()
            {
                X = x,
                Y = MaxAllowedHeight - y
            };
            Line = new Line()
            {
                X1 = prev.Line.X2,
                Y1 = prev.Line.Y2,
                X2 = Point.X,
                Y2 = Point.Y
            };
        }
    }
}
