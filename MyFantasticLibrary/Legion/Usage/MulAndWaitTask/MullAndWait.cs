using ComponentContract;
using LegionContract;
using System.Threading;

namespace MulAndWaitTask
{
    [Component("Mul And Wait Task", "1.0.0", "MJayJ")]
    [Legion(typeof(DataIn), typeof(DataOut))]
    public class AddAndWaitTask : LegionTask
    {
        public override LegionDataOut Run(LegionDataIn dataIn)
        {
            DataIn data = (DataIn)dataIn;
            DataOut dataOut = new DataOut();
            dataOut.Result = data.A * data.B * data.C;
            Thread.Sleep(dataOut.Result);
            return dataOut;
        }
    }
}
