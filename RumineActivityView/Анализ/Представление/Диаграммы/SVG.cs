using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
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
