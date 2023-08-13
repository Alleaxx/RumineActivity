using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    public enum PostOutputs
    {
        PeriodEnd, PeriodDifference
    }
    public class OutputValue : EnumType<PostOutputs>
    {
        public OutputValue(PostOutputs type) : base(type)
        {
            switch (type)
            {
                case PostOutputs.PeriodDifference:
                    Name = "В промежутке";
                    break;
                case PostOutputs.PeriodEnd:
                    Name = "К моменту";
                    break;
            }
        }
    }
}
