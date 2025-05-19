using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    class PeriodDay : Period
    {
        public PeriodDay() : base(Periods.Day)
        {
            Name = "День";
            NameReport = "Ежедневная";
            NameCategory = "По дням";
            TimeInterval = new TimeSpan(1, 0, 0, 0, 0);

            EntryDateFunc = range => range.From.ToString("dd.MM.yyyy");
            ChartDateFunc = range => range.From.ToString("dd.MM.yy");
        }
    }
}
