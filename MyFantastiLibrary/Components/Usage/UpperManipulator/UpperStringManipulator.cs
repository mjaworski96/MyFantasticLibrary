using ComponentContract;
using StringManipulator;

namespace UpperManipulator
{
    [Component("Uppercase String Manipulator", Version = "1.0", Publisher = "MJayJ", Description = "Convert string to upper case")]
    public class UpperStringManipulator : IStringManipulator
    {
        public string Manip(string toManip)
        {
            return toManip.ToUpper();
        }
    }
}
