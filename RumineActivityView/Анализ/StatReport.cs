using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public interface IReportSource
    {
        public IEnumerable<Post> Posts { get; }
        public IEnumerable<Topic> Topics { get; }
    }
    public class AppReportSource : IReportSource
    {
        public AppReportSource()
        {

        }

        public IEnumerable<Post> Posts => StatApp.App.Posts;
        public IEnumerable<Topic> Topics => StatApp.App.Topics;
    }


    interface IReportCreator
    {
        StatReport Create();
    }


    //Собирает по каждому посту
    public abstract class Report : IReportCreator
    {
        public static readonly DateTime FoundationDate = new DateTime(2011, 7, 27);

        protected Post[] Posts { get; private set; }
        protected Topic[] Topics { get; private set; }
        protected ReportOptions Options { get; private set; }

        public Report(IReportSource source, ReportOptions options = null)
        {
            if (options == null)
            {
                options = new ReportOptions();
            }
            Options = options;


            Posts = source.Posts
                .Where(p => p.Date >= options.Period.From && p.Date <= options.Period.To)
                .OrderBy(d => d.Date).ToArray();
            Topics = source.Topics.OrderBy(d => d.ID).ToArray();
        }
        public abstract StatReport Create();
    }

    public class ReportOptions
    {
        //Конкретные даты
        public DateRange Period { get; set; } = new DateRange(Report.FoundationDate, DateTime.Now);
        public TimeSpan TimeInterval { get; set; } = new TimeSpan(24, 0, 0);

        public ReportOptions()
        {

        }
    }

    //Отчет по существующим записям
    public class ReportDefault : Report
    {
        public ReportDefault(IReportSource source, ReportOptions options) : base(source, options)
        {

        }
        public override StatReport Create()
        {
            if (Posts.Length < 2)
            {
                return new StatReport();
            }

            List<Entry> entries = new List<Entry>();
            int a = 1;
            for (int i = 1; i < Posts.Length; i++)
            {
                Post prev = Posts[i - a];
                Post curr = Posts[i];
                if (curr.Date - prev.Date >= Options.TimeInterval)
                {
                    entries.Add(GetEntryFor(curr, prev));
                    a = 1;
                }
                else
                {
                    a++;
                }
            }
            return new StatReport()
            {
                Name = "Полный отчет с параметрами",
                Entries = entries
            };
        }

        public Entry GetEntryFor(Post a, Post b)
        {
            DateRange range = new DateRange(a, b);
            int difference = a.ID - b.ID;
            return new Entry($"{range.From:dd.MMMyyyy} - {range.To:dd.MMMyyyy}", difference) { Range = range };
        }

    }
    //Отчет с РАВНЫМИ периодами, достраивает недостающие данные





    //Отчет об активности. Коллекция записей 'За период написано сообщений'
    public class StatReport
    {
        //Информация
        public string Name { get; set; }


        //Данные
        public IEnumerable<Entry> Entries { get; set; }
        public bool IsEmpty => Entries == null || Entries.Count() == 0;

        //Статистика по данным
        public int Count => Entries.Count();
        public double SumValue => IsEmpty ? 0 : Entries.Sum(e => e.Value);
        public double AverageValue => IsEmpty ? 0 : Entries.Average(e => e.Value);
        public Entry MostInactive => Entries.OrderBy(e => e.ValuePerDay).FirstOrDefault();
        public Entry MostActive => Entries.OrderBy(e => e.ValuePerDay).LastOrDefault();

        public StatReport() : this("Нулевой отчет", Array.Empty<Entry>())
        {

        }
        public StatReport(string name, IEnumerable<Entry> entries)
        {
            Name = name;
            Entries = entries;
        }
    }

    //Запись 'За период написано сообщений'
    public class Entry
    {
        public DateRange Range { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public Entry()
        {

        }
        public Entry(string name, double value)
        {
            Name = name;
            Value = value;
        }

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
        public DateRange(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }
        public DateRange(Post a, Post b)
        {
            To = a.Date;
            From = b.Date;
        }
    }


    //Методы измерения
    public enum MeasureUnits
    {
        Messages, Pages, OldPages
    }
    public enum MeasureMethods
    {
        Total, ByMonth, ByDay, ByHour
    }
    public class MeasureUnit
    {
        public MeasureUnits Unit { get; private set; }
        public string Name { get; private set; }
        public int Value { get; private set; }
        public MeasureUnit(MeasureUnits unit)
        {
            Unit = unit;
            switch (Unit)
            {
                case MeasureUnits.Messages:
                    Name = "Сообщения";
                    Value = 1;
                    break;
                case MeasureUnits.Pages:
                    Name = "Страницы";
                    Value = 20;
                    break;
                case MeasureUnits.OldPages:
                    Name = "Старые страницы";
                    Value = 10;
                    break;
            }
        }
    }
    public class MeasureMethod
    {
        public MeasureMethods Method { get; private set; }
        public string Name { get; private set; }
        public MeasureMethod(MeasureMethods method)
        {
            Method = method;
            switch (Method)
            {
                case MeasureMethods.ByDay:
                    Name = "В среднем в день";
                    break;
                case MeasureMethods.ByHour:
                    Name = "В среднем в час";
                    break;
                case MeasureMethods.ByMonth:
                    Name = "В среднем за месяц";
                    break;
                case MeasureMethods.Total:
                    Name = "Всего за период";
                    break;
            }
        }
    }

    public enum EntryIntervals
    {
        Day, Week, Month, Year, Own
    }
    public class EntryInterval
    {
        public string Name { get; private set; }
        public double Days { get; set; }

        public EntryInterval(string name, double days)
        {
            Name = name;
            Days = days;
        }
    }
    public class DateInterval
    {
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public DateRange Dates
        {
            get
            {
                if(Year == 0)
                {
                    return new DateRange(Report.FoundationDate, DateTime.Now);
                }
                else
                {
                    if(Month == 0)
                    {
                        return new DateRange(new DateTime(Year,1,1), new DateTime(Year,12,31));
                    }
                    else
                    {
                        return new DateRange(new DateTime(Year, Month, 1), new DateTime(Year, Month, DateTime.DaysInMonth(Year,Month)));
                    }
                }
            }
        }

        public DateInterval(int year, int month)
        {
            Year = year;
            Month = month;
        }
    }
}
