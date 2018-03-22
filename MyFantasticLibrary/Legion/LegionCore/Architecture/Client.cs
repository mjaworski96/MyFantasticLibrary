using ComponentsLoader;
using LegionContract;
using System;

namespace LegionCore.Architecture
{
    public class Client
    {
        private LegionTask[] _tasks;
        private IClientCommunicator _communicator;

        public Client(IClientCommunicator communicator, int tasksCount)
        {
            _communicator = communicator;
            _tasks = new LegionTask[tasksCount];
        }
        public void Init()
        {
            try
            {
                LoadedComponent<LegionTask> loadedComponent
                = _communicator.CurrentTask;
                for (int i = 0; i < _tasks.Length; i++)
                {
                    _tasks[i] = loadedComponent.NewInstantion;
                    IdManagement.SetId(_tasks[i], i);
                }
            }
            catch(NullReferenceException e)
            {
                Exception exc = new LegionException("Your clases must inherit directly from LegionTask, LegionDataIn or LegionDataOut", e);
                _communicator.RaiseError(exc);
                throw exc;
            }
            
        }
    }
}
