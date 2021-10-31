using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public enum Reports
    {
        Fact, Periodical, Empty
    }
    public class ReportType : EnumType<Reports>
    {
        public ReportType(Reports reports) : base(reports)
        {
            switch (reports)
            {
                case Reports.Fact:
                    Name = "Фактический";
                    break;
                case Reports.Periodical:
                    Name = "Периодический";
                    break;
                case Reports.Empty:
                    Name = "Пустой";
                    break;
            }
        }
    }

}
