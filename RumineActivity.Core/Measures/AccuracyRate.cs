using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Measures
{
    public enum AccuracyRates
    {
        Ideal,
        High,
        Good,
        Medium,
        Low,
        None,
    }

    /// <summary>
    /// Показатель погрешностей в измерении активности
    /// </summary>
    public class AccuracyRate : EnumType<AccuracyRates>
    {
        public int MaxErrorPosts { get; set; }

        public AccuracyRate(AccuracyRates rate) : base(rate)
        {
            switch(rate)
            {
                case AccuracyRates.Ideal:
                    Name = "Идеальная";
                    MaxErrorPosts = 0;
                    break;
                case AccuracyRates.High:
                    Name = "Высокая";
                    MaxErrorPosts = 20;
                    break;
                case AccuracyRates.Good:
                    Name = "Хорошая";
                    MaxErrorPosts = 100;
                    break;
                case AccuracyRates.Medium:
                    Name = "Средняя";
                    MaxErrorPosts = 500;
                    break;
                case AccuracyRates.Low:
                    Name = "Низкая";
                    MaxErrorPosts = 5000;
                    break;
                case AccuracyRates.None:
                    Name = "Никакая";
                    MaxErrorPosts = 50000000;
                    break;
            }
        }


        public static AccuracyRate GetAccuracyFor(int missed)
        {
            if(missed < 0)
                return new AccuracyRate(AccuracyRates.None);

            var rates = Enum.GetValues<AccuracyRates>().Select(v => new AccuracyRate(v)).OrderBy(r => r.MaxErrorPosts).ToArray();
            return rates.First(r => missed <= r.MaxErrorPosts);
        }
    }
}
