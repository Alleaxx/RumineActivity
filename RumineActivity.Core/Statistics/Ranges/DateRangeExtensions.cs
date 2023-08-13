using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public static class DateRangeExtensions
    {
        /// <summary>
        /// Максимальное оптимальное количество записей статистики для периода (любого)
        /// </summary>
        public const int ReportEntriesMaxAmount = 200;
        public const int ReportEntriesMinAmount = 3;


        /// <summary>
        /// Получить количество периодов в указанном временном интервале
        /// </summary>
        public static double GetEntriesCapacityForRange(this DateRange range, Period period)
        {
            var value = range.Diff / period.TimeInterval;

            return value;
        }

        /// <summary>
        /// Оптимален ли для данного интервала указанный период
        /// </summary>
        public static bool IsOkWithPeriod(this DateRange range, Period period)
        {
            var amount = GetEntriesCapacityForRange(range, period);

            return amount <= ReportEntriesMaxAmount && amount >= ReportEntriesMinAmount;
        }

        /// <summary>
        /// Временной интервал полностью в границах истории Румине
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public static bool IsInsideRumineBorders(this DateRange range)
        {
            DateTime minDate = RumineValues.FoundationDate;
            DateTime maxDate = RumineValues.EndDate;

            return range.From >= minDate && (range.To <= maxDate || range.To <= maxDate.AddDays(1));
        }


        /// <summary>
        /// Признак что временной интервал обозначает любой полный период (один или несколько полных лет, месяцев или дней)
        /// </summary>
        public static bool IsAnyPeriod(this DateRange range)
        {
            return range.IsFullYearPeriod().HasValue || range.IsFullMonthPeriod().HasValue || range.IsFullDayPeriod().HasValue || range.IsRuminePeriod();
        }
        public static int? IsFullYearPeriod(this DateRange range)
        {
            for (int i = 1; i < 20; i++)
            {
                if(range.From.AddYears(i) == range.To && range.From.Month == 1 && range.From.Day == 1)
                {
                    return i;
                }
            }
            return null;
        }
        public static int? IsFullMonthPeriod(this DateRange range)
        {
            for (int i = 1; i < 120; i++)
            {
                if (range.From.AddMonths(i) == range.To && range.From.Day == 1)
                {
                    return i;
                }
            }
            return null;
        }
        public static int? IsFullDayPeriod(this DateRange range)
        {
            for (int i = 1; i < 33; i++)
            {
                if (range.From.AddDays(i) == range.To)
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        /// Признак общих временных рамок текущей истории Румине
        /// </summary>
        public static bool IsRuminePeriod(this DateRange range)
        {
            return range.From == RumineValues.FoundationDate && (range.To == RumineValues.EndDate || range.To.AddDays(-1) == RumineValues.EndDate);
        }
    }
}
