using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public enum CompareValues
    {
        Relative, Absolute
    }
    public class CompareValue : EnumType<CompareValues>
    {
        public CompareValue(CompareValues values) : base(values)
        {
            switch (values)
            {
                case CompareValues.Absolute:
                    Name = "Значения";
                    break;
                case CompareValues.Relative:
                    Name = "Проценты";
                    break;
            }
        }
    }
}
