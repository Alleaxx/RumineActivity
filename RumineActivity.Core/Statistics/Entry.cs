using RumineActivity.Core.Measures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{

    //Для записи с 'к моменту (ДАТА) написано сообщений следует придумать нечто другое'

    /// <summary>
    /// Запись 'За период написано сообщений'
    /// </summary>
    public class Entry
    {
        public override string ToString()
        {
            return ToString(MeasureMethods.Total, MeasureUnits.Messages);
        }
        public string ToString(MeasureMethods method, MeasureUnits unit)
        {
            return $"{GetName()} | {GetValue(method, unit)}";
        }

        public string GetName()
        {
            return Range.GetName(Period.EntryDateFunc, false);
        }
        public string GetNameChart()
        {
            return Range.GetName(Period.ChartDateFunc, false);
        }
        public Period Period { get; private set; }
        public DateRange Range { get; private set; }
        public DateTime FromDate => Range.From;
        public DateTime ToDate => Range.To;


        public int Index { get; private set; }
        public Entry? PreviousEntry { get; private set; }


        public Entry(int index, DateRangePostBorders postBorders, ConfigurationReport config, Entry? prev, bool isPeriodical)
        {
            Index = index;
            Range = postBorders.DateRange;
            PostBorders = postBorders;
            IsPeriodical = isPeriodical;
            PostsWritten = postBorders.PostInnerBorders.GetWrittenPosts();
            Period = config.Period;
            PreviousEntry = prev;
        }



        /// <summary>
        /// Является ли запись фактической или периодической
        /// </summary>
        public bool IsPeriodical { get; private set; }
        /// <summary>
        /// Признак того, что граница частично выходит за границы отчета
        /// </summary>
        public bool IsOuterPartial { get; set; }
        public DateRangePostBorders PostBorders { get; private set; }



        #region Получение значения

        private double PostsWritten { get; init; }
        public double FractionMode { get; set; }
        public double PostsWrittenTotal => GetValue(MeasureMethods.Total, MeasureUnits.Messages);
        public double PostsWrittenAverage => GetValue(MeasureMethods.ByDay, MeasureUnits.Messages);
        public double GetValueTotal(MeasureUnits units)
        {
            return GetValue(MeasureMethods.Total, units);
        }
        public double GetValueDayAverage(MeasureUnits units)
        {
            return GetValue(MeasureMethods.ByDay, units);
        }
        public double GetValue(MeasureMethods method, MeasureUnits unit)
        {
            double val = PostsWritten;
            val = MeasureMethod.Create(method).GetValue(val, Range);
            val = new MeasureUnit(unit).GetValue(val);
            return val;
        }
        
        #endregion
    }
}
