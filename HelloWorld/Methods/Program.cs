using System;
using System;

namespace HelloWorld
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] intsToCompress = new int[] { 10, 15, 20 };
            Console.WriteLine(GetSum(intsToCompress));
        }

        static private int GetSum(int[] intsToCompress)
        {
            int total = 0;

            foreach (int num in intsToCompress)
            {
                total += num;
            }

            return total;
        }
    }
}