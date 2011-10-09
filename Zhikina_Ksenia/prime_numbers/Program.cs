using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prime_numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            int x, y=2, p, k=0;
            Console.WriteLine("prime numbers under 100");
            for (x = 2; x < 100; x++) 
            {
                p = x / y;
                while ((y<=Math.Sqrt(x)+1) & (y!=x))
                {
                    if (x == p * y)
                    {
                        y++;
                        k++;
                    }
                    else 
                    {
                        k = k + 0;
                        y++;
                        p = x / y;
                    }
                }
                if (k == 0)
                {
                    Console.WriteLine(x + " ");
                }
                k = 0; 
                y = 2;
            }
            Console.ReadKey();
        }
    }
}
