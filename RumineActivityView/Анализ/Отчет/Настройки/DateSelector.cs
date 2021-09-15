using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    //Указатель на год и месяц
    public class YearMonthDateSelector
    {
        public int Year { get; set; } = 0;
        public int Month { get; set; } = 0;
        public (int from, int to) Day { get; set; } = (0, 0);

        public DateRange Dates
        {
            get
            {
                if (Year == 0)
                {
                    return FullHistory();
                }
                else if (Year != 0 && Month == 0)
                {
                    return YearHistory();
                }
                else if (Year != 0 && Month != 0)
                {
                    return MonthHistory();
                }

                return OwnHistory();
            }
        }
        private static DateRange FullHistory()
        {
            return new DateRange(ReportCreatorOptions.FoundationDate, DateTime.Now);
        }
        private DateRange YearHistory()
        {
            return new DateRange(new DateTime(Year, 1, 1), new DateTime(Year, 12, 31));
        }
        private DateRange MonthHistory()
        {
            return new DateRange(new DateTime(Year, Month, 1), new DateTime(Year, Month, 1).AddMonths(1));
        }
        private DateRange OwnHistory()
        {
            return new DateRange(new DateTime(Year, Month, Day.from), new DateTime(Year, Month, Day.to));
        }


        public YearMonthDateSelector(int year, int month)
        {
            Year = year;
            Month = month;
        }
    }
}
