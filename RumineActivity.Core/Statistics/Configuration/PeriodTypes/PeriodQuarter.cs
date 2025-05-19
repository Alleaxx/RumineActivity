using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    class PeriodQuarter : Period
    {
        public PeriodQuarter() : base(Periods.Quarter)
        {
            Name = "Квартал";
            NameReport = "Ежеквартальная";
            NameCategory = "По кварталам";
            TimeInterval = new TimeSpan(90, 0, 0, 0, 0);

            EntryDateFunc = range => $"{DateExtensions.DefineQuarterSymbol(range.From)} квартал {range.From:yyyy}";
            ChartDateFunc = range => $"{DateExtensions.DefineQuarterSymbol(range.From)} кв. {range.From:yy}";
        }
        public override DateTime GetNextDate(DateTime date)
        {
            int seasonIndex = DateExtensions.DefineQuarter(date);
            int newSeasonIndex = seasonIndex;
            for (int i = 1; i <= 12; i++)
            {
                var newDate = date.AddMonths(i);
                newSeasonIndex = DateExtensions.DefineQuarter(newDate);
                if (seasonIndex != newSeasonIndex)
                {
                    return newDate;
                }
            }
            throw new Exception("Не удалось найти новый квартал! Приехали!");
        }
    }
}
