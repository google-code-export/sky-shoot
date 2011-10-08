using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomewWork1
{
    class Program
    {
        private static int START = 2;
        private static int END = 100;

        static void Main(string[] args)
        {
            writePrimeNumbers();
        }

        private static void writePrimeNumbers()
        {
            List<int> primeNumbers = searchPrimeNumbers(START, END);

            foreach (int primeNumber in primeNumbers) Console.Write(primeNumber + " ");
            Console.ReadKey();
        }

        /* 
            Метод поиска простых числе "в лоб"
        */

        private static List<int> searchPrimeNumbers(int start, int end)
        {
            List<int> primeNumbers = new List<int>();

            bool isPrime;
            for (int number = start; number < end; ++number){
                isPrime = true;
                
                foreach (int primeNumber in primeNumbers){
                    if (number % primeNumber == 0){
                        isPrime = false;
                        break;
                    }
                }
                if (isPrime) primeNumbers.Add(number);
            }

            return primeNumbers;
        }

    }
}
