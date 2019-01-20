
namespace LegionCore.NetworkCommunication
{
    /// <summary>
    /// Operation Code - what should be done with message
    /// </summary>
    public enum OperationCode
    {
        /// <summary>
        /// Nothing
        /// </summary>
        NO_OPERATION = 0,
        /// <summary>
        /// Send current task files metadatas
        /// </summary>
        GET_CURRENT_TASK = 100,
        /// <summary>
        /// Send current task files
        /// </summary>
        GET_CURRENT_TASK_FILES = 101,
        /// <summary>
        /// Send task input data
        /// </summary>
        GET_DATA_IN = 200,
        /// <summary>
        /// Save task result
        /// </summary>
        SAVE_RESULTS = 300,
        /// <summary>
        /// Save error data
        /// </summary>
        RAISE_ERROR = 400,
        /// <summary>
        /// Save initialization error data
        /// </summary>
        RAISE_INITIALIZATION_ERROR = 401
    }
}
