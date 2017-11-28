using Calculator;
using ComponentContract;

namespace MyCalculator
{
    [Component("My Calculator", Version = "1.0", Publisher = "MJayJ", Description = "Simple calculator")]
    public class Calculator : ICalculator
    {
        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Div(double a, double b)
        {
            return a / b;
        }

        public double Mod(double a, double b)
        {
            return a % b;
        }

        public double Mul(double a, double b)
        {
            return a * b;
        }

        public double Sub(double a, double b)
        {
            return a - b;
        }
    }
}
