using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    public enum MaxValues
    {
        Relative,
        Max1_5,
        Max5,
        Max15,
        Max30,
        Max75,
        Max150,
    }
    public class MaxValue : EnumType<MaxValues>
    {
        public double Value { get; init; }

        public MaxValue(MaxValues key) : base(key)
        {
            switch (key)
            {
                case MaxValues.Relative:
                    Name = "Текущий + 5%";
                    Value = 0;
                    break;
                case MaxValues.Max1_5:
                    Name = "1.5 стр в день";
                    Value = 1.5;
                    break;
                case MaxValues.Max5:
                    Name = "5 стр в день";
                    Value = 5;
                    break;
                case MaxValues.Max15:
                    Name = "15 стр в день";
                    Value = 15;
                    break;
                case MaxValues.Max30:
                    Name = "30 стр в день";
                    Value = 30;
                    break;
                case MaxValues.Max75:
                    Name = "75 стр в день";
                    Value = 75;
                    break;
                case MaxValues.Max150:
                    Name = "150 стр в день";
                    Value = 150;
                    break;
            }
        }
    }
}
