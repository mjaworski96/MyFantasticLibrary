using AbstractClassContract;
using ComponentContract;

namespace AbstractClassContractImplementaion
{
    [Component("Abstract Class Contract Implementation", Version = "1.0", Publisher = "MJayJ", Description = "Only for tests")]
    public class DoSomethingImpl : DoSomething
    {
        public override void TryDoSomething()
        {
            //Implementation
        }
    }
}
