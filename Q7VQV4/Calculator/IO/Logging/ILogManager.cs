namespace Calculator.IO.Logging;

public interface ILogManager : ILogTarget, IDisposable
{
    void SetLogLevel(LogLevel level);
}
