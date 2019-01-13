using LegionContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegionCore.Architecture.Server
{
    public class FromFileParameter: IParameter
    {
        private string _Filename;
        private StreamReader _File;
        private bool _Closed;

        public FromFileParameter(string Filename)
        {
            _Filename = Filename;
        }

        public void Open()
        {
            _File = new StreamReader(_Filename);
        }
        public void Close()
        {
            if (_Closed)
                return;
            _Closed = true;
            _File?.Close();
        }
        public bool ParametersAvailable()
        {
            if (_Closed)
                return false;
            return !_File.EndOfStream;
        }
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
        public void Dispose()
        {
            _File?.Dispose();
        }
    }
}
