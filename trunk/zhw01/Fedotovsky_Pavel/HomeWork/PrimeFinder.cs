using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeWork
{
    class PrimeFinder : IPrimeFinder
    {
        public List<int> GetPrimes(int start, int end)
        {
            List<int> primes = new List<int>();
            bool[] isPrime = new bool[end + 1];

            for (int i = 0; i <= end; i++)
                isPrime[i] = true;

            isPrime[0] = isPrime[1] = false;
            
            for (int i = 2; i <= end; i++) {
                if(isPrime[i]) {
                    primes.Add(i);
                    for (int j = i * i; j <= end; j += i)
                        isPrime[j] = false;
                }    
            }

            return primes;
        }
    }
}
