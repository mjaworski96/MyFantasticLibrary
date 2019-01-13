
namespace LegionCore.NetworkCommunication
{
    public enum OperationCode
    {
        NO_OPERATION = 0,
        GET_CURRENT_TASK = 100,
        GET_CURRENT_TASK_FILES = 101,
        GET_DATA_IN = 200,
        SAVE_RESULTS = 300,
        RAISE_ERROR = 400,
        RAISE_INITIALIZATION_ERROR = 401
    }
}
