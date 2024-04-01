using Calculator.IO.Logging;
using Microsoft.Extensions.Hosting;

namespace Calculator.Interpreters;

public class InterpreterProgram(IInterpreter interpreter, IO.IHost host, ILogManager logManager)
    : IHostedService
{
    private readonly IInterpreter _interpreter = interpreter;
    private readonly IO.IHost _host = host;
    private readonly ILogManager _logManager = logManager;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            _host.Write(" > ", ConsoleColor.White);
            string? text = _host.ReadLine();
            if (text is null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                continue;
            }

            try
            {
                object? result = await _interpreter.Execute(text);
                _host.WriteLine(result, ConsoleColor.Green);
            }
            catch (Exception exception)
            {
                _host.WriteLine(
                    "Unexpected error happened while evaluating expression.",
                    ConsoleColor.Red
                );
                _host.WriteLine(exception, ConsoleColor.Red);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _logManager.Debug("Closing app");
        _logManager.Dispose();
    }
}
