using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeWork_1
{
    class PrimeNumbers
    {
        const int Max = 100;
        static void Main()

        {
            bool[] PrimeNum = new bool[Max];
            for (int i = 1; i < Max; i++)
            {
                PrimeNum[i] = true;
            }
            PrimeNum[1] = false;

            for (int i = 1; i < Max; i++)
            {
                if (PrimeNum[i])
                    for (int k = i + i; k < Max; k += i)
                        PrimeNum[k] = false;
            }

            for (int i = 1; i < Max; i++)
                if (PrimeNum[i]) Console.Write("{0} ", i);

            Console.Write("\n");
            Console.ReadKey();
        }
    }
}