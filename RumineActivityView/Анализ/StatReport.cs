using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{

    interface IReportCreator
    {
        StatReport Create();
    }


    //Собирает по каждому посту
    public abstract class Report : IReportCreator
    {
        protected readonly DateTime FoundationDate = new DateTime(2011, 7, 27);

        protected Post[] Posts { get; private set; }
        protected Topic[] Topics { get; private set; }
        protected ReportOptions Options { get; set; }
        public Report(IEnumerable<Post> posts, IEnumerable<Topic> topics, ReportOptions options = null)
        {
            if (options == null)
            {
                options = ReportOptions.AllIncluded(posts, topics);
            }
            Options = options;


            Posts = posts
                .Where(p => options.TopicSources.Contains(p.TopicID))
                .Where(p => p.Date >= options.Period.From && p.Date <= options.Period.To)
                .OrderBy(d => d.Date).ToArray();
            Topics = topics.OrderBy(d => d.ID).ToArray();
        }
        public abstract StatReport Create();
    }

    public class ReportOptions
    {
        //Конкретные даты
        public DateRange Period { get; private set; } = new DateRange();
        //Конкретные темы
        public int[] TopicSources { get; private set; } = Array.Empty<int>();

        public static ReportOptions AllIncluded(IEnumerable<Post> posts, IEnumerable<Topic> topics)
        {
            ReportOptions options = new ReportOptions();
            var sortedPosts = posts.OrderBy(p => p.Date);

            options.Period = new DateRange(sortedPosts.Last(), sortedPosts.First());
            options.TopicSources = topics.Select(t => t.ID).Distinct().ToArray();

            return options;
        }
    }

    //Каждые два поста - период
    public class ReportAll : Report
    {
        public ReportAll(IEnumerable<Post> posts, IEnumerable<Topic> topics, ReportOptions options = null) : base(posts, topics, options)
        {

        }
        public override StatReport Create()
        {
            if(Posts.Length < 2)
            {
                return new StatReport().SetEmpty();
            }

            List<Entry> entries = new List<Entry>();
            for (int i = 1; i < Posts.Length; i++)
            {
                Post prev = Posts[i - 1];
                Post curr = Posts[i];
                entries.Add(GetEntryFor(curr, prev));
            }
            return new StatReport() {
                Name = "Полный отчет с параметрами",
                Period = $"С {Options.Period.From:dd MMMM yyyy} по {Options.Period.To:dd MMMM yyyy}",
                Source = $"Темы: {string.Join(';', Options.TopicSources)}",
                Entries = entries };
        }

        public Entry GetEntryFor(Post a, Post b)
        {
            DateRange range = new DateRange(a, b);
            int difference = a.ID - b.ID;
            return new Entry() { Range = range, Value = difference,
                Name = $"{range.From:dd-MM-yy} - {range.To:dd-MM-yy}" };
        }

    }





    //Отчет об активности. Коллекция записей 'За период написано сообщений'
    public class StatReport
    {
        //Информация
        public string Name { get; set; }
        //Период отчета
        public string Period { get; set; }
        //Охват отчета
        public string Source { get; set; }


        //Данные
        public IEnumerable<Entry> Entries { get; set; }
        public bool IsEmpty => Entries == null || Entries.Count() == 0;

        //Статистика по данным
        public int Count => Entries.Count();
        public double AverageValue => IsEmpty ? 0 : Entries.Average(e => e.ValuePerDay);
        public Entry MostInactive => Entries.OrderBy(e => e.ValuePerDay).FirstOrDefault();
        public Entry MostActive => Entries.OrderBy(e => e.ValuePerDay).LastOrDefault();



        public StatReport SetEmpty()
        {
            Name = "Нулевой отчет";
            Period = "Периода нет";
            Source = "Недостаточно исходных данных для построения отчета!";
            Entries = Array.Empty<Entry>();
            return this;
        }
    }

    //Запись 'За период написано сообщений'
    public class Entry
    {
        public DateRange Range { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }
        public double ValuePerHour => Value / Range.Diff.TotalHours;
        public double ValuePerDay => Value / Range.Diff.TotalDays;
        public double ValuePerMonth => Value / Range.Diff.TotalDays * 30;
    }


    //Временной период
    public class DateRange
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public TimeSpan Diff => To - From;

        public DateRange()
        {

        }
        public DateRange(Post a, Post b)
        {
            To = a.Date;
            From = b.Date;
        }
    }
}
