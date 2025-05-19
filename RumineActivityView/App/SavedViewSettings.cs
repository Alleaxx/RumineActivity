using RumineActivity.Core.Comparisons;
using RumineActivity.Core.Measures;

namespace RumineActivity.View
{
    /// <summary>
    /// Модель сохраненных настроек в LocalStorage
    /// </summary>
    public class SavedViewSettings
    {
        public int RoundAccuracy { get; set; }
        public MeasureUnits MeasureUnit { get; set; }
        public MeasureMethods MeasureMethod { get; set; }
        public MaxValues MaxValue { get; set; }
        public bool ShowCompareValueDifference { get; set; }
        public DisplayTypes DisplayType { get; set; }
        public Sortings SortingEntriesSelected { get; set; }
        public CompareDiffFormats CompareView { get; set; }
        public bool SortingEntriesDescending { get; set; }

        public SavedViewSettings()
        {
            RoundAccuracy = 1;
            MeasureUnit = MeasureUnits.Pages;
            MeasureMethod = MeasureMethods.ByDay;
            MaxValue = MaxValues.Relative;
            ShowCompareValueDifference = true;
            DisplayType = DisplayTypes.Histogram;
            SortingEntriesSelected = Sortings.Index;
            CompareView = CompareDiffFormats.Absolute;
            SortingEntriesDescending = false;
        }
    }
}
