namespace Calculator.IO.Logging;

public class FileLogger : ILogTarget, IDisposable
{
    private readonly StreamWriter _writer;

    public FileLogger(string targetFile)
    {
        _writer = new StreamWriter(targetFile, true);
        _writer.WriteLine();
    }

    public async Task Log(LogLevel logLevel, object? message, Exception? exception = null)
    {
        await _writer.WriteLineAsync($"[{logLevel}] - {message}");
        if (exception is not null)
        {
            await _writer.WriteLineAsync(exception.ToString());
        }
    }

    public void Dispose()
    {
        _writer.Dispose();
    }
}
