using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
namespace RumineActivity.View
{
    public class StatComponent : ComponentBase
    {
        [Inject]
        public IStatApp App { get; set; }
        [Inject]
        public IReportsFactory ReportsFactory { get; set; }
        [Inject]
        public IActivityApi API { get; set; }

        public ViewOptions View => App.ViewOptions;




        const string SelectedClass = "selected";
        const string SelectableClass = "selectable";
        protected string CssSelect<T>(T a, T to) => a.Equals(to) ? SelectedClass : SelectableClass;
        protected string CssSelect<T>(T a, T to, params string[] classes) => a.Equals(to) ? $"{string.Join(" ", classes)} {SelectedClass}" : $"{string.Join(" ", classes)} {SelectableClass}";

        public static ViewType[] DataViews { get; private set; } = Enum.GetValues<DViewTypes>().Select(d => new ViewType(d)).ToArray();
    }
}
