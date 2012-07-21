using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {


            double count = 0;
            for (int chislo = 1; chislo < 100; chislo++)
            {
                for (int i = 2; i <= Math.Sqrt(chislo); i++)
                {
                    if ((chislo % i) == 0) { count++; }
                }
                if (count == 0)
                    Console.WriteLine(chislo);

                count = 0;
            }
        }
    }
}


