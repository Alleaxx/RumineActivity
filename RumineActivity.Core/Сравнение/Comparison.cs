using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public interface IComparison
    {
        string Name { get; }
        bool IsEmpty { get; }
        bool IsSimple { get; }

        List<ActivitySource> Items { get; }
        List<ActivitySource> PossibleItems { get; }
        ActivitySource CompareElement { get; }

        void ToggleCompare(ActivitySource source);
        void Toggle(ActivitySource source);

    }
    public class Comparison : Named, IComparison
    {
        public override string Name => IsEmpty ? "Подготовка к сравнению" : $"{Type} {Items.Count} источников акт*вности";
        private string Type => IsSimple ? "Обзор" : "Сравнение";

        private readonly IReportsFactory ReportsFactory;
        private readonly IActivityApi ActivityApi;

        //Элементы сравнения
        public List<ActivitySource> Items { get; private set; }
        public ActivitySource CompareElement { get; private set; }
        public List<ActivitySource> PossibleItems { get; private set; }

        public bool IsEmpty => !Items.Any();
        public bool IsSimple => CompareElement == null || (CompareElement != null && Items.Count < 2);


        public Comparison(IReportsFactory reportsFactory, IActivityApi api)
        {
            ReportsFactory = reportsFactory;
            ActivityApi = api;
            Items = new List<ActivitySource>();
            Load();
        }
        private async void Load()
        {
            var topics = (await ActivityApi.GetForum()).Topics;
            PossibleItems = new List<ActivitySource>(GetAllActivitySources(topics));
        }
        //Все возможные источники активности
        private IEnumerable<ActivitySource> GetAllActivitySources(IEnumerable<Topic> topics)
        {
            return new ActivitySource[]
            {
                new ActivitySource(ReportsFactory, PostSource.Create(PostSources.All)),
                new ActivitySource(ReportsFactory, PostSource.Create(PostSources.NotChat)),
                new ActivitySource(ReportsFactory, PostSource.Create(PostSources.OnlyChat)),
            }.Concat(topics.Select(t => new ActivitySource(ReportsFactory, t)));
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
                SetCompareNone();
            }
            else
            {
                SetCompare(source);
            }
        }

        private void SetCompareNone()
        {
            SetCompare(null);
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
                AddNew(newSource);
            }
        }
        private async void AddNew(ActivitySource newSource)
        {
            await newSource.SetReport();
            Items.Add(newSource);
            PossibleItems.Remove(newSource);
            if (CompareElement == null)
            {
                SetCompare(newSource);
            }
            newSource.UpdateCompareInfo(CompareElement);
        }


        private void Remove(ActivitySource removingSource)
        {
            if (Items.Remove(removingSource))
            {
                PossibleItems.Add(removingSource);
                if (CompareElement == removingSource)
                {
                    SetCompareNone();
                }
            }
        }
        private void UpdateWithCompareElement()
        {
            foreach (var currSource in Items)
            {
                currSource.UpdateCompareInfo(CompareElement);
            }
        }


    }
}
