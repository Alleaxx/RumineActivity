using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Models
{
    public class Rage
    {
        public string RageTitle { get; set; }
        public List<RageStat> Statistics { get; set; }

        public Rage()
        {
            Statistics = new List<RageStat>();
        }
    }
    public class RageEntry
    {
        public string RageTitle { get; init; }
        public RageStat Stat { get; init; }

        public RageEntry(Rage rage, string filter)
        {
            RageTitle = rage.RageTitle;
            Stat = rage.Statistics.FirstOrDefault(r => r.Title == filter)
                ?? rage.Statistics.FirstOrDefault()
                ?? new RageStat() { Title = "Все время" };
        }
    }
    public class RageStat
    {
        public string Title { get; set; }
        public int CountUniq { get; set; }
        public int CountTotal { get; set; }

        public double CoeffSpam => CountTotal > 0 ? 1.0 * CountTotal / CountUniq : 0;
    }
}
