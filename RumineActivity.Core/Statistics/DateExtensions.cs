using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public static class DateExtensions
    {
        private static string[] MonthNamesShort = new[]
        {
            "янв",
            "фев",
            "мар",
            "апр",
            "май",
            "июн",
            "июл",
            "авг",
            "сен",
            "окт",
            "ноя",
            "дек",
        };

        /// <summary>
        /// Получить имя месяца в указанном формате
        /// </summary>
        public static string GetMonthName(this int month, string format = "MMMM")
        {
            try
            {
                if(format == "MMM")
                {
                    return MonthNamesShort[month-1];
                }

                return new DateTime(2011, month, 1).ToString(format);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Попытка сделать имя для месяца: {month}");
                Console.WriteLine(ex.Message);
                return default;
            }
        }

        /// <summary>
        /// Определить номер недели для дня указанного месяца
        /// </summary>
        public static int DefineDayYearWeekIndex(DateTime date)
        {
            var days = DateTime.IsLeapYear(date.Year) ? 366 : 365;
            var firstDay = DefineFirstDayOfFirstWeek(date.Year);
            if(date.DayOfYear < firstDay)
            {
                return 0;
            }
            else
            {
                int week = 1;
                int weekCounter = 0;
                for (int i = firstDay; i <= days; i++)
                {
                    if(i == date.DayOfYear)
                    {
                        return week;
                    }
                    weekCounter++;
                    if(weekCounter == 7)
                    {
                        week++;
                        weekCounter = 0;
                    }
                }
            }
            return -1;
        }
        
        /// <summary>
        /// Определить номер недели для дня указанного месяца
        /// </summary>
        public static int DefineDayMonthWeekIndex(DateTime date)
        {
            return DefineDayMonthWeekIndex(date, date.Day);
        }
        
        /// <summary>
        /// Определить номер недели для дня указанного месяца
        /// </summary>
        public static int DefineDayMonthWeekIndex(DateTime date, int dayOfMonth)
        {
            var firstDay = new DateTime(date.Year, date.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            int firstDayType = Convert.ToInt32(firstDay.DayOfWeek);
            if (firstDayType == 0)
            {
                firstDayType = 7;
            }

            var weekCounter = 1;
            var dayTypeIndex = firstDayType;
            for (int dayIndex = 1; dayIndex <= daysInMonth; dayIndex++)
            {
                if (dayIndex == dayOfMonth)
                {
                    return weekCounter;
                }

                if (dayTypeIndex == 7)
                {
                    weekCounter++;
                    dayTypeIndex = 1;
                }
                else
                {
                    dayTypeIndex++;
                }
            }
            return 1;
        }

        private static int DefineFirstDayOfFirstWeek(int year)
        {
            DateTime test = new DateTime(year, 1, 1);
            switch (test.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    return test.Day;
                case DayOfWeek.Tuesday:
                    return test.Day;
                case DayOfWeek.Wednesday:
                    return test.Day;
                case DayOfWeek.Thursday:
                    return test.Day;
                case DayOfWeek.Friday:
                    return 4;
                case DayOfWeek.Saturday:
                    return 3;
                case DayOfWeek.Sunday:
                    return 2;
            }
            return 1;
        }

        /// <summary>
        /// Определить квартал даты
        /// </summary>
        public static int DefineQuarter(DateTime date)
        {
            switch (date.Month)
            {
                case 1:
                case 2:
                case 3:
                    return 1;
                case 4:
                case 5:
                case 6:
                    return 2;
                case 7:
                case 8:
                case 9:
                    return 3;
                case 10:
                case 11:
                case 12:
                    return 4;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// Определить полугодие даты
        /// </summary>
        public static int DefineHalfYear(DateTime date)
        {
            switch (date.Month)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    return 1;
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                    return 2;
            }
            return 0;
        }

        /// <summary>
        /// Определить сезон даты
        /// </summary>
        public static int DefineSeason(DateTime date)
        {
            switch (date.Month)
            {
                case 1:
                case 2:
                    return 1;
                case 3:
                case 4:
                case 5:
                    return 2;
                case 6:
                case 7:
                case 8:
                    return 3;
                case 9:
                case 10:
                case 11:
                    return 4;
                case 12:
                    return 5;
            }
            return 0;
        }

        public static string QuarterToSymbol(int quarter)
        {
            switch (quarter)
            {
                case 1:
                    return "I";
                case 2:
                    return "II";
                case 3:
                    return "III";
                case 4:
                    return "IV";
                default:
                    return string.Empty;
            }
        }
        public static string SeasonToString(int season)
        {
            switch (season)
            {
                case 1:
                    return "Зима";
                case 2:
                    return "Весна";
                case 3:
                    return "Лето";
                case 4:
                    return "Осень";
                case 5:
                    return "Конец";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Определить символьное представление квартала для даты
        /// </summary>
        public static string DefineQuarterSymbol(DateTime date)
        {
            return QuarterToSymbol(DefineQuarter(date));
        }
        /// <summary>
        /// Определить символьное представление квартала для даты
        /// </summary>
        public static string DefineSeasonText(DateTime date)
        {
            var year = date;
            if(date.Month == 12)
            {
                year = year.AddYears(1); 
            }
            switch (date.Month)
            {
                case 12:
                case 1:
                case 2:
                    return $"Зима {year:yy}";
                case 3:
                case 4:
                case 5:
                    return $"Весна {date:yy}";
                case 6:
                case 7:
                case 8:
                    return $"Лето {date:yy}";
                case 9:
                case 10:
                case 11:
                    return $"Осень {date:yy}";
            }
            return string.Empty;
        }

        /// <summary>
        /// Определить треть месяца для даты
        /// </summary>
        public static string DefineDayMonthPartString(DateTime date)
        {
            if (date.Day / 10 < 1)
            {
                return "01-09";
            }
            else if (date.Day / 10 < 2)
            {
                return "10-19";
            }
            else
            {
                return "20-31";
            }
        }


        /// <summary>
        /// Отформатировать время как разницу
        /// </summary>
        public static string GetFullDifferenceName(this TimeSpan timeSpan)
        {
            if(timeSpan.TotalMinutes < 60)
            {
                return $"{timeSpan.TotalMinutes:#,0} мин.";
            }
            else if(timeSpan.TotalHours < 24)
            {
                return $"{timeSpan.TotalHours:#,0} час.";
            }
            else
            {
                return $"{timeSpan.TotalDays:#,0} дн.";
            }
        }

    }
}
