using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Настройки диаграммы
    public class DiagramOptions
    {
        public double Width { get; set; } = 1000;
        public double Height { get; set; } = 450;
    }
    public abstract class DiagramChart
    {
        public DiagramChart(StatisticsReport report)
        {

        }
    }

    //Диаграммы
    public class Gistogramm : DiagramChart
    {
        public EntryRectangle[] Rectangles { get; set; }
        
        public Gistogramm(StatisticsReport report, DiagramOptions options) : base(report)
        {
            if (!report.IsEmpty)
            {
                int count = report.Count;
                double max = report.MostActive.ValueRelative;
                Rectangles = report.Entries.Select((entry, index) => new EntryRectangle(index, entry, count, max, options)).ToArray();
            }
        }
    }
    public class Graphic : DiagramChart
    {
        public DiagramOptions Options { get; set; }
        public EntryLine[] Lines { get; set; }
        public string Path
        {
            get
            {
                StringBuilder sb = new StringBuilder(Lines.Length * 4);
                sb.Append($"M 0 {Options.Height}");
                foreach (var entryLine in Lines)
                {
                    sb.Append(entryLine.Point.Path);
                }
                return sb.ToString();
            }
        }
        public Graphic(StatisticsReport report, DiagramOptions options) : base(report)
        {
            Options = options;
            List<EntryLine> line = new List<EntryLine>();

            EntryLine prevEntry = null;
            int count = report.Count;
            double maxedValue = report.MostActive.ValueRelative;
            int counter = 0;
            foreach (var entry in report.Entries)
            {
                EntryLine newLine = new EntryLine(prevEntry, entry, options, maxedValue, count, counter);
                line.Add(newLine);
                prevEntry = newLine;
                counter++;
            }
            Lines = line.ToArray();
        }
    }

    //ДОБАВИТЬ - Удельный вес временного периода
    //Объекты SVG, представляющие запись
    public class EntryRectangle
    {
        public Entry Entry { get; set; }
        public int Index { get; set; }
        public Rect Empty { get; set; } = new Rect();
        public Rect Filled { get; set; } = new Rect();

        public EntryRectangle(int index, Entry entry, int count, double max, DiagramOptions options)
        {

            Index = index;
            Entry = entry;
            double widthWeight = 1;
            int width = Math.Max(1, (int)(widthWeight / count * options.Width));

            int xOffset = index * width;

            double mod = entry.ValueRelative / max;

            int height = (int)options.Height - 40;
            int emptyHeight = (int)(height * (1 - mod));
            int restHeight = height - emptyHeight;

            Empty = new Rect()
            {
                X = xOffset,
                Width = width,
                Y = 0,
                Height = emptyHeight,
            };
            Filled = new Rect()
            {
                X = xOffset,
                Width = width,
                Y = emptyHeight,
                Height = restHeight
            };
        }
    }
    public class EntryLine
    {
        public int Index { get; set; }
        public Entry Entry { get; set; }
        public Point Point { get; set; }
        public Line Line { get; set; }

        public EntryLine(EntryLine prev, Entry entry, DiagramOptions options, double maxedValue, int count, int index)
        {
            Index = index;
            Entry = entry;
            int height = (int)(entry.ValueRelative / maxedValue * ((options.Height - 50)));
            int y = height;

            int prevX = prev == null ? 0 : prev.Point.X;

            double widthWeight = 1;
            int width = Math.Max(1, (int)((double)widthWeight / count * options.Width));
            int x = prevX + width;

            Point = new Point()
            {
                X = x,
                Y = (int)(options.Height - y)
            };

            Line = new Line()
            {
                X1 = prev != null ? prev.Line.X2 : 0,
                Y1 = prev != null ? prev.Line.Y2 : (int)(options.Height - 50),
                X2 = x,
                Y2 = (int)(options.Height - 50 - y)
            };
        }
    }
    
    //Объекты SVG
    public class Rect
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Path => $" L {X} {Y}";
    }
    public class Line
    {
        public int X1 { get; set; }
        public int X2 { get; set; }
        public int Y1 { get; set; }
        public int Y2 { get; set; }
    }
}
