using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class EditComponent : ComponentBase
    {
        [Inject]
        public EditApp App { get; set; }

        public IEnumerable<Post> Posts => App.Posts;
        public IEnumerable<Topic> Topics => App.Topics;
    }
}
