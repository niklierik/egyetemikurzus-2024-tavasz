using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Shell.Infrastructure;

namespace Shell.UserInterface
{
    internal class Ui
    {
        private readonly CommandProvider _commandProvider;
        private readonly IHost _host;

        public Ui(CommandProvider commandProvider, IHost host)
        {
            _commandProvider = commandProvider;
            _host = host;
        }

        public void Run()
        {
            while (true)
            {
                string input = _host.ReadLine();
                string[] splittedInput = input.Split(' ');
                IShellCommand? commandToExecute = FindCommandName(splittedInput[0]);
                if (commandToExecute != null)
                {
                    //TODO: Folytatni
                    commandToExecute.Execute();
                }  
            }
        }

        private IShellCommand? FindCommandName(string commandName)
        {
            foreach (var command in _commandProvider.Commands) 
            { 
                if (command.Name.Equals(commandName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return command;
                }
            }

            return null;
        }
    }
}
