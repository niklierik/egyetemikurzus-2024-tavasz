using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Ora3
{
    internal static class GenericSum
    {
        public static T Sum<T>(IEnumerable<T> values) 
            where T : INumber<T>
        {
            T result = T.Zero;
            T counter = T.Zero;
            foreach (var item in values)
            {
                result += item;
                counter++;
            }
            return result / counter;
        }
    }
}
