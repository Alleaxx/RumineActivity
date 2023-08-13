using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Statistics
{
    public enum IntervalTypes
    {
        Closed,
        Opened,
        HalfStart,
        HalfEnd,
        //включает в себя обе границы - отрезок / закрытый числовой промежуток
        //не включает в себя обе границы - интервал / открытый числовой промежуток
        //включает 1-ю или второй - полузамкнутый промежуток (полуинтервал)
        //

    }
    public class IntervalType : EnumType<IntervalTypes>
    {
        public IntervalType(IntervalTypes type) : base(type)
        {
                
        }
    }
}
