using RumineActivity.Core.Comparisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Comparisons
{
    public class ComparisonReports
    {
        public IEnumerable<StatisticsReport> ReportForCompare => ReportsList;
        private List<StatisticsReport> ReportsList { get; init; }
        public StatisticsReport MainReport { get; init; }
        
        public ComparisonReports(StatisticsReport main, IEnumerable<StatisticsReport> reportsForCompare)
        {
            ReportsList = new List<StatisticsReport>(reportsForCompare.Take(30).OrderBy(r => r.DateRangeAll.From));
            MainReport = main;
            CreateCompareProperties();
        }


        //Создание и заполнение конкретных свойств для отчетов и записей
        private Dictionary<CompareKeyReport, IEnumerable<ICompareProp>> PropertiesDictionary { get; set; }
        private void CreateCompareProperties()
        {
            PropertiesDictionary = new Dictionary<CompareKeyReport, IEnumerable<ICompareProp>>();

            foreach (var key in CompareKeyReports)
            {
                PropertiesDictionary.Add(key, key.GetCompares(ReportsList).ToArray());
            }
            foreach (var compare in PropertiesDictionary)
            {
                var mainElementCompare = compare.Value.FirstOrDefault(c => c.Source == MainReport);
                foreach (var property in compare.Value)
                {
                    property.SetValueCompareWith(mainElementCompare);
                }
            }
        }
        private Dictionary<CompareKeyEntry, IEnumerable<ICompareProp>> GetPropertiesEntryDictionary(int index)
        {
            var dictionary = new Dictionary<CompareKeyEntry, IEnumerable<ICompareProp>>();
            var entries = ReportForCompare.Select(r => r.Entries.FirstOrDefault(e => e.Index == index)).Where(e => e != null);


            foreach (var key in CompareKeyEntries)
            {
                dictionary.Add(key, key.GetCompares(entries).ToArray());
            }
            foreach (var pairPropertyKeyCompare in dictionary)
            {
                var mainElement = pairPropertyKeyCompare.Value.FirstOrDefault(k => MainReport.Entries.Contains(k.Source));
                foreach (var entryCompare in pairPropertyKeyCompare.Value)
                {
                    entryCompare.SetValueCompareWith(mainElement);
                }
            }

            return dictionary;
        }

        //Получение конкретных свойств
        public IEnumerable<ICompareProp> GetPropertiesByReport(StatisticsReport report)
        {
            return PropertiesDictionary.Values.SelectMany(p => p).Where(v => v.Source == report).ToArray();
        }
        public IEnumerable<ICompareProp> GetPropertiesByReportEntries(StatisticsReport report, int index)
        {
            return GetPropertiesEntryDictionary(index).Values.SelectMany(p => p).Where(v => report.Entries.Contains(v.Source)).ToArray();
        }

        //Создание свойств сравнения
        static ComparisonReports()
        {
            //Отчеты
            var keyRangeReport = new CompareKeyReport("Временные рамки", report => new CompareProperty<DateRange>(report, report.DateRangeAll));
            var keyWrittenTotalReport = new CompareKeyReport("Постов всего", report => new DoubleProperty(report, report.GetTotalValue(Measures.MeasureUnits.Messages), sum => sum.ToString("#,0")));
            var keyWrittenPagesTotalReport = new CompareKeyReport("Страниц всего", report => new DoubleProperty(report, Math.Floor(report.GetTotalValue(Measures.MeasureUnits.Pages)), sum => sum.ToString("#,0")));
            var keyWrittenPostsAverageReport = new CompareKeyReport("~ Постов в день", report => new DoubleProperty(report, report.GetAverageDayValue(Measures.MeasureUnits.Messages), sum => sum.ToString("#,0.0")));
            var keyWrittenPagesAverageReport = new CompareKeyReport("~ Страниц в день", report => new DoubleProperty(report, report.GetAverageDayValue(Measures.MeasureUnits.Pages), sum => sum.ToString("#,0.0")));
            var keyPeriodReport = new CompareKeyReport("Периодичность", report => new CompareProperty<Period>(report, report.Period));
            var keyDurationReport = new CompareKeyReport("Длительность", report => new TimeSpanProperty(report, report.DateRangeAll.Diff, time => $"{time.TotalDays:#,0} дней"));
            var keyEntriesReport = new CompareKeyReport("Записей", report => new DoubleProperty(report, report.Entries.Count));
            
            CompareKeyReports = new CompareKeyReport[]
            {
                //keyRange,
                keyWrittenTotalReport,
                keyWrittenPagesTotalReport,
                keyDurationReport,
                keyWrittenPostsAverageReport,
                keyWrittenPagesAverageReport,
                //keyPeriod,
                //keyEntries,
            };


            //Записи
            var keyRangeEntry = new CompareKeyEntry("Временные рамки", entry => new DateRangeProperty(entry, entry.Range, range => range.GetName()));
            var keyWrittenTotalEntry = new CompareKeyEntry("Постов всего", entry => new DoubleProperty(entry, entry.GetValueTotal(Measures.MeasureUnits.Messages), sum => sum.ToString("#,0")));
            var keyWrittenPagesTotalEntry = new CompareKeyEntry("Страниц всего", entry => new DoubleProperty(entry, entry.GetValueTotal(Measures.MeasureUnits.Pages), sum => sum.ToString("#,0")));
            var keyWrittenPostsAverageEntry = new CompareKeyEntry("~ Постов в день", entry => new DoubleProperty(entry, entry.GetValueDayAverage(Measures.MeasureUnits.Messages), sum => sum.ToString("#,0.0")));
            var keyWrittenPagesAverageEntry = new CompareKeyEntry("~ Страниц в день", entry => new DoubleProperty(entry, entry.GetValueDayAverage(Measures.MeasureUnits.Pages), sum => sum.ToString("#,0.0")));
            var keyPeriodEntry = new CompareKeyEntry("Периодичность", entry => new CompareProperty<Period>(entry, entry.Period));
            var keyDurationEntry = new CompareKeyEntry("Длительность", entry => new TimeSpanProperty(entry, entry.Range.Diff, time => $"{time.TotalDays:#,0} дней"));
            var keyFractionEntry = new CompareKeyEntry("% от целого", entry => new DoubleProperty(entry, entry.FractionMode, fraction => $"{fraction:0.0%}"));

            CompareKeyEntries = new CompareKeyEntry[]
            {
                keyRangeEntry,
                keyWrittenTotalEntry,
                keyWrittenPagesTotalEntry,
                keyDurationEntry,
                keyWrittenPostsAverageEntry,
                keyWrittenPagesAverageEntry,
                keyFractionEntry
                //keyPeriod,
                //keyEntries,
            };

        }
        public static IEnumerable<CompareKeyReport> CompareKeyReports { get; private set; }
        public static IEnumerable<CompareKeyEntry> CompareKeyEntries { get; private set; }

    }
}
