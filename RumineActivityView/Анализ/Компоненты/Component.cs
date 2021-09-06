using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class StatComponent : ComponentBase
    {
        public StatApp App => StatApp.App;
        public IEnumerable<Post> Posts => App.Posts;
        public IEnumerable<Topic> Topics => App.Topics;
        public ViewOptions View => App.ViewOptions;

        public string FormatEntry(Entry entry)
        {
            double value = View.MeasureMethod.GetValue(entry);
            if (value == 0)
            {
                return "???";
            }
            else
            {
                value /= View.MeasureUnit.Value;
                string zeros = string.Join("", Enumerable.Repeat("0", View.RoundAccuracy));
                return value.ToString($"#,0.{zeros}");
            }
        }
    }
}
