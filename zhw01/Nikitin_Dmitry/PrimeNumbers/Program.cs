using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrimeNumbers
{
    class Program
    {
        const int MaxNumber = 99;
        static void Main(string[] args)
        {
            bool[] isPrime = new bool[MaxNumber];
            for (int i = 1; i < MaxNumber; i++)
            {
                isPrime[i] = true;
            }
            isPrime[1] = false;

            for (int i = 1; i < MaxNumber; i++)
            {
                if (isPrime[i])
                    for (int j = 2*i; j < MaxNumber; j += i)
                        isPrime[j] = false;
            }
            for(int i = 1;i<MaxNumber; i++) 
                if(isPrime[i]) Console.Write("{0} ", i);
            Console.Write("\n");
            Console.ReadKey();
        }
    }
}
