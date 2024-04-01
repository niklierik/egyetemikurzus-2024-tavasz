using Calculator.IO.Logging;
using Calculator.State;
using Microsoft.Extensions.Hosting;

namespace Calculator.Interpreters;

public class InterpreterProgram(
    IInterpreter<InterpreterState> interpreter,
    IO.IHost host,
    ILogManager logManager
) : IHostedService
{
    private readonly IInterpreter<InterpreterState> _interpreter = interpreter;
    private readonly IO.IHost _host = host;
    private readonly ILogManager _logManager = logManager;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _interpreter.Init();
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

            object? result = await _interpreter.Execute(text);
            if (result is null)
            {
                _host.WriteLine("null", ConsoleColor.White);
                continue;
            }
            if (result is not Exception)
            {
                _host.WriteLine(result, ConsoleColor.Green);
            }
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _logManager.Debug("Closing app");
        _logManager.Dispose();
    }
}
