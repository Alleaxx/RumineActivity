using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Настройки диаграммы
    public class DiagramSize
    {
        public double Width { get; private set; } = 1000;

        //Высота графической части
        public double HeightChart { get; private set; } = 450;
        //Высота подписей
        public double HeightWriting { get; private set; } = 40;
        //Общая высота диаграммы
        public double Height => HeightChart + HeightWriting;
    }

    public abstract class DiagramChart
    {
        public StatisticsReport Report { get; private set; }
        public DiagramSize Options { get; private set; }
        public double MaxValueMod { get; private set; } = 1.05;


        public DiagramChart(StatisticsReport report, DiagramSize options)
        {
            Report = report;
            Options = options;

            if (!Report.IsEmpty)
            {
                CreateChart();
            }
        }
        protected abstract void CreateChart();
    }

}
