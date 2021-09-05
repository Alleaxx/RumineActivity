using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class DataEntry
    {
        public int Index { get; set; }
        public DiagramOptions Options { get; set; }
        public Entry Entry { get; set; }

        public double ModAbsoulete { get; set; }
        public double ModRelative { get; set; }

        public double Width => ModRelative * Options.Width;
        public double Height => ModRelative * Options.Height;
        public double R => ModRelative * Options.R;


        //Параметры вертикальной гистограммы
        //<rect x = "0" y="0" height="50" width="10" fill="transparent" stroke="transparent"/>
        //<rect x = "0" y="40" height="10" width="10" fill="green" stroke="blue" />

        //<rect x = "10" y="0" height="50" width="10" fill="transparent" stroke="transparent"/>
        //<rect x = "10" y="30" height="20" width="10" fill="green" stroke="blue" />

        //<rect x = "10" y="0" height="50" width="10" fill="transparent" stroke="transparent"/>
        //<rect x = "10" y="20" height="30" width="10" fill="green" stroke="blue" />

        //<rect x = "20" y="0" height="50" width="10" fill="transparent" stroke="transparent"/>
        //<rect x = "20" y="10" height="40" width="10" fill="green" stroke="blue" />


        public double x => Index;


        public DataEntry(int index, Entry entry, StatisticsReport report, DiagramOptions options)
        {
            Index = index;
            Options = options;
            double sumTotal = report.Entries.Sum(e => e.Value);
            ModRelative = entry.Value / sumTotal;

            double sumWeight = report.Entries.Sum(e => e.ValueRelative);
            ModAbsoulete = entry.ValueRelative / sumWeight;  
        }
    }
    public class DiagramOptions
    {
        public double Width { get; set; } = 500;
        public double Height { get; set; } = 500;
        public double R { get; set; } = 50;

        public string Color { get; set; } = "blue";
    }
}
