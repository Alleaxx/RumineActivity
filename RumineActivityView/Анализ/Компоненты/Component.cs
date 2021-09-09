﻿using Microsoft.AspNetCore.Components;
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
            PostsType postsType = View.OutValue.Type;
            MeasureMethods method = View.MeasureMethod.Type;

            double value = entry.GetPosts(postsType, method);

            if(postsType == PostsType.Total)
            {
                return entry.GetPosts(postsType, MeasureMethods.Total).ToString("#,0");
            }
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
