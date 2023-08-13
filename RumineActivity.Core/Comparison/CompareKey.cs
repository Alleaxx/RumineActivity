using RumineActivity.Core.Comparisons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Comparisons
{
    public abstract class CompareKey<T>
    {
        public string Title { get; protected set; }
        protected Func<T, ICompareProp> GetCompareFunc;

        protected CompareKey()
        {
                
        }

        public IEnumerable<ICompareProp> GetCompares(IEnumerable<T> objects)
        {
            return objects.Select(o => GetCompareFunc(o));
        }
    }
    public class CompareKeyReport : CompareKey<StatisticsReport>
    {
        public CompareKeyReport(string title, Func<StatisticsReport, ICompareProp> func)
        {
            Title = title;
            GetCompareFunc = func;
        }
    }
    public class CompareKeyEntry : CompareKey<Entry>
    {
        public CompareKeyEntry(string title, Func<Entry, ICompareProp> func)
        {
            Title = title;
            GetCompareFunc = func;
        }
    }
}
