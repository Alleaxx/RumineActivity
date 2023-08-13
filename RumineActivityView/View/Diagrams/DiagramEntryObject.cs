using RumineActivity.Core;
using RumineActivity.View.Diagrams;

namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Объект диаграммы, отражающий конкретную запись
    /// </summary>
    public abstract class DiagramEntryObject : DiagramObject
    {
        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int Index { get; protected set; }
        /// <summary>
        /// Запись к которой относится объект
        /// </summary>
        public Entry Entry { get; private set; }


        protected DiagramEntryObject(DiagramChart chart, Entry entry) : base(chart)
        {
            Entry = entry;

            var method = chart.ValuesConfig.MeasureMethod;
            var unit = chart.ValuesConfig.MeasureUnit;

            TitleName = entry.GetNameChart();
            TitleValue = chart.ValuesConfig.FormatEntryPosts(entry);

            ColorMain = chart.ValuesConfig.Rules.GetFor(entry).Color;

            PostsFractionFromTotal = entry.GetValue(method, unit) / chart.GetMaxedValue();
            DateFractionFromTotal = entry.Range.DaysDifference / chart.DaysDifference;
        }

        /// <summary>
        /// Если есть запись, то доля временного промежутка от всего отчета
        /// </summary>
        protected double DateFractionFromTotal { get; private set; }
        /// <summary>
        /// Если есть запись, то доля написанных постов от всего отчета
        /// </summary>
        protected double PostsFractionFromTotal { get; private set; }

        /// <summary>
        /// Расчитать ширину в зависимости от доли временного промежутка и максимально допустимой ширины
        /// </summary>
        protected virtual double GetCountedWidth()
        {
            return DateFractionFromTotal * MaxAllowedWidth;
        }
        /// <summary>
        /// Расчитать ширину в зависимости от значения постов и максимально допустимой высоты
        /// </summary>
        protected virtual double GetCountedHeight()
        {
            return PostsFractionFromTotal * MaxAllowedHeight;
        }

    }
}
