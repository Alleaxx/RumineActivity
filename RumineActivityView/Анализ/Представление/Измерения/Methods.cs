using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum MeasureMethods
    {
        Total, ByHour, ByDay, ByMonth,
    }
    public class MeasureMethod : EnumType<MeasureMethods>
    {
        public MeasureMethod(MeasureMethods method) : base(method)
        {
            switch (Type)
            {
                case MeasureMethods.ByDay:
                    Name = "В среднем в день";
                    break;
                case MeasureMethods.ByHour:
                    Name = "В среднем в час";
                    break;
                case MeasureMethods.ByMonth:
                    Name = "В среднем за месяц";
                    break;
                case MeasureMethods.Total:
                    Name = "Всего за период";
                    break;
            }
        }
    }

}
