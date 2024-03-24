using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo("TestMatek")]

namespace Matek
{
    internal class FibonacciCalculator
    {
        public int[] CalculateFibo(int n) {
            if (n <= 0)
            {
                throw new ArgumentException("n must be greater than 0");
            }
            int[] fib = new int[n];

            if(n == 1)
            {
                fib[0] = 0;
                return fib;
            }

            if (n == 2)
            {
                fib[1] = 1;
                return fib;
            }

            for(int i = 2; i < n-1; i++)
            {
                fib[i] = fib[i-1] + fib[i-2];
            }
            return fib;
        }
    }
}
