using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Объект диаграммы (столбец гистограммы / линия)
    /// </summary>
    public abstract class DiagramObject
    {
        /// <summary>
        /// Подсказка с названием объекта
        /// </summary>
        public string TitleName { get; protected set; }
        /// <summary>
        /// Подсказка со значением объекта
        /// </summary>
        public string TitleValue { get; protected set; }
        /// <summary>
        /// Объединенная подсказка
        /// </summary>
        public string TitleCombined => $"{TitleName} | {TitleValue}";

        /// <summary>
        /// Главный цвет
        /// </summary>
        public string ColorMain { get; protected set; }

        protected DiagramObject(DiagramChart chart)
        {
            MaxAllowedHeight = chart.DiagramConfig.HeightChart;
            MaxAllowedWidth = chart.DiagramConfig.WidthChart;
        }

        /// <summary>
        /// Максимально допустимая высота. Берется из настроек диаграммы
        /// </summary>
        public double MaxAllowedHeight { get; protected set; }
        /// <summary>
        /// Максимально допустимая ширина. Берется из настроек диаграммы + может дополнительно модифицироваться по необходимости
        /// </summary>
        public double MaxAllowedWidth { get; protected set; }
    }
}
