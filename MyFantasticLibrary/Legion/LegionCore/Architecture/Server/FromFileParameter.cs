using LegionContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegionCore.Architecture.Server
{
    /// <summary>
    /// Represents input data loaded from file
    /// </summary>
    public class FromFileParameter: IParameter
    {
        private string _Filename;
        private StreamReader _File;
        private bool _Closed;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Filename">Path to file with input data</param>
        public FromFileParameter(string Filename)
        {
            _Filename = Filename;
        }
        /// <summary>
        /// Open file
        /// </summary>
        public void Open()
        {
            _File = new StreamReader(_Filename);
        }
        /// <summary>
        /// Close file
        /// </summary>
        public void Close()
        {
            if (_Closed)
                return;
            _Closed = true;
            _File?.Close();
        }
        /// <summary>
        /// Check if there is any parameters left
        /// </summary>
        /// <returns></returns>
        public bool ParametersAvailable()
        {
            if (_Closed)
                return false;
            return !_File.EndOfStream;
        }
        /// <summary>
        /// Load input data
        /// </summary>
        /// <param name="DataInCore">Empty input data</param>
        /// <returns>Input data if available, null otherwise</returns>
        public LegionDataIn GetNormalDataIn(LegionDataIn DataInCore)
        {
            if (ParametersAvailable())
            {
                LegionDataIn dataIn = DataInCore;
                dataIn.LoadFromStream(_File);
                return dataIn;
            }

            return null;
        }
        /// <summary>
        /// Dispose file
        /// </summary>
        public void Dispose()
        {
            _File?.Dispose();
        }
    }
}
