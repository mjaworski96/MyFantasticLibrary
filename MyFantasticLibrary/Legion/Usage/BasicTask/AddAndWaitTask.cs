using ComponentContract;
using LegionContract;
using System.Threading;

namespace BasicTask
{
    [Component("Add And Wait Component", "1.0.0", "MJayJ")]
    [Legion(typeof(DataIn), typeof(DataOut))]
    public class AddAndWaitTask : LegionTask
    {
        public override LegionDataOut Run(LegionDataIn dataIn)
        {
            DataIn data = (DataIn)dataIn;
            DataOut dataOut = new DataOut();
            Thread.Sleep(data.Wait);
            dataOut.Result = data.A + data.B;
            return dataOut;
        }
    }
}
