namespace Calculator.IO.Logging;

public interface ILogTarget
{
    Task Log(LogLevel logLevel, object? message, Exception? exception = null);

    async Task Info(object? message, Exception? exception = null) =>
        await Log(LogLevel.Info, message, exception);
    async Task Warning(object? message, Exception? exception = null) =>
        await Log(LogLevel.Warn, message, exception);
    async Task Error(object? message, Exception? exception = null) =>
        await Log(LogLevel.Error, message, exception);
    async Task Debug(object? message, Exception? exception = null) =>
        await Log(LogLevel.Debug, message, exception);
}
