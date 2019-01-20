using System;

namespace LegionContract
{
    /// <summary>
    /// Attribute for all legion tasks.
    /// </summary>
    public class LegionAttribute: Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="typeIn">Input data type (must be inherited from <see cref="LegionDataIn"></see>/>)</param>
        /// <param name="typeOut">Result type (must be inherited from <see cref="LegionDataOut"></see>/>)</param>
        public LegionAttribute(Type typeIn, Type typeOut)
        {
            TypeIn = typeIn;
            TypeOut = typeOut;
        }

        /// <summary>
        /// Input data type (must be inherited from <see cref="LegionDataIn"></see>/>)
        /// </summary>
        public Type TypeIn { get; private set; }
        /// <summary>
        /// Result type (must be derived from <see cref="LegionDataOut"></see>/>)
        /// </summary>
        public Type TypeOut { get; private set; }
    }
}
