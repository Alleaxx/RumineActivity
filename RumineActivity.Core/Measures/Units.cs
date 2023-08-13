using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    //Методы и единицы измерения
    public enum MeasureUnits
    {
        Messages, Pages, OldPages
    }
    public class MeasureUnit : EnumType<MeasureUnits>
    {
        public int Value { get; private set; }

        public MeasureUnit(MeasureUnits unit) : base(unit)
        {
            switch (Type)
            {
                case MeasureUnits.Messages:
                    Name = "Сообщения";
                    Value = 1;
                    break;
                case MeasureUnits.Pages:
                    Name = "Страницы";
                    Value = 20;
                    break;
                case MeasureUnits.OldPages:
                    Name = "Страницы (10)";
                    Value = 10;
                    break;
            }
        }

        public double GetValue(double val)
        {
            return val / Value;
        }
    }

}
