using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
namespace RumineActivity.View
{
    public class ReportComponent : StatComponent
    {
        [Parameter]
        public StatisticsReport Report { get; set; }
    }
}
