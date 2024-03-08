using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shell.Application;
using Shell.Infrastructure;

namespace Shell.UserInterface
{
    internal class CommandProvider
    {
        public IShellCommand[] Commands
        {
            get;  
        }

        public CommandProvider()
        {
            Commands = new IShellCommand[]
                {
                    new ExitCommand(),
                };
        }
    }
}
