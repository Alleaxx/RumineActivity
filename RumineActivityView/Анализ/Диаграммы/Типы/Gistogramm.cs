using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Гистограмма, прямоугольники
    public class Gistogramm : DiagramChart
    {
        public EntryRectangle[] Rectangles { get; private set; }

        public Gistogramm(StatisticsReport report, DiagramSize options) : base(report, options)
        {

        }

        protected override void CreateChart()
        {
            double max = Report.MostActive.PostsRelative * MaxValueMod;

            EntryRectangle prevEntry = null;
            List<EntryRectangle> rects = new List<EntryRectangle>();
            for (int i = 0; i < Report.Entries.Length; i++)
            {
                var entry = Report.Entries[i];
                EntryRectangle newRect = new EntryRectangle(prevEntry, i, entry, max, Options, Report.FirstLastPost);
                rects.Add(newRect);
                prevEntry = newRect;
            }
            Rectangles = rects.ToArray();
        }
    }
    public class EntryRectangle
    {
        public Entry Entry { get; private set; }
        public int Index { get; private set; }
        public Rect Empty { get; private set; } = new Rect();
        public Rect Filled { get; private set; } = new Rect();

        public EntryRectangle(EntryRectangle prev, int index, Entry entry, double max, DiagramSize options, DateRange range)
        {
            Index = index;
            Entry = entry;

            //Ширина
            double widthDays = entry.Range.Diff.TotalDays;
            double allDays = range.Diff.TotalDays;
            int width = Math.Max(1, (int)(widthDays / allDays * options.Width));

            //Отступ от левого края
            int x = prev == null ? 0 : prev.Filled.X + prev.Filled.Width;

            //Высота
            double mod = entry.PostsRelative / max;
            int height = (int)options.HeightChart;
            int fillHeight = (int)(height * mod);
            int emptyHeight = height - fillHeight;


            Empty = new Rect()
            {
                X = x,
                Width = width,
                Y = 0,
                Height = emptyHeight,
            };
            Filled = new Rect()
            {
                X = x,
                Width = width,
                Y = emptyHeight,
                Height = fillHeight
            };
        }
    }

}
