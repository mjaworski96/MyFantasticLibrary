namespace LegionContract
{
    /// <summary>
    /// Legion task abstraction
    /// </summary>
    public abstract class LegionTask: IdentifiedById
    {
        /// <summary>
        /// Runs task
        /// </summary>
        /// <param name="dataIn">Input data</param>
        /// <returns>Task result</returns>
        public abstract LegionDataOut Run(LegionDataIn dataIn);
    }
}
