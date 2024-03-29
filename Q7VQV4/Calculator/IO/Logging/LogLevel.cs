namespace Calculator.IO.Logging;

public enum LogLevel
{
    None = 0,
    Error = 1,
    Warn = 2,
    Info = 3,
    Debug = 4,
    All = 5,
}

public static class LogLevelHelper
{
    public static ConsoleColor GetColor(this LogLevel logLevel)
    {
        return logLevel switch
        {
            LogLevel.Info => ConsoleColor.Gray,
            LogLevel.Warn => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Debug => ConsoleColor.Cyan,
            _ => ConsoleColor.White
        };
    }
}
