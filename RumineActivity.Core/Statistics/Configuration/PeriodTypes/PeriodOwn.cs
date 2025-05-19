using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    class PeriodOwn : Period
    {
        public PeriodOwn() : base(Periods.Own)
        {
            Name = "Свой период";
            NameReport = $"Ежедневная";
            NameCategory = "По 14 дней";
            TimeInterval = new TimeSpan(14, 0, 0, 0, 0);
            Days = 14;

            EntryDateFunc = range => $"{range.From:dd.MM.yyyy} - {range.To:dd.MM.yyyy}";
            ChartDateFunc = range => $"{range.From:dd.MM.yy}";
        }
    }
}
