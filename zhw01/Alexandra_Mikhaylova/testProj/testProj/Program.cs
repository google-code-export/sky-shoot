using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testProj
{
    class Program
    {
        static void Main(string[] args)
        {
            const int N = 100;
            int[] nums = new int[N+1];
            for (int i = 0; i < N + 1; i ++)
                nums[i] = 0;
            for (int i = 2; i < N + 1; i ++)
                {
                    if (nums[i] == 1)
                        continue;
                    for (int k = 2; i * k < N + 1; k++)
                        nums[i * k] = 1;
                }

            for (int i = 2; i < N + 1; i++)
                if (nums[i] == 0)
                    Console.WriteLine(i);

            Console.ReadKey();
        }
    }
}
