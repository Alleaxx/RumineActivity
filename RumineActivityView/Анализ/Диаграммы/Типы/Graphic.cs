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

        public string GetPathAttribute()
        {
            StringBuilder sb = new StringBuilder(Lines.Length * 4 + 10);
            sb.Append($"M 0 {Options.HeightChart}");
            foreach (var entryLine in Lines)
            {
                sb.Append(entryLine.Point.Path);
            }
            return sb.ToString();
        }


        public Graphic(StatisticsReport report, DiagramSize options) : base(report, options)
        {

        }

        protected override void CreateChart()
        {
            List<EntryLine> line = new List<EntryLine>();

            double maxedValue = Report.MostActive.PostsRelative * MaxValueMod;

            EntryLine prevEntry = null;
            for (int i = 0; i < Report.Entries.Length; i++)
            {
                var currentEntry = Report.Entries[i];
                EntryLine newLine = new EntryLine(prevEntry, i, currentEntry, maxedValue, Options, Report.FirstLastPost);
                line.Add(newLine);
                prevEntry = newLine;
            }

            Lines = line.ToArray();
        }
    }
    public class EntryLine
    {
        public int Index { get; set; }
        public Entry Entry { get; set; }
        public Point Point { get; set; }
        public Line Line { get; set; }

        public EntryLine(EntryLine prev, int index, Entry entry, double max, DiagramSize options, DateRange range)
        {
            Index = index;
            Entry = entry;

            int height = (int)(entry.PostsRelative / max * options.HeightChart);
            int y = height;

            //Ширина
            double widthDays = entry.Range.Diff.TotalDays;
            double allDays = range.Diff.TotalDays;
            int width = Math.Max(1, (int)(widthDays / allDays * options.Width));

            //Смещение по ширине
            int x = prev == null ? width : width + prev.Point.X;

            Point = new Point()
            {
                X = x,
                Y = (int)(options.HeightChart - y)
            };

            Line = new Line()
            {
                X1 = prev != null ? prev.Line.X2 : 0,
                Y1 = prev != null ? prev.Line.Y2 : (int)(options.HeightChart),
                X2 = Point.X,
                Y2 = Point.Y
            };
        }
    }
}
