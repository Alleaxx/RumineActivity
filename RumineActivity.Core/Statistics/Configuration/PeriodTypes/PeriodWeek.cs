using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    class PeriodWeek : Period
    {
        public PeriodWeek() : base(Periods.Week)
        {
            Name = "Неделя";
            NameReport = "Еженедельная";
            NameCategory = "По неделям";
            TimeInterval = new TimeSpan(7, 0, 0, 0, 0);

            EntryDateFunc = range =>
            {
                int week = DateExtensions.DefineDayYearWeekIndex(range.From);
                bool sameMonth = range.From.Month == range.To.Month;
                string month = sameMonth ? $"{range.From:dd}-{range.To:dd} {range.From.Month.GetMonthName("MMM")}" : $"{range.From:dd} {range.From.Month.GetMonthName("MMM")} - {range.To:dd} {range.To.Month.GetMonthName("MMM")}";
                return $"{week} неделя {range.From:yyyy} ({month})";
            };
            ChartDateFunc = range => $"{DateExtensions.DefineDayYearWeekIndex(range.From)} нед {range.From:yy}";
        }

        public override DateTime GetNextDate(DateTime date)
        {
            int weekIndex = DateExtensions.DefineDayYearWeekIndex(date);
            int newWeekIndex = weekIndex;
            for (int i = 1; i <= 8; i++)
            {
                var newDate = date.AddDays(i);
                newWeekIndex = DateExtensions.DefineDayYearWeekIndex(newDate);
                if (weekIndex != newWeekIndex)
                {
                    return newDate;
                }
            }
            throw new Exception("Не удалось найти новую неделю! Приехали!");
        }
    }
}
