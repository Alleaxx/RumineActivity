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
            EntryRectangle prevEntry = new EntryRectangle();
            List<EntryRectangle> rects = new List<EntryRectangle>();
            for (int i = 0; i < Report.Length; i++)
            {
                Entry entry = Report.Entries[i];
                EntryRectangle newRect = new EntryRectangle(prevEntry, entry, this);
                rects.Add(newRect);
                prevEntry = newRect;
            }
            Rectangles = rects.ToArray();
        }
    }

    public class EntryRectangle : DiagramObject
    {
        public Rect Empty { get; private set; } = new Rect();
        public Rect Filled { get; private set; } = new Rect();

        public EntryRectangle()
        {
            Filled = new Rect()
            {
                X = 0,
                Width = 0
            };
        }
        public EntryRectangle(EntryRectangle prev, Entry entry, DiagramChart chart) : base(prev, entry, chart)
        {
            int x = prev.Filled.X + prev.Filled.Width;

            int width = CountWidth();
            var (filled, empty) = CountDoubleHeight();

            Empty = new Rect()
            {
                X = x,
                Width = width,
                Y = 0,
                Height = empty,
            };
            Filled = new Rect()
            {
                X = x,
                Width = width,
                Y = empty,
                Height = filled
            };
        }


        protected (int filled, int empty) CountDoubleHeight()
        {
            int fillHeight = CountHeight();
            int emptyHeight = MaxAllowedHeight - fillHeight;
            return (fillHeight, emptyHeight);
        }
    }

}
