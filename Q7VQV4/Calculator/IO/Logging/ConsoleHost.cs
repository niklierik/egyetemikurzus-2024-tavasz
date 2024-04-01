using System.Globalization;
using System.Text;

namespace Calculator.IO.Logging;

public class ConsoleHost : IHost, ILogTarget
{
    public ConsoleHost()
    {
        Console.OutputEncoding = Encoding.UTF8;
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
    }

    public void Write(object? message)
    {
        Console.Write(message);
    }

    public void Write(object? message, ConsoleColor foregroundColor)
    {
        Console.ForegroundColor = foregroundColor;
        Write(message);
    }

    public void Write(object? message, ConsoleColor foregroundColor, ConsoleColor backgroundColor)
    {
        Console.BackgroundColor = backgroundColor;
        Write(message, foregroundColor);
    }

    public void WriteLine(object? message)
    {
        Console.WriteLine(message);
    }

    public void WriteLine(object? message, ConsoleColor foregroundColor)
    {
        Console.ForegroundColor = foregroundColor;
        WriteLine(message);
    }

    public void WriteLine(
        object? message,
        ConsoleColor foregroundColor,
        ConsoleColor backgroundColor
    )
    {
        Console.BackgroundColor = backgroundColor;
        WriteLine(message, foregroundColor);
    }

    public void WriteLine()
    {
        Console.WriteLine();
    }

    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void ResetColor()
    {
        Console.ResetColor();
    }

    public Task Log(LogLevel logLevel, object? message, Exception? exception = null)
    {
        WriteLine($"[{logLevel}] - {message}", logLevel.GetColor());

        if (exception is not null)
        {
            WriteLine(exception, logLevel.GetColor());
        }

        return Task.CompletedTask;
    }
}
