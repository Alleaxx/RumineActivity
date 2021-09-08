using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView.Pages
{
    public partial class SourceCompare : StatComponent
    {
        public Comparison Comparison => App.Comparison;

        public IEnumerable<ActivitySource> PossibleAll => Comparison.PossibleItems.Where(i => i.Mode.Mode != TopicsModes.Topics);
        public IEnumerable<ActivitySource> PossibleTopics => Comparison.PossibleItems.Where(i => i.Mode.Mode == TopicsModes.Topics);


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
            get => "";
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
        public CompareValue[] Values { get; set; } = CompareValue.Values;
        private void Set(CompareValue value)
        {
            CompareValue = value;
        }
        public string GetValue(ICompareProp prop)
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

    public enum CompareValues
    {
        Relative, Absolute
    }
    public class CompareValue : EnumType<CompareValues>
    {
        public CompareValue(CompareValues values) : base(values)
        {
            switch (values)
            {
                case CompareValues.Absolute:
                    Name = "Значения";
                    break;
                case CompareValues.Relative:
                    Name = "Проценты";
                    break;
            }
        }

        public static CompareValue[] Values => Enum.GetValues<CompareValues>().Select(v => new CompareValue(v)).ToArray();
    }
}
