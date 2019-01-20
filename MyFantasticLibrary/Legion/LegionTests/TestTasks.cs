using BasicTask;
using ComponentContract;
using LegionContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LegionTests
{
    [Component("Error Task", "1.0.0", "MJayJ")]
    [Legion(typeof(DataIn), typeof(DataOut))]
    public class ErrorTask : LegionTask
    {
        public override LegionDataOut Run(LegionDataIn dataIn)
        {
            throw new Exception("test");
        }
    }
}
