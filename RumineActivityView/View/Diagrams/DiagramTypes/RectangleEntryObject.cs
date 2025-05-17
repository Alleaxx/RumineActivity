using RumineActivity.Core;
using RumineActivity.View.Diagrams.SVG;

namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Объект для гистограммы
    /// </summary>
    public class RectangleEntryObject : DiagramEntryObject
    {
        /// <summary>
        /// "Пустой" прямоугольник записи
        /// </summary>
        public Rect Empty { get; private set; } = new Rect();
        /// <summary>
        /// "Заполненный" прямоугольник записи
        /// </summary>
        public Rect Filled { get; private set; } = new Rect();

        public Point CenterAbove { get; set; } = new Point();

        public string ValueText { get; set; }
        public bool ShowValueAbove { get; set; }
        public bool IsFirstGroup { get; set; }
        public bool IsLastGroup { get; set; }

        /// <summary>
        /// Создать прямоугольник для записи
        /// </summary>
        public static RectangleEntryObject CreateEntryRectangle(DiagramChart chart, RectangleEntryObject prev, Entry entry)
        {
            return new RectangleEntryObject(chart, prev, entry);
        }
        private RectangleEntryObject(DiagramChart chart, RectangleEntryObject prev, Entry entry) : base(chart, entry)
        {
            //чтобы не задевать границы по горизонтали вплотную
            MaxAllowedWidth -= 5;


            bool isFirst = prev == null;
            double x;
            if (!isFirst)
            {
                Index = prev.Index + 1;
                x = prev.Filled.X + prev.Filled.Width;
            }
            else
            {
                var config = chart.DiagramConfig;
                Index = 0;
                x = config.WidthLegend;
            }

            var width = GetCountedWidth();
            var (filled, empty) = GetCountedDoubleHeight();

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

            ValueText = chart.ValuesConfig.FormatEntryPosts(entry);
            ShowValueAbove = width > ValueText.Length * 6.2;
            CenterAbove = new Point()
            {
                X = x + width / 2,
                Y = empty - 5
            };

        }

        /// <summary>
        /// Получить высоты для заполненного и пустого прямоугольника
        /// </summary>
        protected (double filled, double empty) GetCountedDoubleHeight()
        {
            double fillHeight = GetCountedHeight();
            double emptyHeight = MaxAllowedHeight - fillHeight;
            return (fillHeight, emptyHeight);
        }

        public static explicit operator Entry(RectangleEntryObject rect)
        {
            return rect.Entry;
        }
    }
}
