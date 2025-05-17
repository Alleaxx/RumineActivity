using Microsoft.AspNetCore.Components;
using RumineActivity.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.View
{

    public interface IReportConfigEditor
    {
        ConfigurationReport CurrentConfig { get; }
    }

    /// <summary>
    /// Редактор конфигурации отчета
    /// </summary>
    public class ReportConfigEditor : IReportConfigEditor
    {
        private readonly IStatApp App;

        public event Action<ConfigurationReport> OnConfigChanged;

        public ConfigurationReport CurrentConfig { get; private set; }

        public ReportConfigEditor(IStatApp app)
        {
            App = app;
            DateRangePickers = new IDateRangePicker[0];
            SetNewConfig(new ConfigurationReport());
        }
        public void SetNewConfig(ConfigurationReport config)
        {
            CurrentConfig = new ConfigurationReport(config);
            UpdateDatePickers();
            OnConfigChanged?.Invoke(CurrentConfig);
        }

        private void UpdateDatePickers()
        {
            DeattachEvents();

            DateRangePickerSimple = new DateRangePickerSimple(CurrentConfig.DateRange);
            DateRangePickerAdvanced = new DateRangePickerAdvanced(CurrentConfig.DateRange);
            DateRangePickers = new IDateRangePicker[]
            {
                DateRangePickerAdvanced,
                DateRangePickerSimple
            };

            AttachEvents();
        }
        private void DeattachEvents()
        {
            foreach (var picker in DateRangePickers)
            {
                if(picker != null)
                {
                    picker.OnDateRangeUpdated -= DateRangePicker_OnDatesUpdated;
                }
            }
        }
        private void AttachEvents()
        {
            foreach (var picker in DateRangePickers)
            {
                picker.OnDateRangeUpdated += DateRangePicker_OnDatesUpdated;
            }
        }


        public IDateRangePicker[] DateRangePickers { get; private set; }
        public DateRangePickerAdvanced DateRangePickerAdvanced { get; private set; }
        public DateRangePickerSimple DateRangePickerSimple { get; private set; }


        private void DateRangePicker_OnDatesUpdated(IDateRangePicker obj)
        {
            var result = obj.TryCreateDateRange();
            if (result != null)
            {
                CurrentConfig.DateRange = result;
                ChangePeriodIfIsNotOk();

                //плюс обновляем прочие пикеры
            }
        }
        private void ChangePeriodIfIsNotOk()
        {
            var result = CurrentConfig.DateRange;
            if (!result.IsOkWithPeriod(CurrentConfig.Period))
            {
                var firstFit = EnumValues.PeriodsList.FirstOrDefault(p => result.IsOkWithPeriod(p));
                CurrentConfig.Period = firstFit ?? Period.Create(Periods.Week);
            }
        }


    }
}
