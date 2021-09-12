using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class TableGrouping
    {
        public Dictionary<string, Dictionary<string, Entry>> Table { get; private set; }
        public Dictionary<string, double> TableSum { get; private set; }
        public string[] ColGroups { get; private set; }

        private readonly Func<Entry, string> GetFirstKey;
        private readonly Func<Entry, string> GetSecondKey;

        public TableGrouping(Periods date, IEnumerable<Entry> entries)
        {
            int yearDifference = DateTime.Now.Year - 2011;
            switch (date)
            {
                case Periods.Year:
                    GetFirstKey = e => "Всё время";
                    GetSecondKey = e => e.Range.From.ToString("yyyy");
                    ColGroups = Enumerable.Range(2011, yearDifference + 1).Select(y => y.ToString()).ToArray();
                    break;
                case Periods.Month:
                    GetFirstKey = e => e.Range.From.ToString("yyyy");
                    GetSecondKey = e => e.Range.From.ToString("MMMM");
                    ColGroups = Enumerable.Range(1, 12).Select(m => m.MonthName()).ToArray();
                    break;
                case Periods.Week:
                    GetFirstKey = e => e.Range.From.ToString("MMMM yyyy");
                    GetSecondKey = e => $"{(e.Range.From.Day / 7) + 1} неделя";
                    ColGroups = new string[] { "1 неделя", "2 неделя", "3 неделя", "4 неделя", "5 неделя" };
                    break;
                case Periods.Day:
                    GetFirstKey = e => $"{((e.Range.From.Day - 1) / 7) + 1}н | {e.Range.From:MMMM yyyy}";
                    GetSecondKey = e => e.Range.From.ToString("dddd");

                    DateTime sundayDate = new DateTime(2021, 9, 5);
                    int startWeekDay = (int)entries.First().Range.From.DayOfWeek;
                    var firstWeekPart = Enumerable.Range(startWeekDay, 8 - startWeekDay);
                    var secondWeekPart = Enumerable.Range(1, startWeekDay - 1);
                    ColGroups = firstWeekPart.Concat(secondWeekPart).Select(y => sundayDate.AddDays(y).ToString("dddd")).ToArray();
                    break;
                default:
                    GetFirstKey = e => e.Range.From.ToString("yyyy");
                    GetSecondKey = e => e.Range.From.ToString("MMMM");
                    ColGroups = Enumerable.Range(1, 12).Select(m => m.MonthName()).ToArray();
                    break;
            }
            
            CreateTable(entries);
        }








        private void CreateTable(IEnumerable<Entry> entries)
        {
            TableSum = new Dictionary<string, double>();
            Table = new Dictionary<string, Dictionary<string, Entry>>();
            var grouped = entries.ToLookup(e => GetFirstKey(e));
            foreach (var firstGroup in grouped)
            {
                string firstKey = firstGroup.Key;
                TableSum.Add(firstKey, firstGroup.Sum(i => i.PostsDefault));
                Table.Add(firstKey, new Dictionary<string, Entry>());
                foreach (var secondGroup in ColGroups)
                {
                    Table[firstKey].Add(secondGroup, firstGroup.FirstOrDefault(e => GetSecondKey(e) == secondGroup));
                }
            }
        }
    }


}
