using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prime_numbers
{
    class SieveOfEratosthenes
    {
        private bool[] sieve;

        public SieveOfEratosthenes(int quontity)
        {
            sieve = new bool[quontity - 2];
            for (int i = 0; i < quontity - 2; i++)
                sieve[i] = true;
            for (int k = 0; k < quontity - 2; k++)
            {
                if (sieve[k] == false)
                    continue;
                else
                    Clip(k + 2);
            }
        }

        private void Clip(int step)
        {
            int i = 2 * step - 2;
            while(i < sieve.Length)
            {
                sieve[i] = false;
                i += step;
            }
        }

        public void Print()
        {
            for (int i = 0; i < sieve.Length; i++)
                if (sieve[i] == true)
                    Console.WriteLine(i + 2);
        }
    }
}
