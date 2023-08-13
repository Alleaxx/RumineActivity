using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;

namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Настройки отображения диаграммы
    /// </summary>
    public class DiagramConfig
    {
        //2.24 оптимальное соотношение ширины и высоты

        /// <summary>
        /// Общая ширина диаграммы
        /// </summary>
        public double Width => WidthChart + WidthLegend;
        /// <summary>
        /// Ширина графической чести
        /// </summary>
        public double WidthChart { get; set; }
        /// <summary>
        /// Ширина подписей легенды
        /// </summary>
        public double WidthLegend { get; set; }

        /// <summary>
        /// Общая высота диаграммы
        /// </summary>
        public double Height => HeightChart + HeightWriting;
        /// <summary>
        /// Высота графической части
        /// </summary>
        public double HeightChart { get; set; }
        /// <summary>
        /// Высота подписей
        /// </summary>
        public double HeightWriting { get; set; }

        /// <summary>
        /// Количество боковых подсказок-легенд
        /// </summary>
        public int LegendItemsCount { get; set; }
             
        public DiagramConfig()
        {
            WidthChart = 1070;
            WidthLegend = 50;
            HeightChart = 460;
            HeightWriting = 40;
            MaxAllowedEntries = 18;
            LegendItemsCount = 10;
        }

        /// <summary>
        /// Максимально допустимое количество подписей (больше не помещается).
        /// Если периодов больше, то записи будут пытаться группироваться по дате (если настроена сортировка).
        /// Если группировка не сработала, то они будут показываться через раз / два / три и т.д.
        /// </summary>
        public int MaxAllowedEntries { get; set; }

        /// <summary>
        /// Проверка, нужно ли отображать легенду для указанной записи
        /// </summary>
        [Legacy]
        public bool CheckShowBottomTitle(DiagramEntryObject obj, int length)
        {
            int entryIndex = obj.Index;
            //13 - максимум, при котором отображается каждая запись
            int entriesCount = length;
            int maxAllowedEntries = MaxAllowedEntries;
            double everyEntryN = Math.Ceiling((double)entriesCount / maxAllowedEntries);

            return (entriesCount < maxAllowedEntries) || ((entryIndex % everyEntryN) < 1);
        }
    }
}
