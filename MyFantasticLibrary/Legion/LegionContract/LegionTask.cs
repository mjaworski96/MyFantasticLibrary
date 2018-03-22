namespace LegionContract
{
    public abstract class LegionTask: IdentifiedById
    {
        public abstract LegionDataOut Run(LegionDataIn dataIn);
    }
}
