﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public static class Selection
    {
        //Измерение
        public static MeasureMethod[] Methods { get; private set; } = Enum.GetValues<MeasureMethods>().Select(u => new MeasureMethod(u)).ToArray();
        public static OutputValue[] Outs { get; private set; } = new PostOutputs[] { PostOutputs.PeriodDifference }.Select(p => new OutputValue(p)).ToArray();
        public static MeasureUnit[] Units { get; private set; } = Enum.GetValues<MeasureUnits>().Select(u => new MeasureUnit(u)).ToArray();
        public static CompareValue[] CompareValues { get; private set; } = Enum.GetValues<CompareValues>().Select(v => new CompareValue(v)).ToArray();

        //Настройки отчета
        public static Period[] Periods { get; private set; } = Enum.GetValues<Periods>().Select(d => new Period(d)).ToArray();
        public static TopicsMode[] TopicModes { get; private set; } = Enum.GetValues<TopicsModes>().Select(u => new TopicsMode(u)).ToArray();
        public static ReportType[] ReportTypes { get; private set; } = new Reports[] { Reports.Fact, Reports.Periodical }.Select(r => new ReportType(r)).ToArray();

        //Представление
        public static ViewType[] DataViews { get; private set; } = Enum.GetValues<DViewTypes>().Select(d => new ViewType(d)).ToArray();

        public static int[] Years { get; set; } = new int[] { 0, 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018, 2019, 2020, 2021 };
        public static int[] Months { get; set; } = Enumerable.Range(0, 13).ToArray();

    }
}
