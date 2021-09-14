using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет с РАВНЫМИ периодами, достраивает недостающие данные
    public class ReportPeriods : ReportCreator
    {
        public ReportPeriods(IForumSource source, ReportCreatorOptions options) : base(source, options)
        {

        }
        protected override StatisticsReport Construct()
        {
            List<Entry> newEntries = new List<Entry>();

            var splittedEntries = SplitPosts();
            Func<DateTime, bool> condition = EndCondition(); 
            
            DateTime fromDate = Options.EmptyPeriodsEnabled ? Options.DateRange.From : Posts.First().Date;
            while (true)
            {
                DateRange currentRange = GetRangeFor(fromDate);
                Entry entry = new Entry(newEntries.Count, currentRange, Options.Period.DateFormat, Options.TopicMode.Mode);
                entry.Set(WrittenPosts(splittedEntries, currentRange));
                newEntries.Add(entry);

                fromDate = currentRange.To;
                if (!condition.Invoke(currentRange.To))
                {
                    break;
                }
            }

            return new StatisticsReport($"Отчет по периодам", newEntries.ToArray(), Options);
        }

        private double WrittenPosts(IEnumerable<Entry> splittedEntries, DateRange currentRange)
        {
            EntryFraction[] innerEntries = splittedEntries
                .Where(e => e.Range.IsIntersectedWithRange(currentRange))
                .Select(e => new EntryFraction(currentRange, e)).ToArray();
            return innerEntries.Sum(Posts);


            double Posts(EntryFraction obj)
            {
                return obj.Entry.PostsWritten * obj.Fraction;
            }
        }

        private DateRange GetRangeFor(DateTime now)
        {
            DateTime next = now.NextDate(Options.Period);
            if (next >= Options.DateRange.To)
                return new DateRange(now, Options.DateRange.To);
            else
                return new DateRange(now, next);
        }
        private Func<DateTime, bool> EndCondition()
        {
            DateTime lastPostDate = Posts.Last().Date;
            if (Options.EmptyPeriodsEnabled)
                return (toDate) => Options.DateRange.To > toDate;
            else
                return (toDate) => toDate <= lastPostDate;
        }
    }

    public class EntryFraction
    {
        public readonly Entry Entry;
        public readonly double Fraction;

        public EntryFraction(DateRange source, Entry entry)
        {
            Entry = entry;
            Fraction = Entry.Range.GetFractionOfRange(source);
        }
    }
}
