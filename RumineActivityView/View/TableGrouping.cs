using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
using RumineActivity.Core.Measures;

namespace RumineActivity.View
{
    public class TableGrouping
    {
        public Dictionary<string, Dictionary<string, Entry>> TableRows { get; private set; }
        public Dictionary<string, double> TableSum { get; private set; }
        public string[] ColGroups { get; private set; }

        private Func<Entry, string> GetFirstKey;
        private Func<Entry, string> GetSecondKey;

        private readonly ValuesViewConfig Config;

        public TableGrouping(ValuesViewConfig config, StatisticsReport report)
        {
            var date = report.Period.Type;
            var entries = report.Entries;
            Config = config;
            switch (date)
            {
                case Periods.Year:
                    SetYear();
                    break;
                case Periods.Month:
                    SetMonth();
                    break;
                case Periods.Season:
                    SetSeason();
                    break;
                case Periods.Week:
                    SetWeek();
                    break;
                case Periods.Day:
                    SetDay(entries);
                    break;
                default:
                    SetMonth();
                    break;
            }
            
            CreateTable(entries);
        }
        
        private void SetYear()
        {
            int yearDifference = DateTime.Now.Year - 2011;
            GetFirstKey = e => "Всё время";
            GetSecondKey = e => e.FromDate.ToString("yyyy");
            ColGroups = Enumerable.Range(2011, yearDifference + 1).Select(y => y.ToString()).ToArray();
        }
        private void SetMonth()
        {
            GetFirstKey = e => e.FromDate.ToString("yyyy");
            GetSecondKey = e => e.FromDate.Month.GetMonthName("MMM");
            ColGroups = Enumerable.Range(1, 12).Select(m => m.GetMonthName("MMM")).ToArray();
        }
        private void SetSeason()
        {
            GetFirstKey = e => e.FromDate.ToString("yyyy");
            GetSecondKey = e => $"{DateExtensions.DefineSeasonSymbol(e.FromDate)} сезон";
            ColGroups = Enumerable.Range(1, 4).Select(m => $"{DateExtensions.SeasonToSymbol(m)} сезон").ToArray();
        }
        private void SetDay(IEnumerable<Entry> entries)
        {
            GetFirstKey = e => $"{DateExtensions.DefineDayMonthWeekIndex(e.FromDate)}н | {e.FromDate:MMMM yyyy}";
            GetSecondKey = e => e.FromDate.ToString("dddd");

            var fullWeek = Enumerable.Range(20, 7).Select(i => new DateTime(2023, 11, i));
            ColGroups = fullWeek.Select(d => d.ToString("dddd")).ToArray();
        }
        private void SetWeek()
        {
            GetFirstKey = e =>  e.FromDate.ToString("MMM yyyy");
            GetSecondKey = e => DateExtensions.DefineDayMonthWeekIndex(e.FromDate) != 5 
                                ? $"{DateExtensions.DefineDayMonthWeekIndex(e.FromDate)} неделя"
                                : "5/1 неделя (переходная)";
            ColGroups = new string[] { "1 неделя", "2 неделя", "3 неделя", "4 неделя", "5/1 неделя (переходная)" };
        }





        private void CreateTable(IEnumerable<Entry> entries)
        {
            TableSum = new Dictionary<string, double>();
            TableRows = new Dictionary<string, Dictionary<string, Entry>>();
            var grouped = entries.ToLookup(e => GetFirstKey(e));
            foreach (var firstGroup in grouped)
            {
                string firstKey = firstGroup.Key;
                TableSum.Add(firstKey, firstGroup.Sum(i => i.GetValueTotal(Config.MeasureUnit)));
                TableRows.Add(firstKey, new Dictionary<string, Entry>());
                foreach (var secondGroup in ColGroups)
                {
                    TableRows[firstKey].Add(secondGroup, firstGroup.FirstOrDefault(e => GetSecondKey(e) == secondGroup));
                }
            }
        }
    }
    class Grouping
    {
        public string[] ColGroups { get; private set; }

        public Func<Entry, string> GetFirstKey { get; set; }
        public Func<Entry, string> GetSecondKey { get; set; }

        public Grouping()
        {

        }
    }

}
