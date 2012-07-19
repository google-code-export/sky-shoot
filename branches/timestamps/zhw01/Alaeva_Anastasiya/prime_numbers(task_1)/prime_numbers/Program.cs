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
            int j, i, count = 0, flag;
            for (j = 2; j <= 100; j++)
                {
                    flag = 1;
                    for (i = 2; i * i <= j; i++)
                    {
                        if (j % i == 0)
                        {
                            flag = 0; break; 
                        }
                    }
                    if (flag != 0)
                    {
                        Console.WriteLine(j);
                        count++;
                    }
                }
            Console.ReadKey();
          }
    }
}
