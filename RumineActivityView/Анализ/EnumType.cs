using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivityView
{
    public class EnumType<T>
    {
        public override string ToString()
        {
            return Name;
        }

        public string Name { get; protected set; }
        public T Type { get; private set; }
        public EnumType(T type)
        {
            Type = type;
        }


        public override bool Equals(object obj)
        {
            if(obj is EnumType<T> type)
            {
                return type.Type.Equals(Type);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}
