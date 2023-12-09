using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class DateRangePickerAdvanced : IDateRangePicker
    {
        public event Action<IDateRangePicker> OnDateRangeUpdated;

        public IReadOnlyCollection<DateRangeCell> Periods => Cells;
        private List<DateRangeCell> Cells { get; set; }
        public DateRangeCell FirstCell { get; private set; }
        public DateRangeCell LastCell { get; private set; }

        public DateRangePickerAdvanced()
        {
            CreatePeriodCells();
            PickFirstCell(Cells.First());
            PickLastCell(Cells.Last());
        }
        public DateRangePickerAdvanced(DateRange dateRange)
        {
            CreatePeriodCells();
            PickCellsFromRange(dateRange);
        }

        private void CreatePeriodCells()
        {
            Cells = new List<DateRangeCell>();

            var startDate = RumineValues.FoundationDate;
            var endDate = DateTime.Now;
            var years = Enumerable.Range(startDate.Year, endDate.Year - startDate.Year + 1);
            foreach (var year in years)
            {
                for (int month = 1; month < 13; month++)
                {
                    var newCell = new DateRangeCell(year, month);
                    newCell.OnDateRangeUpdated += Cell_OnDateRangeUpdated;
                    Cells.Add(newCell);
                }
            }
        }
        private void Cell_OnDateRangeUpdated(DateRangeCell obj)
        {
            OnDateRangeUpdated?.Invoke(this);
        }

        private void PickCellsFromRange(DateRange range)
        {
            var dateTo = range.To.AddDays(-1);
            var firstCell = Cells.FirstOrDefault(c => c.Year == range.From.Year && c.Month == range.From.Month);
            var lastCell = Cells.FirstOrDefault(c => c.Year == dateTo.Year && c.Month == dateTo.Month);
            if(firstCell != null)
            {
                FirstCell = firstCell;
            }
            if(lastCell != null)
            {
                LastCell = lastCell;
            }
            UpdateAllCells();
            if (firstCell != null)
            {
                firstCell.DayFrom = range.From.Day;
            }
            if (lastCell != null)
            {
                lastCell.DayTo = range.To.Day;
            }
            OnDateRangeUpdated?.Invoke(this);
        }

        public void PickCells(IEnumerable<DateRangeCell> cells)
        {
            var sortedCells = cells.Where(c => c.DateTo > RumineValues.FoundationDate).OrderBy(c => c.DateFrom);
            FirstCell = sortedCells.FirstOrDefault();
            LastCell = sortedCells.LastOrDefault();
            UpdateAllCells();
            OnDateRangeUpdated?.Invoke(this);
        }
        public void PickFirstCell(DateRangeCell cell)
        {
            CheckCellsSelection(cell);
            if (cell.DateFrom <= LastCell.DateFrom)
            {
                FirstCell = cell;
            }
            else if (FirstCell == LastCell)
            {
                FirstCell = cell;
                LastCell = cell;
            }
            else
            {
                FirstCell = cell;
                LastCell = cell;
            }
            UpdateAllCells();
            OnDateRangeUpdated?.Invoke(this);
        }
        public void PickLastCell(DateRangeCell cell)
        {
            CheckCellsSelection(cell);
            if(cell.DateFrom >= FirstCell.DateFrom)
            {
                LastCell = cell;
            }
            else if(FirstCell == LastCell)
            {
                FirstCell = cell;
                LastCell = cell;
            }
            else
            {
                FirstCell = cell;
                LastCell = cell;
            }
            UpdateAllCells();
            OnDateRangeUpdated?.Invoke(this);
        }
        private void CheckCellsSelection(DateRangeCell cell)
        {
            if(FirstCell == null || LastCell == null)
            {
                FirstCell = cell;
                LastCell = cell;
            }
        }
        
        private void UpdateAllCells()
        {
            foreach (var cell in Cells)
            {
                cell.UpdateCell(FirstCell, LastCell);
            }
        }


        public DateRange TryCreateDateRange()
        {
            if(FirstCell == null || LastCell == null)
            {
                return default;
            }
            if(FirstCell == LastCell)
            {
                return new DateRange(FirstCell.DateFrom, FirstCell.DateTo.AddDays(1));
            }
            else
            {
                return new DateRange(FirstCell.DateFrom, LastCell.DateTo.AddDays(1));
            }
        }
    }
}
