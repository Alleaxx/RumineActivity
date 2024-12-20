﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using RumineActivity.Core;
namespace RumineActivity.View
{
    public class ViewRules
    {
        public Rule UnknownRule { get; private set; }
        public List<Rule> List { get; private set; } = new List<Rule>();
        public Rule GetFor(Entry entry) => List.FirstOrDefault(r => r.Check(entry)) ?? UnknownRule;
        public void Add(string color, double from, double to)
        {
            List.Add(new RuleRanged(color, from, to));
        }

        public ViewRules()
        {
            SetDefaultRules();
        }

        //Правила по редкости
        private void SetDefaultRules()
        {
            UnknownRule = new RuleRanged("lightpink", -1, -1);
            Add("#E6E6E6", 0, 1.5);
            Add("#fffab5", 1.5, 5);
            Add("#BDECB6", 5, 15);
            Add("#C7DFFA", 15, 30);
            Add("#d8bee8", 30, 70);
            Add("#fccd6f", 70, 200);
            Add("#6ff2fc", 200, 10000);
        }
        //Правила по интенсивности (один цвет)
        private void SetIntenseColors()
        {
            //...
        }
    }

    public class Rule
    {
        public string Color { get; set; }
        protected Predicate<Entry> CheckMethod { get; set; }

        public bool Check(Entry entry)
        {
            return CheckMethod?.Invoke(entry) ?? true;
        }

        public Rule()
        {
            CheckMethod = EveryEntry;
        }
        private bool EveryEntry(Entry e) => true;
    }

    public class RuleRanged : Rule
    {
        private readonly static double MinBorder = 0;
        private readonly static double MaxBorder = 10000;

        public double Min
        {
            get => min;
            set
            {
                min = value;
            }
        }
        private double min;
        public double Max
        {
            get => max;
            set
            {
                max = value;
            }
        }
        private double max;

        public RuleRanged(string color, double from, double to)
        {
            CheckMethod = RangeCheck;
            Color = color;
            Min = from;
            Max = to;
        }
        private bool RangeCheck(Entry entry)
        {
            return entry.PostsWrittenAverage / 20 >= Min && entry.PostsWrittenAverage / 20 < Max;
        }
    }
}
