
namespace LegionCore.NetworkCommunication
{
    /// <summary>
    /// Status of client-server communicaton
    /// </summary>
    public enum CodeStatus: int
    {
        /// <summary>
        /// Ok
        /// </summary>
        OK = 100,
        /// <summary>
        /// Error
        /// </summary>
        ERROR = 200,
        /// <summary>
        /// Server finished work
        /// </summary>
        FINISHED = 300,
        /// <summary>
        /// Ok, no operation should be done
        /// </summary>
        NO_OPERATION = 400,
        /// <summary>
        /// Ok, but no content
        /// </summary>
        NO_CONTENT = 500
    }
}
