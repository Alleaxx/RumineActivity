using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Отчет об активности. Коллекция записей 'За период написано сообщений'
    public class StatisticsReport
    {
        //Данные
        public string Name { get; private set; }
        public IEnumerable<Entry> Entries { get; private set; }

        //Статистика по данным
        public bool IsEmpty => Entries == null || !Entries.Any();
        public int Count => Entries.Count();
        public double SumValue => IsEmpty ? 0 : Entries.Sum(e => e.Value);
        public double AverageValue => IsEmpty ? 0 : Entries.Average(e => e.Value);
        public Entry MostInactive => Entries.Where(e => e.ValueRelative > 0).OrderBy(e => e.ValueRelative).FirstOrDefault();
        public Entry MostActive => Entries.Where(e => e.ValueRelative > 0).OrderBy(e => e.ValueRelative).LastOrDefault();

        public StatisticsReport() : this("Нулевой отчет", Array.Empty<Entry>())
        {

        }
        public StatisticsReport(string name, IEnumerable<Entry> entries)
        {
            Name = name;
            Entries = entries;
        }
    }
}




