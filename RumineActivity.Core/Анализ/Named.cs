using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class Named
    {
        public override string ToString() => $"{GetType().Name}: {Name}";
        public virtual string Name { get; protected set; }
        public Named()
        {
            Name = "";
        }
    }
}
