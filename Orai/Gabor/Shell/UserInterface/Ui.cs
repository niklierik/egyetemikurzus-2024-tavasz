using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly ICommandProvider _commandProvider;
        private readonly IHost _host;

        public Ui(ICommandProvider commandProvider, IHost host)
        {
            _commandProvider = commandProvider;
            _host = host;
        }

        public void Run()
        {
            while (true)
            {
                _host.Write("> ");
                string input = _host.ReadLine();
                string[] splittedInput = input.Split(' ');
                IShellCommand? commandToExecute = FindCommandName(splittedInput[0]);
                if (commandToExecute != null)
                {
                    try
                    {
                        commandToExecute.Execute(_host, splittedInput);
                    }
                    catch (Exception ex)
                    {
                        _host.WriteLine("Hiba történt");
                        Trace.WriteLine(ex, "commandexception");
                    }
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
