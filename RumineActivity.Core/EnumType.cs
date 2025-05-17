using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RumineActivity.Core
{
    public class EnumType<T>
    {
        public readonly T Type;
        public override string ToString()
        {
            return Name;
        }

        public virtual string Name { get; protected set; }

        public EnumType(T type)
        {
            Type = type;
            Name = type.ToString();
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

        public static implicit operator T(EnumType<T> enums)
        {
            return enums.Type;
        }
    }
}
