using ComponentContract;
using StringManipulator;

namespace LowerManipulator
{
    [Component("Lowercase String Manipulator", Version = "1.0", Publisher = "MJayJ", Description = "Convert string to lower case")]
    public class LowerStringManipulator : IStringManipulator
    {
        public string Manip(string toManip)
        {
            return toManip.ToLower();
        }
    }
}
