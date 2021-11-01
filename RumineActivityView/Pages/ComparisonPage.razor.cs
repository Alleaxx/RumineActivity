using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using RumineActivity.Core;
namespace RumineActivity.View.Pages
{
    public partial class ComparisonPage : StatComponent
    {
        [Inject]
        public IComparison Comparison { get; set; }

        private IEnumerable<ActivitySource> PossibleAll => Comparison.PossibleItems.Where(i => i.Mode.Mode != PostSources.Topic);
        private IEnumerable<ActivitySource> PossibleTopics => Comparison.PossibleItems.Where(i => i.Mode.Mode == PostSources.Topic);



        private IEnumerable<ICompareProp[]> PropertyRows() => Rows(s => s.Properties);
        private IEnumerable<ICompareProp[]> HistoryRows() => Rows(s => s.History);

        private IEnumerable<ICompareProp[]> Rows(Func<ActivitySource, ICompareProp[]> getArr)
        {
            List<ICompareProp[]> rows = new List<ICompareProp[]>();
            int length = getArr.Invoke(Comparison.Items.First()).Length;
            for (int i = 0; i < length; i++)
            {
                var row = Comparison.Items.Select(item => getArr.Invoke(item)[i]);
                rows.Add(row.ToArray());
            }
            return rows;

        }

        private string GetSourceClass(ActivitySource source) => Comparison.CompareElement == source ? "compare" : "";
        private string GetCompareClass(ICompareProp prop)
        {
            double value = prop.GetTotalDiff();
            double mod = prop.GetModDiff();
            if (value > 0)
            {
                return "more";
            }
            else if (value < 0)
            {
                return "less";
            }

            if (mod == 1)
            {
                return "self";
            }
            else
            {
                return "self";
            }
        }

        public string NewItem
        {
            get => "-";
            set
            {
                ActivitySource source = Comparison.PossibleItems.Where(p => p.Name == value).FirstOrDefault();
                if(source != null)
                {
                    Comparison.Toggle(source);
                }
            }
        }

        public CompareView CompareView { get; set; } = CompareView.Create(CompareViews.Absolute);
        private void Set(CompareView value)
        {
            CompareView = value;
        }

    }
}
