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


    }
}
