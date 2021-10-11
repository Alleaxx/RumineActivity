using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class StatComponent : ComponentBase
    {
        [Inject]
        public StatApp App { get; set; }


        public IEnumerable<Post> Posts => App.Posts;
        public IEnumerable<Topic> Topics => App.Topics;
        public ViewOptions View => App.ViewOptions;




        const string SelectedClass = "selected";
        const string SelectableClass = "selectable";
        protected string CssSelect<T>(T a, T to) => a.Equals(to) ? SelectedClass : SelectableClass;
        protected string CssSelect<T>(T a, T to, params string[] classes) => a.Equals(to) ? $"{string.Join(" ", classes)} {SelectedClass}" : $"{string.Join(" ", classes)} {SelectableClass}";    
    }
}
