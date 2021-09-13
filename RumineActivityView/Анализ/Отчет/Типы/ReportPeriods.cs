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

            Period period = Options.Period;
            DateRange dateRange = Options.DateRange;
            DateTime lastPostDate = Posts.Last().Date;
            var splittedEntries = SplitPosts();
            Func<DateTime, bool> condition = Options.EmptyPeriodsEnabled ? toDate => dateRange.IsDateInside(toDate) : toDate => toDate <= lastPostDate; 
            
            DateTime fromDate = Options.EmptyPeriodsEnabled ? dateRange.From : Posts.First().Date;
            DateTime toDate = new DateTime();
            do
            {
                toDate = period.GetNextDate(fromDate);
                DateRange range = new DateRange(fromDate, toDate);
                Entry entry = new Entry(newEntries.Count, range, period.DateFormat, Options.TopicMode.Mode);

                var innerRangeEntries = splittedEntries.Select(e => new EntryDateRangeFraction(range, e)).Where(f => f.Fraction > 0);
                double written = innerRangeEntries.Sum(PostsWritten);

                entry.Set(PostOutputs.PeriodDifference, written);
                newEntries.Add(entry);
                fromDate = toDate;
            }
            while (condition.Invoke(toDate));

            return new StatisticsReport($"Отчет по периодам", newEntries, Options);
        }

        private double PostsWritten(EntryDateRangeFraction obj)
        {
            Entry e = obj.Entry;
            double fraction = obj.Fraction;

            if (fraction == 1)
                return e.PostsDefault;
            else
                return e.PostsDefault * fraction;
        }
    }

    public class EntryDateRangeFraction
    {
        public readonly DateRange Source;
        public readonly Entry Entry;
        public readonly double Fraction;

        public EntryDateRangeFraction(DateRange source, Entry entry)
        {
            Source = source;
            Entry = entry;
            Fraction = Entry.Range.GetFraction(source);
        }
    }
}
