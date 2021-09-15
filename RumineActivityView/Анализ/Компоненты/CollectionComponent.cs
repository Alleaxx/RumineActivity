using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class CollectionComponent<T> : ComponentBase
    {
        [Parameter]
        public bool Nullable { get; set; }
        [Parameter]
        public string HeaderText { get; set; }
        [Parameter]
        public IEnumerable<T> List { get; set; }
        [Parameter]
        public T Selected { get; set; }
        [Parameter]
        public EventCallback<T> OnSelectCallback { get; set; }

        protected override void OnInitialized()
        {
            index = List.ToList().IndexOf(Selected);
        }


        protected void Set(T select)
        {
            Selected = select;
            OnSelectCallback.InvokeAsync(select);
        }

        protected string Item
        {
            get => index.ToString();
            set
            {
                index = int.Parse(value);
                Set(List.ElementAt(index));
            }
        }
        private int index { get; set; } = 0;

        protected string GetCssClass(T select)
        {
            if (select != null)
            {
                return select.Equals(Selected) ? "selected" : "";
            }
            return "";
        }
    }
}
