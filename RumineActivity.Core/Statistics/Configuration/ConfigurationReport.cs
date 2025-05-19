using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    /// <summary>
    /// Конфигурация создания отчета
    /// </summary>
    public class ConfigurationReport
    {
        public DateRange DateRange
        {
            get => dateRange;
            set
            {
                dateRange = value;
                UpdatePrevNextRanges();
            }
        }
        private DateRange dateRange;
        public DateRange DateRangePrevious { get; private set; }
        public DateRange DateRangeNext { get; private set; }

        public Period Period
        {
            get => period;
            set
            {
                period = value;
                UpdatePrevNextRanges();
            }
        }
        private Period period;

        /// <summary>
        /// Создать стандартную конфигурацию отчета (вся история Румине, периодичность месяц)
        /// </summary>
        public ConfigurationReport()
        {
            DateRange = new DateRange(RumineValues.FoundationDate, RumineValues.EndDate);
            Period = Period.Create(Periods.Month);
        }
        
        /// <summary>
        /// Скопировать значения переданной конфигурации отчета
        /// </summary>
        public ConfigurationReport(ConfigurationReport configuration)
        {
            DateRange = new DateRange(configuration.DateRange.From, configuration.DateRange.To);
            Period = Period.Create(configuration.Period.Type);
        }

        private void UpdatePrevNextRanges()
        {
            if (DateRange.IsFullYearPeriod().HasValue)
            {
                int years = DateRange.IsFullYearPeriod().Value;
                DateRangeNext = DateRange.AddYearsGetNewRange(years);
                DateRangePrevious = DateRange.AddYearsGetNewRange(-years);
            }
            else if (DateRange.IsFullMonthPeriod().HasValue)
            {
                int months = DateRange.IsFullMonthPeriod().Value;
                DateRangeNext = DateRange.AddMonthsGetNewRange(months);
                DateRangePrevious = DateRange.AddMonthsGetNewRange(-months);
            }
            else if (DateRange.IsFullDayPeriod().HasValue)
            {
                int days = DateRange.IsFullDayPeriod().Value;
                DateRangeNext = DateRange.AddDaysGetNewRange(days);
                DateRangePrevious = DateRange.AddDaysGetNewRange(-days);
            }
            else
            {
                DateRangeNext = new DateRange(DateRange.To, DateRange.To + DateRange.Diff);
                DateRangePrevious = new DateRange(DateRange.From - DateRange.Diff, DateRange.From);
            }
        }

        public ConfigurationReport CopyThisWithNewRange(DateRange range)
        {
            var newConfig = new ConfigurationReport(this);
            newConfig.DateRange = range;
            return newConfig;
        }
        public ConfigurationReport CopyThisWithPeriod(Period period)
        {
            var newConfig = new ConfigurationReport(this);
            newConfig.Period = period;
            return newConfig;
        }

        public string GetReportName()
        {
            return $"{Period.NameReport} статистика за {DateRange.GetName()}";
        }

        public string GetLinkHref()
        {
            string dateFrom = dateRange.From.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            string dateTo = dateRange.To.ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            return $"report?From={dateFrom}&To={dateTo}&Period={Period?.Type}";
        }

        /// <summary>
        /// Сравнить фактические значения двух отчётов
        /// </summary>
        public bool CompareEqual(ConfigurationReport other)
        {
            bool isRangeSame = DateRange.From == other.DateRange.From && DateRange.To == other.DateRange.To;
            bool isPeriodSame = Period?.Type == other.Period?.Type && Period?.TimeInterval == other.Period?.TimeInterval;

            return isRangeSame && isPeriodSame;
        }

        /// <summary>
        /// Получить список сообщений на анализ согласно настройкам отчета
        /// </summary>
        public Post[] GetPostsSource(IForum source)
        {
            return source.Posts
                .OrderBy(d => d.Date)
                .ToArray();
        }

        /// <summary>
        /// Признак корректного сочетания периодичности для выбранного интервала
        /// </summary>
        public bool IsCorrect()
        {
            return DateRange.IsOkWithPeriod(Period);
        }
    }
}
