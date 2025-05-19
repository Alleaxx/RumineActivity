using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    class PeriodYear : Period
    {
        public PeriodYear() : base(Periods.Year)
        {
            Name = "Год";
            NameReport = "Ежегодная";
            NameCategory = "По годам";
            TimeInterval = new TimeSpan(365, 0, 0, 0, 0);

            EntryDateFunc = range => range.From.ToString("yyyy год");
            ChartDateFunc = range => range.From.ToString("yyyy");
        }
        public override DateTime GetNextDate(DateTime date)
        {
            return new DateTime(date.Year + 1, 1, 1);
        }
    }
}
