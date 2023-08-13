using System;
using System.Collections.Generic;
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
        /// <summary>
        /// Соотношение максимально допустимого количества показываемых записей и имеющихся по факту
        /// </summary>
        private double AllowedEntriesMode { get; init; }
        /// <summary>
        /// Получить максимальное значение для диаграммы (абсолютно или относительно в зависимости от настроек)
        /// </summary>
        public virtual double GetMaxedValue()
        {
            if (ValuesConfig.UseRelativeValues)
            {
                if(ValuesConfig.MeasureMethod.Type == MeasureMethods.Total)
                {
                    return Report.MostActiveTotal.GetValue(ValuesConfig.MeasureMethod, ValuesConfig.MeasureUnit) * MaxedValueMod;
                }
                else
                {
                    return Report.MostActiveAverage.GetValue(ValuesConfig.MeasureMethod, ValuesConfig.MeasureUnit) * MaxedValueMod;
                }
            }
            else
            {
                var method = ValuesConfig.MeasureMethod.Type;
                var period = Report.Period.Type;
                var unit = ValuesConfig.MeasureUnit.Type;
                double maximumMessagesPerDay = ValuesConfig.AbsoluteValue;


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
                    else if (period == Periods.Season)
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

                var relativeVal = Report.MostActiveAverage.GetValue(ValuesConfig.MeasureMethod, ValuesConfig.MeasureUnit) * MaxedValueMod;
                return maximumMessagesPerDay > relativeVal ? maximumMessagesPerDay : relativeVal;
            }
        }

        public DiagramChart(ValuesViewConfig valuesConfig,StatisticsReport report, DiagramConfig size)
        {
            ValuesConfig = valuesConfig;
            Report = report;
            DiagramConfig = size;
            MaxedValueMod = 1.05;
            AllowedEntriesMode = (double)report.Length / size.MaxAllowedEntries;

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
                var newLine = new LineObject(this, legendItem.mode, legendItem.value, Report.Period);
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
            var onePart = maxed / legendItemsCount;
            List<(double value, double mode)> values = new List<(double value, double mode)>();
            for (int i = 1; i <= legendItemsCount; i++)
            {
                double usedI = i == legendItemsCount ? i - 1 + 0.5 : i;

                var fraction = usedI / legendItemsCount;
                var valBeforeProcessing = onePart * usedI;
                var valAfterProcessing = StatExtensions.FindNearestMultiFive(valBeforeProcessing, 1);

                if(!values.Any(v => v.value == valAfterProcessing))
                {
                    values.Add((valAfterProcessing, valAfterProcessing / maxed));
                }
                //values.Add((valBeforeProcessing, valBeforeProcessing / maxed));
            }
            return values.ToArray();
        }

        /// <summary>
        /// Создает метки легенды привязанные к конкретным объектам диаграммы
        /// </summary>
        protected virtual void FillLegendLabels(IEnumerable<DiagramEntryObject> diagramEntries)
        {
            var func = GetEntriesDateGroupingFunc(Report.Period, ValuesConfig.SortingEntriesSelected.Key);
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
        private Func<IEnumerable<DiagramEntryObject>, IEnumerable<IGrouping<string, DiagramEntryObject>>> GetEntriesDateGroupingFunc(Period periodObj, Sortings sort)
        {
            var period = periodObj.Type;
            var everyEntryN = AllowedEntriesMode;

            Func<Entry, string> test = entry => entry.GetNameChart();

            //стандартная функция сохраняющая оригинальное имя
            Func<IEnumerable<DiagramEntryObject>, IEnumerable<IGrouping<string, DiagramEntryObject>>>
                defaultFunc = entries => entries.GroupBy(e => e.Entry.GetNameChart());

            //10-19 дек 2012
            Func<IEnumerable<DiagramEntryObject>, IEnumerable<IGrouping<string, DiagramEntryObject>>>
                dayMonthPartFunc = entries => entries.GroupBy(e => $"{DateExtensions.DefineDayMonthPartString(e.Entry.FromDate)} {e.Entry.FromDate.Month.GetMonthName("MMM")} {e.Entry.FromDate:yyyy}");

            //дек 2012
            Func<IEnumerable<DiagramEntryObject>, IEnumerable<IGrouping<string, DiagramEntryObject>>>
                shortMonthYearFunc = entries => entries.GroupBy(e => $"{e.Entry.FromDate.Month.GetMonthName("MMM")} {e.Entry.FromDate:yyyy}");

            //IV кв. 2012
            Func<IEnumerable<DiagramEntryObject>, IEnumerable<IGrouping<string, DiagramEntryObject>>>
                seasonFunc = entries => entries.GroupBy(e => $"{DateExtensions.DefineSeasonSymbol(e.Entry.FromDate)} кв. {e.Entry.FromDate:yyyy}");

            //1-е п. 2012
            Func<IEnumerable<DiagramEntryObject>, IEnumerable<IGrouping<string, DiagramEntryObject>>>
                halfYearFunc = entries => entries.GroupBy(e => $"{DateExtensions.DefineHalfYear(e.Entry.FromDate)}-е п. {e.Entry.FromDate:yyyy}");

            //2013 год
            Func<IEnumerable<DiagramEntryObject>, IEnumerable<IGrouping<string, DiagramEntryObject>>>
                yearFunc = entries => entries.GroupBy(e => e.Entry.FromDate.ToString("yyyy год"));

            var groupingFunc = defaultFunc;

            //группировка по дате бессмысленна, если сортировка стоит не по дате
            if(sort != Sortings.Index)
            {
                return groupingFunc;
            }

            //От 18
            if (everyEntryN > 1)
            {
                if (period == Periods.Day)
                {
                    groupingFunc = dayMonthPartFunc;
                }
                else if (period == Periods.Week)
                {
                    groupingFunc = shortMonthYearFunc;
                }
                else if (period == Periods.Month)
                {
                    groupingFunc = seasonFunc;
                }
                else if (period == Periods.Season)
                {
                    groupingFunc = halfYearFunc;
                }
            }
            //От 27
            if (everyEntryN >= 1.5)
            {
                if (period == Periods.Week)
                {
                    groupingFunc = shortMonthYearFunc;
                }
                else if (period == Periods.Month)
                {
                    groupingFunc = halfYearFunc;
                }
                else if (period == Periods.Season)
                {
                    groupingFunc = yearFunc;
                }
            }
            //От 45
            if (everyEntryN >= 2.5)
            {
                if (period == Periods.Week)
                {
                    groupingFunc = shortMonthYearFunc;
                }
                else if (period == Periods.Month)
                {
                    groupingFunc = halfYearFunc;
                }
            }
            //От 54
            if (everyEntryN >= 3)
            {
                if (period == Periods.Week)
                {
                    groupingFunc = shortMonthYearFunc;
                }
            }
            //От 100
            if (everyEntryN >= 5.5)
            {
                if (period == Periods.Day)
                {
                    groupingFunc = shortMonthYearFunc;
                }
                else if (period == Periods.Week)
                {
                    groupingFunc = seasonFunc;
                }
                else if (period == Periods.Month)
                {
                    groupingFunc = yearFunc;
                }
            }

            return groupingFunc;
        }
        
        #endregion
    }
}
