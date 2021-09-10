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

        private Func<Entry, string> GetFirstKey { get; set; }
        private Func<Entry, string> GetSecondKey { get; set; }

        public TableGrouping(Dates date, IEnumerable<Entry> entries)
        {
            int yearDifference = DateTime.Now.Year - 2011;
            switch (date)
            {
                case Dates.Year:
                    GetFirstKey = e => "Всё время";
                    GetSecondKey = e => e.Range.From.ToString("yyyy");
                    ColGroups = Enumerable.Range(2011, yearDifference + 1).Select(y => y.ToString()).ToArray();
                    break;
                case Dates.Month:
                    GetFirstKey = e => e.Range.From.ToString("yyyy");
                    GetSecondKey = e => e.Range.From.ToString("MMMM");
                    ColGroups = Enumerable.Range(1, 12).Select(m => GetMonthName(m)).ToArray();
                    break;
                case Dates.Week:
                    GetFirstKey = e => e.Range.From.ToString("MMMM yyyy");
                    GetSecondKey = e => $"{((e.Range.From.Day / 7) + 1)} неделя";
                    ColGroups = new string[] { "1 неделя", "2 неделя", "3 неделя", "4 неделя", "5 неделя" };
                    break;
                case Dates.Day:
                    GetFirstKey = e => e.Range.From.ToString($"MMMM yyyy");
                    GetSecondKey = e => $"{e.Range.From.Day}";
                    ColGroups = Enumerable.Range(1, 31).Select(i => i.ToString()).ToArray();
                    break;
                default:
                    GetFirstKey = e => "Всё время";
                    GetSecondKey = e => e.Range.From.ToString("yyyy");
                    ColGroups = Enumerable.Range(2011, yearDifference + 1).Select(y => y.ToString()).ToArray();
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


        private string GetMonthName(int month) => new DateTime(2011, month, 1).ToString("MMMM");
    }

}
