using RumineActivity.Core.Models;
using RumineActivity.Core.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    /// <summary>
    /// Временные рамки (с даты по дату)
    /// </summary>
    public class DateRange
    {
        #region Формирование имени

        public override string ToString()
        {
            return this.GetName();
        }
        public string GetName(Func<DateRange, string> func, bool preferOwnName = true)
        {
            if (!preferOwnName)
            {
                return func.Invoke(this);
            }
            else
            {
                return GetName() ?? func.Invoke(this);
            }
        }
        public string GetName()
        {
            string specialName = $"{From:dd.MM.yyyy} - {To:dd.MM.yyyy}";

            if (this.IsFullYearPeriod().HasValue)
            {
                var value = this.IsFullYearPeriod().Value;
                if (value == 1)
                {
                    specialName = $"{From.Year} год";
                }
                else
                {
                    specialName = $"{From.Year} - {To.Year - 1} год";
                }
            }
            else if (this.IsFullMonthPeriod().HasValue)
            {
                var value = this.IsFullMonthPeriod().Value;
                if (value == 1)
                {
                    specialName = $"{From.Month.GetMonthName()} {From.Year}";
                }
                else
                {
                    specialName = $"{From.Month.GetMonthName()} {From.Year} - {To.AddMonths(-1).Month.GetMonthName()} {To.AddMonths(-1).Year}";
                }
            }
            else if (this.IsFullDayPeriod().HasValue)
            {
                var value = this.IsFullDayPeriod().Value;
                if (value == 1)
                {
                    specialName = $"{From.Day} {From.Month.GetMonthName("MMM")} {From.Year}";
                }
                else
                {
                    specialName = $"{From.Day} {From.Month.GetMonthName("MMM")} {From.Year} - {To.AddDays(-1).Day} {To.AddDays(-1).Month.GetMonthName("MMM")} {To.AddDays(-1).Year}";
                }
            }
            else if (this.IsRuminePeriod())
            {
                specialName = $"Всё время";
            }
            return specialName;
        }

        #endregion

        #region Содержание дат

        public DateTime From { get; init; }
        public DateTime To { get; init; }
        public TimeSpan Diff => To - From;

        public IntervalType IntervalType { get; set; } = new IntervalType(IntervalTypes.HalfStart);

        public bool IsEmpty => From == To;
        public double DaysDifference => Diff.TotalDays;
        
        #endregion

        #region Конструкторы

        public DateRange()
        {

        }
        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
        public DateRange(int year, int month) : this(new DateTime(year, month, 1), new DateTime(year, month, DateTime.DaysInMonth(year, month)))
        {

        }
        public DateRange(Post newer, Post older)
        {
            To = newer.Date;
            From = older.Date;
        }

        #endregion

        #region Взаимодействия с другими датами и интервалами

        /// <summary>
        /// Получать % интервала в другом интервале
        /// </summary>
        public double GetFractionOfRange(DateRange range)
        {
            if(IsEmpty || range.IsEmpty)
            {
                return 0;
            }

            if (IsOutsideOfRange(range))
            {
                return 0;
            }
            else if (IsInsideOfRange(range))
            {
                return 1;
            }
            else
            {
                return CountModOfRange(range);
            }
        }
        private double CountModOfRange(DateRange range)
        {
            DateTime countFrom = range.From > From ? range.From : From;
            DateTime countTo = range.To < To ? range.To : To;

            TimeSpan compareDifference = countTo - countFrom;
            return compareDifference.TotalDays / DaysDifference;
        }

        public bool IsDateInside(DateTime date)
        {
            return From <= date && date < To;
        }
        public bool IsPostInside(Post post)
        {
            return IsDateInside(post.Date);
        }
        public bool IsOutsideOfRange(DateRange range)
        {
            return range.To < From || range.From > To;
        }
        public bool IsInsideOfRange(DateRange range)
        {
            return range.From < From && range.To > To;
        }
        public bool IsIntersectedWithRange(DateRange range)
        {
            return !IsOutsideOfRange(range) && !IsInsideOfRange(range);
        }

        #endregion

        #region Создание новых интервалов
        /// <summary>
        /// Прибавить к датам от и до указанное число лет и вернуть новый временной промежуток
        /// </summary>
        public DateRange AddYearsGetNewRange(int years)
        {
            return new DateRange(this.From.AddYears(years), this.To.AddYears(years));
        }
        /// <summary>
        /// Прибавить к датам от и до указанное число месяцев и вернуть новый временной промежуток
        /// </summary>
        public DateRange AddMonthsGetNewRange(int months)
        {
            return new DateRange(this.From.AddMonths(months), this.To.AddMonths(months));
        }
        /// </summary>
        public DateRange AddDaysGetNewRange(int days)
        {
            return new DateRange(this.From.AddDays(days), this.To.AddDays(days));
        }
        #endregion
    }
}
