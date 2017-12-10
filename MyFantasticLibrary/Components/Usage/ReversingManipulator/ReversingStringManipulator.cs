using ComponentContract;
using StringManipulator;
using System.Text;

namespace ReversingManipulator
{
    [Component("Reversing String Manipulator", Version = "1.0", Publisher = "MJayJ", Description = "Reverse string")]
    public class ReversingStringManipulator : IStringManipulator
    {
        public string Manip(string toManip)
        {
            StringBuilder sb = new StringBuilder(toManip);
            int begin = 0;
            int end = toManip.Length - 1;
            char buffer;
            while (begin < end)
            {
                buffer = sb[begin];
                sb[begin] = sb[end];
                sb[end] = buffer;
                begin++;
                end--;
            }
            return sb.ToString();
        }
    }
}
