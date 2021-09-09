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

        public List<ActivitySource> Items { get; private set; }
        public ActivitySource CompareElement { get; set; }
        public List<ActivitySource> PossibleItems { get; private set; }

        public bool IsEmpty => !Items.Any();
        public bool IsSimple => CompareElement == null;
        public Comparison(IEnumerable<Topic> topics)
        {
            Items = new List<ActivitySource>();
            PossibleItems = new List<ActivitySource>(GetAllActivitySources(topics));
        }


        public void Add(ActivitySource source)
        {
            if (!Items.Contains(source) && source != null)
            {
                source.SetReport();
                Items.Add(source);
                PossibleItems.Remove(source);
                if (CompareElement == null)
                {
                    SetCompare(source);
                }
                source.UpdateCompareInfo(CompareElement);
            }
        }
        public void Remove(ActivitySource source)
        {
            PossibleItems.Add(source);
            Items.Remove(source);
            if(CompareElement == source)
            {
                SetCompare(null);
            }
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
        public void SetCompare(ActivitySource source)
        {
            CompareElement = source;
            Update();
        }
        private void Update()
        {
            foreach (var source in Items)
            {
                source.UpdateCompareInfo(CompareElement);
            }
        }


        //Все возможные источники активности
        private static IEnumerable<ActivitySource> GetAllActivitySources(IEnumerable<Topic> topics)
        {
            List<ActivitySource> sources = new List<ActivitySource>()
            {
                new ActivitySource(TopicsModes.All),
                new ActivitySource(TopicsModes.OnlyChat),
                //new ActivitySource(TopicsModes.NotChat),
            };
            foreach (var topic in topics)
            {
                sources.Add(new ActivitySource(topic.ID, topics));
            }
            return sources;
        }
    }

    public class ActivitySource
    {
        public TopicsMode Mode { get; private set; }
        public StatisticsReport Report { get; private set; }
        public IEnumerable<Entry> SortedEntries { get; private set; }


        //Статистика по источнику активности
        public string Name { get; protected set; }

        public ICompareProp[] Properties { get; set; } = Array.Empty<ICompareProp>();
        private DateProperty FirstPost { get; set; }
        private DateProperty LastPost { get; set; }
        private DoubleProperty DaysActive { get; set; }
        private DoubleProperty TotalPosts { get; set; }
        private DoubleProperty AveragePosts { get; set; }

        public List<DoubleProperty> History { get; private set; }


        //Задание источника
        public ActivitySource(TopicsModes mode) : this(new TopicsMode(mode))
        {

        }
        public ActivitySource(int id, IEnumerable<Topic> topics) : this(new TopicsMode(id))
        {
            int topicId = Mode.TopicId;
            Topic topic = topics.FirstOrDefault(t => t.ID == topicId);
            if (topic == null)
            {
                Name = $"Тема {topicId}";
            }
            else
            {
                Name = $"{topic.Name} [{topicId}]";
            }
        }
        public ActivitySource(TopicsMode mode)
        {
            Mode = mode;
            Name = mode.Name;
        }
        //Создание отчета
        public void SetReport()
        {
            if (!Created)
            {
                Report = CreateReport();
                SortedEntries = Report.Entries.OrderBy(e => e.Range.From).ToArray();
                SetInfo();
            }
        }
        public bool Created => Report != null;
       
        protected virtual StatisticsReport CreateReport()
        {
            return ReportsFactory.CreateReport(Reports.Fact, new ReportOptions(Mode));
        }
        

        //Задание информации, обновление сравнения
        protected virtual void SetInfo()
        {
            History = new List<DoubleProperty>();
            if (!Report.IsEmpty)
            {
                FirstPost = new DateProperty("Первый пост", Report.FirstLastPost.From);
                LastPost = new DateProperty("Последний пост", Report.FirstLastPost.To);
                DaysActive = new DoubleProperty("Охватывает дней", "#,0.0", (LastPost.Property - FirstPost.Property).TotalDays);
                TotalPosts = new DoubleProperty("Всего постов", "#,0", Report.SumValue);
                AveragePosts = new DoubleProperty("~ постов в день", "#,0.0", TotalPosts.Property / DaysActive.Property);
                Properties = new ICompareProp[] { FirstPost, LastPost, DaysActive, TotalPosts, AveragePosts };

                History.Add(new DoubleProperty("Спустя день", "~ #,0", GetTotalValue(1)));
                History.Add(new DoubleProperty("Спустя неделю", "~ #,0", GetTotalValue(7)));
                History.Add(new DoubleProperty("Спустя месяц", "~ #,0", GetTotalValue(30)));
                History.Add(new DoubleProperty("Спустя 3 месяца", "~ #,0", GetTotalValue(90)));
                History.Add(new DoubleProperty("Спустя год", "~ #,0", GetTotalValue(365)));
                History.Add(new DoubleProperty("Спустя 3 года", "~ #,0", GetTotalValue(365 * 3)));
            }
        }
        public void UpdateCompareInfo(ActivitySource elem)
        {
            if(elem == null)
            {
                elem = this;
            }
            FirstPost.SetCompare(elem.FirstPost);
            LastPost.SetCompare(elem.LastPost);
            DaysActive.SetCompare(elem.DaysActive);
            TotalPosts.SetCompare(elem.TotalPosts);
            AveragePosts.SetCompare(elem.AveragePosts);
        }

        private double GetTotalValue(int daysSince)
        {
            var entry = FindNearestEntry(new TimeSpan(daysSince, 0,0,0,0));
            return entry.GetPosts(PostsType.Total, MeasureMethods.Total);
            //if (Mode.Mode == TopicsModes.Topics)
            //    return entry.ValueTopicTotal;
            //else
            //    return entry.ValueTotal;
        }
        private Entry FindNearestEntry(TimeSpan since)
        {
            DateTime newDate = FirstPost.Property.Date + since;
            Entry found = SortedEntries.FirstOrDefault(e => e.Range.From >= newDate);
            if(found == null)
            {
                found = Report.Entries.Where(e => e.Range.From < newDate).OrderByDescending(e => e.Range.From).First();
            }
            return found;
        }
    }
    public interface ICompareProp
    {
        string Name { get; }
        string ToString();
        double GetModDiff();
        double GetTotalDiff();
        bool CompareEqual();
    }
    public class CrProperty<T> : ICompareProp
    {
        public T PropertyCompare { get; private set; }
        public T Property { get; private set; }


        public virtual double GetModDiff() => 0;
        public virtual double GetTotalDiff() => 0;


        public string Format { get; set; }
        public string Name { get; set; } = "";

        public CrProperty(T source)
        {
            Property = source;
        }
        public void SetCompare(CrProperty<T> compare)
        {
            PropertyCompare = compare.Property;
        }

        public virtual bool CompareEqual() => PropertyCompare != null ? Property.Equals(PropertyCompare) : true;
    }
    public class DoubleProperty : CrProperty<double>
    {
        public override string ToString()
        {
            return Property.ToString(Format);
        }

        public override double GetModDiff()
        {
            return Property / PropertyCompare;
        }
        public override double GetTotalDiff()
        {
            return Property - PropertyCompare;
        }
        public DoubleProperty(string name, string format, double source) : base(source)
        {
            Name = name;
            Format = format;
        }
    }
    public class DateProperty : CrProperty<DateTime>
    {
        public override string ToString()
        {
            return Property.ToString(Format);
        }

        public override double GetTotalDiff()
        {
            return (Property - PropertyCompare).TotalDays;
        }
        public override double GetModDiff()
        {
            return 0;
        }

        public DateProperty(string name, DateTime date) : base(date)
        {
            Name = name;
            Format = "dd-MM-yyyy";
        }
    }
}
