using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace primes
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] pr;
            pr = new int[101];

            for (int i = 1; i < 101; i++)
            {
                pr[i] = 0;
            }

            for (int i = 2; i < 101; i++)
            {
                if (pr[i] == 0)
                {
                    Console.WriteLine(i);
                    for (int j = i; j < 101; j += i) pr[j] = 1;
                }
            }

            Console.ReadKey();

        }
    }
}