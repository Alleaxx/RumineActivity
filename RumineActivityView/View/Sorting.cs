using RumineActivity.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RumineActivity.View
{
    public enum Sortings
    {
        Index,
        Value,
        ValueTotal,
        Accuracy,
        Trend,

        RageTitle,
        RageCountUniq,
        RageCountTotal,
        RageCoeff,
    }
    public class Sorting<T,TKey>
    {
        public override string ToString()
        {
            return $"{Title}";
        }

        public event Action OnSortDirectionChanged;

        public Sortings Key { get; set; }
        public string Title { get; set; }
        public Func<T,TKey> SortFunc { get; set; }
        public bool Descending
        {
            get => descending;
            set
            {
                descending = value;
                OnSortDirectionChanged?.Invoke();
            }
        }
        private bool descending;



        public string GetSortSymbol()
        {
            return Descending ? "🡦" : "🡥";
        }

    }
}
