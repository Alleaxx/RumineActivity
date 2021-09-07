using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public interface ISelect
    {

    }
    public class ListOptions
    {
        public IEnumerable<ISelect> Items { get; private set; }
        public ISelect Selected { get; set; }
        public ListOptions(IEnumerable<ISelect> collection)
        {
            Items = collection;
            Selected = collection.FirstOrDefault();
        }
    }
    //public class CollectionAdv<T> : ListOptions where T:ISelect
    //{
    //    public new IEnumerable<T> Items => base.Items.OfType<T>();
    //    public new T Selected => (T)base.Selected;

    //    public CollectionAdv(IEnumerable<T> ts)
    //    {

    //    }
    //}
}
