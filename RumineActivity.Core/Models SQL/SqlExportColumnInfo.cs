using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Models.SQL
{
    class SqlExportColumnInfo
    {
        public string displayName { get; set; }
        public string name { get; set; }
        public string database { get; set; }
        public string table { get; set; }
        public string type { get; set; }
    }
}
