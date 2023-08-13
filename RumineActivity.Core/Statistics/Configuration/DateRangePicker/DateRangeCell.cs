using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class DateRangeCell
    {
        public event Action<DateRangeCell> OnDateRangeUpdated;

        public DateRange DateRange { get; private set; }
        public DateTime DateFrom => DateRange.From;
        public DateTime DateTo => DateRange.To;


        public int DayFrom
        {
            get => DateFrom.Day;
            set
            {
                if (value == DateFrom.Day || value > DayTo)
                {
                    return;
                }

                var newFromDate = new DateTime(DateFrom.Year, DateFrom.Month, value);
                var newToDate = DateTo;
                DateRange = new DateRange(newFromDate, newToDate);
                OnDateRangeUpdated?.Invoke(this);
            }
        }
        public int DayTo
        {
            get => DateTo.Day;
            set
            {
                if (value == DateTo.Day || value < DayFrom)
                {
                    return;
                }

                var newFromDate = DateFrom;
                var newToDate = new DateTime(DateTo.Year, DateTo.Month, value);
                DateRange = new DateRange(newFromDate, newToDate);
                OnDateRangeUpdated?.Invoke(this);
            }
        }

        public int Year => DateFrom.Year;
        public int Month => DateFrom.Month;

        public string GetName()
        {
            return $"{Month.GetMonthName("MMM")}-{DateFrom:yy}";
        }

        public bool IsBordered => IsFirstPicked || IsLastPicked;
        public bool IsSingle => IsFirstPicked && IsLastPicked;
        public bool IsFirstPicked { get; private set; }
        public bool IsLastPicked { get; private set; }
        public bool IsIncluded { get; private set; }

        public DateRangeCell(int year, int month)
        {
            DateRange = new DateRange(year, month);
            DayFrom = 1;
            DayTo = DateTime.DaysInMonth(year, month);
        }

        public void UpdateCell(DateRangeCell first, DateRangeCell last)
        {
            IsFirstPicked = first == this;
            IsLastPicked = last == this;
            IsIncluded = DateFrom >= first.DateFrom && DateFrom <= last.DateTo;
            DayFrom = 1;
            DayTo = DateTime.DaysInMonth(Year, Month);
            if(DateFrom < RumineValues.FoundationDate)
            {
                DayFrom = RumineValues.FoundationDate.Day;
            }
        }

        public IEnumerable<(int day, string text)> GetAvailableDays(bool last)
        {
            List<(int day, string text)> daysList = new List<(int day, string text)>();
            int daysInMonth = DateTime.DaysInMonth(Year, Month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                if(DateFrom.Month == 7 && DateFrom.Year == 2011 && i < 27)
                {
                    continue;
                }
                daysList.Add((i, i.ToString()));
            }

            return daysList;
        }
    }
}
