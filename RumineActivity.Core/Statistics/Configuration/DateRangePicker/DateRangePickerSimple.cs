using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class DateRangePickerSimple : IDateRangePicker
    {
        public event Action<IDateRangePicker> OnDateRangeUpdated;

        private static DateTime GetMaxDate()
        {
            return new DateTime(DateTime.Now.Year, 12, 31);
        }

        public DateTime DateFrom
        {
            get => dateFrom;
            set
            {
                if (value < RumineValues.FoundationDate)
                {
                    dateFrom = RumineValues.FoundationDate;
                }
                else if (value > GetMaxDate())
                {
                    dateFrom = GetMaxDate();
                }
                else
                {
                    dateFrom = value;
                }
                OnDateRangeUpdated?.Invoke(this);
            }
        }
        private DateTime dateFrom;
        public DateTime DateTo
        {
            get => dateTo;
            set
            {
                if (value < RumineValues.FoundationDate)
                {
                    dateTo = RumineValues.FoundationDate;
                }
                else if (value > GetMaxDate())
                {
                    dateTo = GetMaxDate();
                }
                else
                {
                    dateTo = value;
                }
                OnDateRangeUpdated?.Invoke(this);
            }
        }
        private DateTime dateTo;

        public DateRangePickerSimple(DateRange range)
        {
            DateFrom = range.From;
            DateTo = range.To;
        }


        public DateRange TryCreateDateRange()
        {
            return new DateRange(DateFrom, DateTo);
        }
    }
}
