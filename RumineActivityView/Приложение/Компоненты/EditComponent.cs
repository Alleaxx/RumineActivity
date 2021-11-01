using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
namespace RumineActivity.View
{
    public class EditComponent : ComponentBase
    {
        //[Inject]
        //public IDataEditor App { get; set; }
        [Inject]
        public IPostEditor EditPost { get; set; }
        [Inject]
        public IActivityApi Api { get; set; }

        public IForum Forum { get; private set; }
        protected async override Task OnInitializedAsync()
        {
            Forum = await Api.GetForum();
        }
    }
}
