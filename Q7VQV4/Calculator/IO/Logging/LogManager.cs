namespace Calculator.IO.Logging;

public class LogManager(ILogTargetProvider logTargetProvider) : ILogManager
{
#if DEBUG
    private LogLevel _logLevel = LogLevel.All;
#else
    private LogLevel _logLevel = LogLevel.Info;
#endif

    private ICollection<ILogTarget> LogTargets { get; } = logTargetProvider.GetLogTargets();

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
