using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    class PeriodSeason : Period
    {
        public PeriodSeason() : base(Periods.Season)
        {
            Name = "Сезон";
            NameReport = "Ежесезонная";
            NameCategory = "По сезонам";
            TimeInterval = new TimeSpan(90, 0, 0, 0, 0);

            EntryDateFunc = range => $"{DateExtensions.SeasonToString(DateExtensions.DefineSeason(range.From))} {range.From:yyyy}";
            ChartDateFunc = range => $"{DateExtensions.SeasonToString(DateExtensions.DefineSeason(range.From))} {range.From:yy}";
        }
    }
}
