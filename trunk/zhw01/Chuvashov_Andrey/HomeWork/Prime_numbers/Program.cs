using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prime_numbers
{
    class Program
    {
        static void Main(string[] args)
        {
            SieveOfEratosthenes sieve = new SieveOfEratosthenes(100);
            sieve.Print();
            Console.ReadKey();
        }
    }
}
