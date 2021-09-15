using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum CompareProperties
    {
        FirstPost, LastPost, DaysActive, TotalPosts, AveragePosts
    }
    public class ActivitySource : Named
    {
        public PostSource Mode { get; private set; }
        private StatisticsReport Report { get; set; }


        //Статистика по источнику активности
        private Dictionary<CompareProperties, ICompareProp> Values { get; set; } = new Dictionary<CompareProperties, ICompareProp>();
        public ICompareProp[] Properties { get; private set; } = Array.Empty<ICompareProp>();
        public ICompareProp[] History { get; private set; } = Array.Empty<ICompareProp>();


        //Задание источника
        public ActivitySource(Topic topic)
        {
            Mode = PostSource.Create(topic.ID);
            Name = $"{topic.Name} [{topic.ID}]";
        }
        public ActivitySource(PostSource mode)
        {
            Mode = mode;
            Name = mode.Name;
        }

        //Создание отчета
        public void SetReport()
        {
            bool Created = Report != null;
            if (!Created)
            {
                CreateReport();
            }
        }
        private void CreateReport()
        {
            Report = ReportsFactory.CreateReport(Reports.Fact, new ReportCreatorOptions(Mode));
            if (!Report.IsEmpty)
            {
                SetProperties();
                SetHistory();
            }
        }

        //Задание информации, обновление сравнения
        private void SetProperties()
        {
            DateTime firstPost = Report.DateRangePosts.From;
            DateTime lastPost = Report.DateRangePosts.To;
            double sumPosts = Report.SumValue;
            double daysActive = (lastPost - firstPost).TotalDays;

            Values = new Dictionary<CompareProperties, ICompareProp>()
            {
                [CompareProperties.FirstPost] = new DateProperty("Первый пост", firstPost),
                [CompareProperties.LastPost] = new DateProperty("Последний пост", lastPost),
                [CompareProperties.TotalPosts] = new DoubleProperty("Всего постов", "#,0", sumPosts),
                [CompareProperties.DaysActive] = new DoubleProperty("Охватывает дней", "#,0.0", daysActive),
                [CompareProperties.AveragePosts] = new DoubleProperty("~ постов в день", "#,0.0", sumPosts / daysActive),
            };
            Properties = Values.Values.ToArray();
        }
        private void SetHistory()
        {
            History = new ICompareProp[]
            {
                new DoubleProperty("День", "~ #,0", GetWrittenPosts(1)),
                new DoubleProperty("Неделю", "~ #,0", GetWrittenPosts(7)),
                new DoubleProperty("Месяц", "~ #,0", GetWrittenPosts(30)),
                new DoubleProperty("3 месяца", "~ #,0", GetWrittenPosts(90)),
                new DoubleProperty("Год", "~ #,0", GetWrittenPosts(365)),
                new DoubleProperty("3 года", "~ #,0", GetWrittenPosts(365 * 3))
            };
        }

        //Обновить сравнение
        public void UpdateCompareInfo(ActivitySource elem)
        {
            if (elem == null)
            {
                elem = this;
            }
            foreach (var property in Values.Keys)
            {
                Values[property].SetCompare(elem.Values[property]);
            }
        }


        private double GetWrittenPosts(int daysSince)
        {
            TimeSpan days = new TimeSpan(daysSince, 0, 0, 0, 0);
            DateTime date = Report.DateRangePosts.From + days;
            var entry = Report.Entries.FirstOrDefault(e => e.Range.From >= date) ?? Report.Entries.Last();
            return entry.LastPostIndex;
        }
    }
}
