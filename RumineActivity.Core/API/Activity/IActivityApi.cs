﻿using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.API
{
    public interface IActivityApi
    {
        event Action OnLoaded;
        bool? IsLoaded { get; }
        Task LoadDataAsync();

        IForum GetForum();
        IForum GetForum(DateRange range);
    }
}
