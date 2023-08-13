using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.Models
{
    /// <summary>
    /// Модель форумного пользователя
    /// </summary>
    [Legacy]
    public class User
    {
        public int UserID { get; set; }
        public string Nick { get; set; }
    }
}
