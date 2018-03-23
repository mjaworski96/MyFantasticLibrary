using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ComponentsLoader;
using LegionContract;

namespace LegionCore.Architecture
{
    public class Server: IDisposable
    {

        internal LegionDataIn CurrentDataIn
        {
            get
            {
                LegionAttribute attr = (LegionAttribute)CurrentTaskSingleton
                    .GetType()
                    .GetCustomAttribute(typeof(LegionAttribute));
                return (LegionDataIn)Activator.CreateInstance(attr.TypeIn);
            }
        }
        internal LegionDataOut CurrentDataOut
        {
            get
            {
                LegionAttribute attr = (LegionAttribute)CurrentTaskSingleton
                    .GetType()
                    .GetCustomAttribute(typeof(LegionAttribute));
                return (LegionDataOut)Activator.CreateInstance(attr.TypeOut);
            }
    }

        internal LegionTask CurrentTaskSingleton
        {
            get
            {
                return CurrentTask.Singleton;
            }
        }

        internal LoadedComponent<LegionTask> CurrentTask
        {
            get
            {
                return Loader.GetComponentByName<LegionTask>("Add And Wait Task");
            }
        }

        StreamReader dataInReader = new StreamReader("data_in.txt");
        StreamWriter dataOutWriter = new StreamWriter("data_out.txt");

        internal List<LegionDataIn> GetDataIn(int taskCount)
        {
            
            lock (dataInReader)
            {
                List<LegionDataIn> result = new List<LegionDataIn>(taskCount);

                for (int i = 0; i < taskCount && !dataInReader.EndOfStream; i++)
                {
                    LegionDataIn dataIn = CurrentDataIn;
                    dataIn.LoadFromStream(dataInReader);
                    result.Add(dataIn);

                }

                return result;
            }
        }

        internal void SaveResults(List<LegionDataOut> dataOut)
        {
            lock (dataOutWriter)
            {
                foreach (var data in dataOut)
                {
                    data.SaveToStream(dataOutWriter);
                }
            }
        }

        public void Dispose()
        {
            dataInReader?.Dispose();
            dataOutWriter?.Dispose();
        }
    }
}
