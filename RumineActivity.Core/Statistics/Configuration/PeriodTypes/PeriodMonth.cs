using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    class PeriodMonth : Period
    {
        public PeriodMonth() : base(Periods.Month)
        {
            Name = "Месяц";
            NameReport = "Ежемесячная";
            NameCategory = "По месяцам";
            TimeInterval = new TimeSpan(30, 0, 0, 0, 0);

            EntryDateFunc = range => range.From.ToString("MMMM yyyy");
            ChartDateFunc = range => $"{range.From.Month.GetMonthName("MMM")} {range.From:yy}";
        }
        public override DateTime GetNextDate(DateTime date)
        {
            DateTime nextMonth = date.AddMonths(1);
            return new DateTime(nextMonth.Year, nextMonth.Month, 1);
        }
    }
}
