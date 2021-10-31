using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    //Настройки диаграммы
    public class DiagramSize
    {
        public double Width { get; private set; }

        //Высота графической части
        public double HeightChart { get; private set; }
        //Высота подписей
        public double HeightWriting { get; private set; }
        //Общая высота диаграммы
        public double Height => HeightChart + HeightWriting;

        public DiagramSize()
        {
            Width = 1150;
            HeightChart = 460;
            HeightWriting = 40;
        }
    }


    //ПОДПИСИ к диаграммам через класс
    public abstract class DiagramChart
    {
        protected StatisticsReport Report { get; private set; }
        public readonly double DaysDifference;
        public readonly double MaxedValue;
        private double MaxedValueMod { get; set; }


        public DiagramSize Size { get; private set; }


        public DiagramChart(StatisticsReport report, DiagramSize size)
        {
            Report = report;
            Size = size;
            MaxedValueMod = 1.05;

            if (!Report.IsEmpty)
            {
                DaysDifference = Report.DateRangePosts.DaysDifference;
                MaxedValue = Report.MostActive.PostsAverage * MaxedValueMod;
                CreateChart();
            }
        }
        protected abstract void CreateChart();
    }
}
