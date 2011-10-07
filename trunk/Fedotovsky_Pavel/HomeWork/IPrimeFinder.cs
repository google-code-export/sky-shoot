using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeWork
{
    interface IPrimeFinder
    {
        List<int> GetPrimes(int start, int end);
    }
}
