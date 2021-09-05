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
    public enum MeasureMethods
    {
        Total, ByHour, ByDay, ByMonth,
    }
    public class MeasureUnit
    {
        public MeasureUnits Unit { get; private set; }
        public string Name { get; private set; }
        public int Value { get; private set; }

        public MeasureUnit(MeasureUnits unit)
        {
            Unit = unit;
            switch (Unit)
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
    public class MeasureMethod
    {
        public MeasureMethods Method { get; private set; }
        public string Name { get; private set; }
        public MeasureMethod(MeasureMethods method)
        {
            Method = method;
            switch (Method)
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
            switch (Method)
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
