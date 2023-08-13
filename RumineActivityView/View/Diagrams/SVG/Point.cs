namespace RumineActivity.View.Diagrams.SVG
{
    /// <summary>
    /// Точка
    /// </summary>
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Path => $" L {X} {Y}";
        public string PathAttr => $" L {X.ToString().Replace(',','.')} {Y.ToString().Replace(',', '.')}";
    }
}
