using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Models.SQL
{
    /// <summary>
    /// Модель для экспортного файла результата запроса Sqlite
    /// </summary>
    class SqlExportReport
    {
        public string type { get; set; }
        public string query { get; set; }

        public IEnumerable<SqlExportColumnInfo> columns { get; set; }
        public IEnumerable<SqlExportRow> rows { get; set; }

        public SqlExportReport()
        {
            columns= new List<SqlExportColumnInfo>();
            rows = new List<SqlExportRow>();
        }
    }
}
