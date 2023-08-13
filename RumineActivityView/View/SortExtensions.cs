using RumineActivity.Core;
using RumineActivity.Core.Measures;
using System.Collections.Generic;
using System.Linq;

namespace RumineActivity.View
{
    public static class SortExtensions
    {
        static SortExtensions()
        {
            var sorts = new Sorting<Entry, object>[]
            {
                new Sorting<Entry, object>()
                {
                    Key = Sortings.Index,
                    Title = "По дате",
                    SortFunc = e => e.Index
                },
                new Sorting<Entry, object>()
                {
                    Key = Sortings.Value,
                    Title = "По среднему",
                    SortFunc = e => e.GetValue(MeasureMethods.ByHour, MeasureUnits.Messages)
                },
                new Sorting<Entry, object>()
                {
                    Key = Sortings.ValueTotal,
                    Title = "По общему",
                    SortFunc = e => e.GetValue(MeasureMethods.Total, MeasureUnits.Messages)
                },
            };
            EntrySorts = sorts.ToDictionary(s => s.Key);
        }
        public static Dictionary<Sortings, Sorting<Entry, object>> EntrySorts { get; set; }
    }
}
