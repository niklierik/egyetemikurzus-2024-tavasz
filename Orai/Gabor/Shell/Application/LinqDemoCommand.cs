using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shell.Infrastructure;

namespace Shell.Application
{
    internal class LinqDemoCommand : IShellCommand
    {
        public string Name => "linq";

        public void Execute(IHost host, string[] args)
        {
            int[] elements = new int[100];
            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = Random.Shared.Next(0, 100);
            }

            //Query sintax
            var strings = from element in elements
                          orderby element descending
                          select ToFizzbuzz(element);

            //Lambda syntax
            var strings2 = elements
                .Select(element =>
            {
                if (element % 5 == 0 && element % 3 == 0)
                    return "FizzBuzz";
                else if (element % 3 == 0)
                    return "Fizz";
                else if (element % 5 == 0)
                    return "Buzz";
                else
                    return element.ToString();
            }).OrderBy(s => s[0]);

            foreach (var s in strings.ToArray())
            {
                Console.WriteLine(s);
            }
        }

        private string ToFizzbuzz(int element)
        {
            if (element % 5 == 0 && element % 3 == 0)
                return "FizzBuzz";
            else if (element % 3 == 0)
                return "Fizz";
            else if (element % 5 == 0)
                return "Buzz";
            else
                return element.ToString();
        }
    }
}
