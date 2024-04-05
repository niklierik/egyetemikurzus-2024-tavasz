using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shell.Infrastructure;

namespace Shell.Application
{
    [DoNotLoad]
    internal class KresshCommand : IShellCommand
    {
        public string Name => "crash";

        public void Execute(IHost host, string[] args)
        {
            throw new NullReferenceException();

            string s = "abcd";

            string other = s + "abcd";

        }
    }
}
