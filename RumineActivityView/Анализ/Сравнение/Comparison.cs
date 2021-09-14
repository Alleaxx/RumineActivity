using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class Comparison
    {
        public string Name => IsEmpty ? "Подготовка к сравнению" : $"{Type} {Items.Count} источников акт*вности";
        private string Type => IsSimple ? "Обзор" : "Сравнение";

        //Элементы сравнения
        public List<ActivitySource> Items { get; private set; }
        public ActivitySource CompareElement { get; private set; }
        public List<ActivitySource> PossibleItems { get; private set; }

        public bool IsEmpty => !Items.Any();
        public bool IsSimple => CompareElement == null || (CompareElement != null && Items.Count < 2);


        public Comparison(IEnumerable<Topic> topics)
        {
            Items = new List<ActivitySource>();
            PossibleItems = new List<ActivitySource>(GetAllActivitySources(topics));
        }


        public void Toggle(ActivitySource source)
        {
            if (Items.Contains(source))
            {
                Remove(source);
            }
            else
            {
                Add(source);
            }
        }
        public void ToggleCompare(ActivitySource source)
        {
            if (source == CompareElement)
            {
                SetCompare(null);
            }
            else
            {
                SetCompare(source);
            }
        }
        private void SetCompare(ActivitySource source)
        {
            if (CompareElement == source)
            {
                source = null;
            }
            CompareElement = source;
            UpdateWithCompareElement();
        }

        private void Add(ActivitySource newSource)
        {
            if (!Items.Contains(newSource) && newSource != null)
            {
                newSource.SetReport();
                Items.Add(newSource);
                PossibleItems.Remove(newSource);
                if (CompareElement == null)
                {
                    SetCompare(newSource);
                }
                newSource.UpdateCompareInfo(CompareElement);
            }
        }
        private void Remove(ActivitySource removingSource)
        {
            PossibleItems.Add(removingSource);
            Items.Remove(removingSource);
            if(CompareElement == removingSource)
            {
                SetCompare(null);
            }
        }
        private void UpdateWithCompareElement()
        {
            foreach (var currSource in Items)
            {
                currSource.UpdateCompareInfo(CompareElement);
            }
        }


        //Все возможные источники активности
        private static IEnumerable<ActivitySource> GetAllActivitySources(IEnumerable<Topic> topics)
        {
            return new ActivitySource[]
            {
                new ActivitySource(new TopicsMode(TopicsModes.All)),
                new ActivitySource(new TopicsMode(TopicsModes.NotChat)),
                new ActivitySource(new TopicsMode(TopicsModes.OnlyChat)),
            }.Concat(topics.Select(t => new ActivitySource(t)));
        }
    }
}
