﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView.Pages
{
    public partial class SourceCompare : StatComponent
    {
        public Comparison Comparison => App.Comparison;

        private IEnumerable<ActivitySource> PossibleAll => Comparison.PossibleItems.Where(i => i.Mode.Mode != TopicsModes.Topic);
        private IEnumerable<ActivitySource> PossibleTopics => Comparison.PossibleItems.Where(i => i.Mode.Mode == TopicsModes.Topic);



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

        public CompareValue CompareValue { get; set; } = new CompareValue(CompareValues.Absolute);
        private void Set(CompareValue value)
        {
            CompareValue = value;
        }

        public string GetValueCompare(ICompareProp prop)
        {
            switch (CompareValue.Type)
            {
                case CompareValues.Absolute:
                    return prop.GetTotalDiff().ToString("+#,0;-#,0; 0");
                case CompareValues.Relative:
                    return prop.GetModDiff().ToString("0%");
                default:
                    throw new Exception("Такого преобразования значений не существует");
            }
        }
    }
}
