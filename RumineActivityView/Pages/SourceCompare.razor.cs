using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView.Pages
{
    public partial class SourceCompare : StatComponent
    {
        public Comparison Comparison => App.Comparison;

        private string GetSourceClass(ActivitySource source) => Comparison.CompareElement == source ? "compare" : "";

        public IEnumerable<ICompareProp[]> PropertyRows()
        {
            List<ICompareProp[]> rows = new List<ICompareProp[]>();
            int length = Comparison.Items.First().Properties.Length;
            for (int i = 0; i < length; i++)
            {
                var row = Comparison.Items.Select(item => item.Properties[i]);
                rows.Add(row.ToArray());
            }
            return rows;
        }

        public IEnumerable<DoubleProperty[]> ActivityRows()
        {
            List<DoubleProperty[]> rows = new List<DoubleProperty[]>();
            int length = Comparison.Items.First().Properties.Length;
            for (int i = 0; i < length; i++)
            {
                var row = Comparison.Items.Select(item => item.History[i]);
                rows.Add(row.ToArray());
            }
            return rows;

        }

        public string GetCompareClass(ICompareProp prop)
        {
            double value = prop.GetTotalDiff();
            double mod = prop.GetModDiff();
            if (mod == 1)
            {
                return "self";
            }


            if (value > 0)
            {
                return "more";
            }
            else if (value < 0)
            {
                return "less";
            }
            else
            {
                return "self";
            }
        }

        public bool ShowRelative { get; set; } = false;
        public string GetValue(ICompareProp prop) => ShowRelative ? (prop.GetModDiff()).ToString("0%") : prop.GetTotalDiff().ToString("#,0");

    }
}
