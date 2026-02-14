using System;

namespace MyExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator calculator = new Calculator();
            int x = calculator.SumFrom1ToXRecursive(100);
            Console.WriteLine(x);
        }


    }

    class Calculator
    {
        //public void PrintXTO1(int x)
        //{
        //    for (int i = 1; i <= x; i++)
        //    {
        //        Console.Write(i + " ");
        //    }
        //}

        public void PrintXTO1(int x)
        {
            if (x == 1)
            {
                Console.WriteLine(x);
            }
            else
            {
                Console.WriteLine(x);
                PrintXTO1(x - 1);
            }
        }

        public int SumFrom1ToX(int x)
        {
            int result = 0;
            for (int i = 1; i <= x; i++)
            {
                result += i;
            }
            return result;
        }

        public int SumFrom1ToXRecursive(int x)
        {
            if (x == 1)
            {
                return 1;
            }
            else
            {
                return x + SumFrom1ToXRecursive(x - 1);
            }
        }
    }
}
