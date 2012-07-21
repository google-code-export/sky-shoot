using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstHomeTask
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[] primes = new bool[101];
            for(int i=0; i<100; i++)
            {
                primes[i]=true;
            }
           
            for(int i=2; i<100; i++) 
            {

               if(primes[i])
               {
                   for(int k=i*i; k<=100; k+=i)
                    {
                     primes[k]=false;
                    }
               }
            }
            for(int i=2; i<=100; i++)
            { if(primes[i])
              {
                  Console.WriteLine(i);
              }
            }
            Console.ReadKey();
        }
    }
}

