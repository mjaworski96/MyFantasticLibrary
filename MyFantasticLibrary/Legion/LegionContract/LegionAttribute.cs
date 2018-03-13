using System;

namespace LegionContract
{
    public class LegionAttribute: Attribute
    {
        public LegionAttribute(Type typeIn, Type typeOut)
        {
            TypeIn = typeIn;
            TypeOut = typeOut;
        }

        public Type TypeIn { get; private set; }
        public Type TypeOut { get; private set; }
    }
}
