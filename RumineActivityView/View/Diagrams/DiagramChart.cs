using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
using RumineActivity.Core.Measures;
using RumineActivity.View.Diagrams.SVG;

namespace RumineActivity.View.Diagrams
{
    /// <summary>
    /// Диаграмма
    /// </summary>
    public abstract class DiagramChart
    {
        public ValuesViewConfig ValuesConfig { get; init; }
        public DiagramConfig DiagramConfig { get; init; }

        protected StatisticsReport Report { get; init; }
        public double DaysDifference => Report.DateRangePosts.DaysDifference;
        
        
        /// <summary>
        /// Модификатор "верхнего" потолка диаграммы
        /// </summary>
        private double MaxedValueMod { get; init; }

        protected string OwnLegendFormat { get; set; }

        /// <summary>
        /// Получить максимальное значение для диаграммы (абсолютно или относительно в зависимости от настроек)
        /// </summary>
        public virtual double GetMaxedValue()
        {
            if (ValuesConfig.MaxValue.Type == MaxValues.Relative)
            {
                return GetMaxedRelativeValue();
            }
            else
            {
                var method = ValuesConfig.MeasureMethod.Type;
                var period = Report.Period.Type;
                var unit = ValuesConfig.MeasureUnit.Type;
                double maximumMessagesPerDay = ValuesConfig.MaxValue.Value * 20;


                if (method == MeasureMethods.ByHour)
                {
                    maximumMessagesPerDay /= 24;
                }
                else if (method == MeasureMethods.ByMonth)
                {
                    maximumMessagesPerDay *= 30;
                }
                else if (method == MeasureMethods.Total)
                {
                    if (period == Periods.Week)
                    {
                        maximumMessagesPerDay *= 7;
                    }
                    else if (period == Periods.Month)
                    {
                        maximumMessagesPerDay *= 30;
                    }
                    else if (period == Periods.Quarter)
                    {
                        maximumMessagesPerDay *= 90;
                    }
                    else if (period == Periods.Year)
                    {
                        maximumMessagesPerDay *= 365;
                    }
                }

                if (unit == MeasureUnits.Pages)
                {
                    maximumMessagesPerDay /= 20;
                }
                else if (unit == MeasureUnits.OldPages)
                {
                    maximumMessagesPerDay /= 10;
                }

                var relativeVal = GetMaxedRelativeValue();
                return maximumMessagesPerDay > relativeVal ? maximumMessagesPerDay : relativeVal;
            }
        }
        private double GetMaxedRelativeValue()
        {
            if (ValuesConfig.MeasureMethod.Type == MeasureMethods.Total)
            {
                return Report.MostActiveTotal?.GetValue(ValuesConfig.MeasureMethod, ValuesConfig.MeasureUnit) * MaxedValueMod ?? 0;
            }
            else
            {
                return Report.MostActiveAverage?.GetValue(ValuesConfig.MeasureMethod, ValuesConfig.MeasureUnit) * MaxedValueMod ?? 0;
            }
        }


        public DiagramChart(ValuesViewConfig valuesConfig,StatisticsReport report, DiagramConfig size)
        {
            ValuesConfig = valuesConfig;
            Report = report;
            DiagramConfig = size;
            MaxedValueMod = 1.05;

            if (!Report.IsEmpty)
            {
                CreateChart();
            }
        }

        /// <summary>
        /// Создаёт контент диаграммы
        /// </summary>
        protected abstract void CreateChart();

        #region Легенда

        /// <summary>
        /// Метки легенды по вертикали
        /// </summary>
        public LineObject[] LegendLines { get; protected set; }
        /// <summary>
        /// Метки легенды по горизонтали
        /// </summary>
        public List<DiagramLegendObject> LegendLabels { get; private set; }

        /// <summary>
        /// Создать линии легенды для значений по количеству из конфигурации
        /// </summary>
        protected void FillLegendLines()
        {
            var linesLegend = new List<LineObject>();
            var legendValues = GetLegendValues();
            foreach (var legendItem in legendValues)
            {
                var newLine = new LineObject(this, legendItem.mode, legendItem.value, Report.Period, OwnLegendFormat);
                linesLegend.Add(newLine);
            }
            LegendLines = linesLegend.ToArray();
        }
        
        /// <summary>
        /// Получить конкретные значения для отображения легенды и их соотношение к максимальному значению
        /// </summary>
        private IEnumerable<(double value, double mode)> GetLegendValues()
        {
            int legendItemsCount = DiagramConfig.LegendItemsCount;
            var maxed = GetMaxedValue();

            var legendValueStepBase = maxed / legendItemsCount;
            var legendValueStep = GetLegendStepValue(legendValueStepBase);

            List<(double value, double mode)> values = new List<(double value, double mode)>();

            int i = 1;
            while (true)
            {
                var newLegendValue = legendValueStep * i;

                if (!values.Any(v => v.value == newLegendValue))
                {
                    values.Add((newLegendValue, newLegendValue / maxed));
                }

                if(newLegendValue >= maxed * 0.95)
                {
                    break;
                }
                i++;
            }
            return values.ToArray();
        }
        /// <summary>
        /// Рассчитать шаг значения легенды. Он должен быть производным от чисел 1; 2; 2,5; 5
        /// </summary>
        private double GetLegendStepValue(double originalStep)
        {
            int counter = originalStep > 1
                ? StatExtensions.GetTens(originalStep)
                : StatExtensions.GetZeros(originalStep);

            double[] baseNumbers = new double[]
            {
                1.0,
                2.0,
                2.5,
                5.0
            };
            List<double> stepValues = new List<double>();
            foreach (double value in baseNumbers)
            {
                if(originalStep > 1)
                {
                    stepValues.Add(value * Math.Pow(10, counter - 1 - 1));
                    stepValues.Add(value * Math.Pow(10, counter - 1));
                    stepValues.Add(value * Math.Pow(10, counter - 1 + 1));
                }
                else
                {
                    stepValues.Add(value / Math.Pow(10, counter - 1));
                    stepValues.Add(value / Math.Pow(10, counter));
                    stepValues.Add(value / Math.Pow(10, counter + 1));
                }
            }

            double newStep = stepValues[0];
            foreach (var step in stepValues)
            {
                var currentdiff = Math.Abs(originalStep - newStep);
                var newDiff = Math.Abs(originalStep - step);
                if (newDiff < currentdiff)
                {
                    newStep = step;
                }
            }
            return newStep;
        }



        /// <summary>
        /// Создает метки легенды привязанные к конкретным объектам диаграммы
        /// </summary>
        protected virtual void FillLegendLabels(IEnumerable<DiagramEntryObject> diagramEntries)
        {
            var func = GetEntriesDateGroupingFunc(diagramEntries, ValuesConfig.SortingEntriesSelected.Key).GroupingFunc;
            var entriesGrouped = func.Invoke(diagramEntries);
            
            List<DiagramLegendObject> legendObjects = new List<DiagramLegendObject>();
            DiagramLegendObject prev = null;
            foreach (var legend in entriesGrouped.OrderBy(g => g.First().Entry.FromDate))
            {
                var newLegendItem = new DiagramLegendObject(this, Report, legend, prev);
                legendObjects.Add(newLegendItem);
                newLegendItem.HideTitlesIfSmall(prev);
                prev = newLegendItem;
            }
            if(legendObjects.Count > 2)
            {
                legendObjects[0].HideTitlesIfSmall(legendObjects[1]);
            }
            LegendLabels = legendObjects;
        }

        /// <summary>
        /// Скрывает некоторые метки легенды, чтобы их отображаемое количество соответствовало максимально доступному
        /// </summary>
        protected void ReduceLegendLabels()
        {
            int entriesCount = LegendLabels.Count();
            int maxAllowedEntries = DiagramConfig.MaxAllowedEntries;
            double everyEntryN = (double)entriesCount / maxAllowedEntries;
            bool hideBorders = everyEntryN >= 1.5;

            if(maxAllowedEntries == entriesCount)
            {
                return;
            }

            var ordered = LegendLabels.OrderBy(l => l.Center.X);
            int sinceLast = 0;
            for (int i = 0; i < LegendLabels.Count; i++)
            {
                var ele = ordered.ElementAt(i);
                if (sinceLast >= Math.Floor(everyEntryN))
                {
                    sinceLast = 0;
                }
                else
                {
                    ele.HideTitles();
                    sinceLast++;
                }
                if (hideBorders)
                {
                    ele.HideBorders();
                }
            }
        }
        
        /// <summary>
        /// Возвращает функцию группировки записей по периодам, чтобы все подписи помещались в имеющееся пространство
        /// </summary>
        private EntryDateGrouping GetEntriesDateGroupingFunc(IEnumerable<DiagramEntryObject> diagramEntries, Sortings sort)
        {
            if(sort != Sortings.Index)
            {
                return EntryDateGrouping.DefaultGrouping;
            }

            var allDateGroupings = new EntryDateGrouping[]
            {
                EntryDateGrouping.YearGrouping,
                EntryDateGrouping.QuartedGrouping,
                EntryDateGrouping.SeasonGrouping,
                EntryDateGrouping.MonthShortGrouping,
                EntryDateGrouping.MonthFullGrouping,
                EntryDateGrouping.DayMonthGrouping,
                EntryDateGrouping.DayGrouping,
            };

            var allowedEntries = allDateGroupings
                .Where(g => g.CheckEntries(diagramEntries))
                .OrderByDescending(g => g.Level)
                .ToArray();

            if (allowedEntries.Length == 0)
            {
                return EntryDateGrouping.DefaultGrouping;
            }

            return allowedEntries.Last();
        }
        
        #endregion
    }
}
