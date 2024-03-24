using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shell.Infrastructure;

namespace Shell.Application
{
    internal class HelloCommand : IShellCommand
    {
        public string Name => "hello";

        public void Execute(IHost host, string[] args)
        {
            host.WriteLine("Hello World");
        }
    }
}
