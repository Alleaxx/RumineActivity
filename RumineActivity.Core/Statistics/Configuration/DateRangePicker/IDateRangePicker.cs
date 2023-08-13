using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public interface IDateRangePicker
    {
        event Action<IDateRangePicker> OnDateRangeUpdated;
        DateRange TryCreateDateRange();
    }
}
