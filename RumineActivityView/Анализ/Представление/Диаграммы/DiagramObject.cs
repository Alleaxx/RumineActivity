using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{

    public abstract class DiagramObject
    {
        public readonly int Index;
        public Entry Entry { get; private set; }

        protected DiagramObject()
        {
            Index = 0;
        }
        protected DiagramObject(DiagramObject prev, Entry entry, DiagramChart chart)
        {
            Index = prev.Index + 1;
            Entry = entry;

            MaxAllowedHeight = (int)chart.Size.HeightChart;
            MaxAllowedWidth = (int)chart.Size.Width;

            PostsMaxMod = entry.PostsAverage / chart.MaxedValue;
            DateMod = entry.Range.DaysDifference / chart.DaysDifference;
        }


        protected int MaxAllowedHeight { get; private set; }
        protected int MaxAllowedWidth { get; private set; }
        protected double DateMod { get; private set; }
        protected double PostsMaxMod { get; private set; }
        protected virtual int CountWidth()
        {
            return Math.Max(1, (int)(DateMod * MaxAllowedWidth));
        }
        protected virtual int CountHeight()
        {
            return (int)(PostsMaxMod * MaxAllowedHeight);
        }
    }
}
