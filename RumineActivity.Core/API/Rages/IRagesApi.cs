using RumineActivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumineActivity.Core.API
{
    public interface IRagesApi
    {
        event Action OnLoaded;
        bool? IsLoaded { get; }
        Task LoadData();

        Task<IEnumerable<Rage>> GetRages();
    }
}
