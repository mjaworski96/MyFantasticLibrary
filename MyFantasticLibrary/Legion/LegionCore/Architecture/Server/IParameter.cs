using LegionContract;
using System;

namespace LegionCore.Architecture.Server
{
    /// <summary>
    /// Abstraction for legion input data source
    /// </summary>
    public interface IParameter: IDisposable
    {
        /// <summary>
        /// Check if there are any parameters left
        /// </summary>
        /// <returns></returns>
        bool ParametersAvailable();
        /// <summary>
        /// Get input data
        /// </summary>
        /// <param name="DataInCore">Input data to full in</param>
        /// <returns>Input data if available, null otherwise</returns>
        LegionDataIn GetNormalDataIn(LegionDataIn DataInCore);
        /// <summary>
        /// Open input data source
        /// </summary>
        void Open();
        /// <summary>
        /// Close input data source
        /// </summary>
        void Close();
    }
}
