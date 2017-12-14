using Calculator;
using ComponentContract;

namespace MyCalculator
{
    [Component("My Calculator", Version = "2.0", Publisher = "MJayJ", Description = "Simple calculator")]
    class Calculator2: ICalculator
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
