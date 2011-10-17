using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeWork
{
    class Program
    {

        private static int start = 0;
        private static int end = 100;

        static void Main(string[] args)
        {
            IPrimeFinder primeFinder = new PrimeFinder();
            List<int> primes = primeFinder.GetPrimes(start, end);

            foreach(int primeNumber in primes)
                Console.WriteLine(primeNumber);
            Console.ReadKey();
        }
    }
}
