using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
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
                    Name = "Старые страницы";
                    Value = 10;
                    break;
            }
        }
    }
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

        public double GetValue(Entry entry)
        {
            double value = entry.Value;
            switch (Type)
            {
                case MeasureMethods.ByDay:
                    return entry.ValuePerDay;
                case MeasureMethods.ByHour:
                    return entry.ValuePerHour;
                case MeasureMethods.ByMonth:
                    return entry.ValuePerMonth;
                default:
                    return value;
            }
        }
    }
}
