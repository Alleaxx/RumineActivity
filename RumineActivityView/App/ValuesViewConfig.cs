﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
using RumineActivity.Core.Comparisons;
using RumineActivity.Core.Measures;

namespace RumineActivity.View
{
    /// <summary>
    /// Настройка отображения и формата всех значений
    /// </summary>
    public class ValuesViewConfig
    {
        private readonly ILocalStorageService LocalStorageService;
        private readonly IJsonService JsonService;

        public event Action<ValuesViewConfig> OnValuesChanged;

        public ViewRules Rules { get; private set; }

        public int AbsoluteValue
        {
            get => absoluteValue;
            set
            {
                if (value >= 1 && value < RumineValues.MaximumMessagesPerDayAbsolute)
                {
                    absoluteValue = value;
                    OnValueChanged();
                }
            }
        }
        public int RoundAccuracy
        {
            get => roundAccuracy;
            set
            {
                if(value >= 0 && value < 5)
                {
                    roundAccuracy = value;
                    OnValueChanged();
                }
            }
        }
        public MeasureUnit MeasureUnit
        {
            get => measureUnit;
            set
            {
                measureUnit = value;
                OnValueChanged();
            }
        }
        public MeasureMethod MeasureMethod
        {
            get => measureMethod;
            set
            {
                measureMethod = value;
                OnValueChanged();
            }
        }
        public bool UseRelativeValues
        {
            get => useRelativeValues;
            set
            {
                useRelativeValues = value;
                OnValueChanged();
            }
        }
        public bool ShowCompareValueDifference
        {
            get => showCompareValueDifference;
            set
            {
                showCompareValueDifference = value;
                OnValueChanged();
            }
        }
        public DisplayType DisplayType
        {
            get => displayType;
            set
            {
                displayType = value;
                OnValueChanged();
            }
        }
        public CompareDiffFormat CompareView
        {
            get => compareView;
            set
            {
                compareView = value;
                OnValueChanged();
            }
        }
        public Sorting<Entry, object> SortingEntriesSelected
        {
            get => sortingEntriesSelected;
            set
            {
                if (sortingEntriesSelected != null)
                {
                    sortingEntriesSelected.OnSortDirectionChanged -= EntrySorting_OnSortDirectionChanged;
                }

                sortingEntriesSelected = value;
                OnValueChanged();

                if (sortingEntriesSelected != null)
                {
                    sortingEntriesSelected.OnSortDirectionChanged += EntrySorting_OnSortDirectionChanged;
                }


            }
        }
        private void EntrySorting_OnSortDirectionChanged()
        {
            OnValuesChanged?.Invoke(this);
        }

        private int roundAccuracy;
        private int absoluteValue;
        private MeasureUnit measureUnit;
        private MeasureMethod measureMethod;
        private bool useRelativeValues;
        private bool showCompareValueDifference;
        private DisplayType displayType;
        private CompareDiffFormat compareView;
        private Sorting<Entry, object> sortingEntriesSelected;

        public string FormatEntryPosts(Entry entry, MeasureMethods? methodParam = null, MeasureUnits? unitParam = null)
        {
            if (entry == null)
            {
                return "???";
            }
            var method = methodParam ?? MeasureMethod.Type;
            var unit = unitParam ?? MeasureUnit.Type;
            return FormatValue(entry.GetValue(method, unit), entry.Period, method, unit);
        }
        public string FormatValue(double value, Periods period, MeasureMethods? methodParam = null, MeasureUnits? unitParam = null, bool forceNoZeros = false)
        {
            var method = methodParam ?? MeasureMethod.Type;
            var unit = unitParam ?? MeasureUnit.Type;

            if (value == 0)
            {
                return "???";
            }

            string format = GetNumberFormatDouble();
            if (method == MeasureMethods.Total && unit == MeasureUnits.Messages)
            {
                format = GetNumberFormatInt();
            }
            if (method == MeasureMethods.Total && (unit == MeasureUnits.Pages || unit == MeasureUnits.OldPages))
            {
                return Math.Ceiling(value).ToString(GetNumberFormatInt());
            }
            if (method == MeasureMethods.ByDay && period == Periods.Day && (unit == MeasureUnits.Pages || unit == MeasureUnits.OldPages))
            {
                return Math.Ceiling(value).ToString(GetNumberFormatInt());
            }
            if (method == MeasureMethods.ByDay && unit == MeasureUnits.Messages && period == Periods.Day)
            {
                format = GetNumberFormatInt();
            }
            if (forceNoZeros)
            {
                format = GetNumberFormatInt();
            }

            return value.ToString(format);
        }
        

        private string GetZeroFormat()
        {
            return CreateZeroFormatString(RoundAccuracy);
        }
        private string CreateZeroFormatString(int amount)
        {
            return string.Join("", Enumerable.Repeat("0", amount));
        }
       
        private string GetNumberFormatDouble()
        {
            return $"#,0.{GetZeroFormat()}";
        }
        public string GetNumberFormatInt()
        {
            return $"#,0";
        }

        private async Task OnValueChanged()
        {
            await SaveSettings();
            OnValuesChanged?.Invoke(this);
        }

        public ValuesViewConfig(IJsonService jsonService, ILocalStorageService localStorage)
        {
            this.JsonService = jsonService;
            this.LocalStorageService = localStorage;
            Rules = new ViewRules();
            roundAccuracy = 2;
            measureUnit = new MeasureUnit(MeasureUnits.Pages);
            measureMethod = MeasureMethod.Create(MeasureMethods.ByDay);
            displayType = new DisplayType(DisplayTypes.Histogram);
            absoluteValue = 2500;
            useRelativeValues = true;
            showCompareValueDifference = true;
            compareView = CompareDiffFormat.Create(CompareDiffFormats.Absolute);
            sortingEntriesSelected = SortExtensions.EntrySorts.Values.FirstOrDefault();
            sortingEntriesSelected.OnSortDirectionChanged += EntrySorting_OnSortDirectionChanged;
        }
        private const string SettingsKey = "StatAppSettings";
        public async Task SaveSettings()
        {
            try
            {
                var settings = CreateSaveSettings();
                var jsonSettings = JsonService.ConvertToJson(settings);
                await LocalStorageService.SetPropAsync(SettingsKey, jsonSettings);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public async Task LoadSettings()
        {
            try
            {
                var settingsJson = await LocalStorageService.GetValueAsync(SettingsKey);
                if(string.IsNullOrEmpty(settingsJson))
                {
                    return;
                }
                var settings = JsonService.CreateFromJson<SavedViewSettings>(settingsJson);
                if(settings != null)
                {
                    ApplySavedSettings(settings);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private SavedViewSettings CreateSaveSettings()
        {
            return new SavedViewSettings()
            {
                RoundAccuracy = this.roundAccuracy,
                AbsoluteValue = this.absoluteValue,
                UseRelativeValues = this.UseRelativeValues,
                ShowCompareValueDifference = this.ShowCompareValueDifference,
                CompareView = this.CompareView,
                MeasureUnit = this.measureUnit,
                MeasureMethod = this.measureMethod,
                DisplayType = this.displayType,
                SortingEntriesSelected = this.SortingEntriesSelected.Key,
                SortingEntriesDescending = this.sortingEntriesSelected.Descending
            };
        }
        private void ApplySavedSettings(SavedViewSettings savedSettings)
        {
            roundAccuracy = savedSettings.RoundAccuracy;
            absoluteValue = savedSettings.AbsoluteValue;
            useRelativeValues = savedSettings.UseRelativeValues;
            showCompareValueDifference = savedSettings.ShowCompareValueDifference;

            //sortingEntriesSelected = SortExtensions.EntrySorts[savedSettings.SortingEntriesSelected];
            //sortingEntriesSelected.Descending = savedSettings.SortingEntriesDescending;
            compareView = CompareDiffFormat.Create(savedSettings.CompareView);
            measureMethod = EnumValues.Methods.FirstOrDefault(m => m.Type == savedSettings.MeasureMethod);
            measureUnit = EnumValues.Units.FirstOrDefault(m => m.Type == savedSettings.MeasureUnit);
            displayType = new DisplayType(savedSettings.DisplayType);
            OnValuesChanged?.Invoke(this);
        }
    }
    public class SavedViewSettings
    {
        public int RoundAccuracy { get; set; }
        public int AbsoluteValue { get; set; }
        public MeasureUnits MeasureUnit { get; set; }
        public MeasureMethods MeasureMethod { get; set; }
        public bool UseRelativeValues { get; set; }
        public bool ShowCompareValueDifference { get; set; }
        public DisplayTypes DisplayType { get; set; }
        public Sortings SortingEntriesSelected { get; set; }
        public CompareDiffFormats CompareView { get; set; }
        public bool SortingEntriesDescending { get; set; }
    }
}
