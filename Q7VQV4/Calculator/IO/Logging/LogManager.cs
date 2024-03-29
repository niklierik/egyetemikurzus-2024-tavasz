namespace Calculator.IO.Logging;

public class LogManager(ICollection<ILogTarget> logTargets) : ILogManager
{
    private LogLevel _logLevel;

    private ICollection<ILogTarget> LogTargets { get; } = logTargets;

    public async Task Log(LogLevel logLevel, object? message, Exception? exception = null)
    {
        if (_logLevel < logLevel)
        {
            return;
        }

        await Task.WhenAll(
            LogTargets.Select(logTarget => logTarget.Log(logLevel, message, exception))
        );
    }

    public void SetLogLevel(LogLevel level)
    {
        _logLevel = level;
    }

    public void Dispose()
    {
        foreach (ILogTarget logTarget in LogTargets)
        {
            if (logTarget is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}
