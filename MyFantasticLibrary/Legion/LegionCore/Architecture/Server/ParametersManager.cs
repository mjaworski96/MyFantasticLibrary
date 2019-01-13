using System;
using System.Collections.Generic;
using System.Text;
using LegionContract;
using LegionCore.Logging;

namespace LegionCore.Architecture.Server
{
    internal class ParametersManager
    {
        private List<IParameter> _ParametersSources;
        private int _LastParameterId;
        private int _CurrentParameterSourceId;

        private IParameter CurrentParameterSource
        { get => _ParametersSources[_CurrentParameterSourceId];  }

        public int LastParameterId { get => _LastParameterId; }

        internal bool NoParametersAvailable { get; set; }

        internal ParametersManager(IEnumerable<string> filenames)
        {
            _ParametersSources = new List<IParameter>();
            foreach (var item in filenames)
            {
                _ParametersSources.Add(new FromFileParameter(item));
            }
        }

        internal void CheckNextInputParameters()
        {
            if (!CurrentParameterSource.ParametersAvailable())
            {
                CurrentParameterSource.Close();
                if (_CurrentParameterSourceId + 1 < _ParametersSources.Count)
                {
                    _CurrentParameterSourceId++;
                    CurrentParameterSource.Open();
                }
                else
                {
                    NoParametersAvailable = true;
                }
            }
        }
        internal LegionDataIn GetNormalDataIn(LegionDataIn DataInCore)
        {
            CheckNextInputParameters();
            if (CurrentParameterSource.ParametersAvailable())
            {
                LegionDataIn dataIn;
                try
                {
                    dataIn = CurrentParameterSource.GetNormalDataIn(DataInCore);
                }
                catch(Exception e)
                {
                    string msg = "Task parameter initialization error: " + e.Message;
                    LoggingManager.Instance.LogError(msg + "\n" + e.StackTrace);
                    dataIn = new LegionErrorDataIn(e);
                }
                IdManagement.SetId(dataIn, _LastParameterId);
                _LastParameterId++;
                return dataIn;
            }
            return null;
        }

        internal void Dispose()
        {
            foreach (var item in _ParametersSources)
            {
                item.Dispose();
            }
        }

        internal void Init()
        {
            if(_ParametersSources.Count != 0)
                CurrentParameterSource.Open();
        }
    }
}
