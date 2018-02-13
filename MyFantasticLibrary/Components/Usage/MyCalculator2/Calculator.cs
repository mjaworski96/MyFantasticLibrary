using Calculator;
using ComponentContract;
using System;

namespace MyCalculator2
{
    [Component("My Calculator", "2.0",  "MJayJ II",  "Simple calculator")]
    public class Calculator : ICalculator
    {
        public double Add(double a, double b)
        {
            return a + b;
        }

        public double Div(double a, double b)
        {
            if (b == 0) throw new ArithmeticException("Divide by 0");
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
