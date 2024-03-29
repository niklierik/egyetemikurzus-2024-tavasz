namespace Calculator.IO;

public interface IHost
{
    void Write(object? message);
    void Write(object? message, ConsoleColor color);
    void Write(object? message, ConsoleColor color, ConsoleColor backgroundColor);
    void WriteLine(object? message);
    void WriteLine(object? message, ConsoleColor color);
    void WriteLine(object? message, ConsoleColor color, ConsoleColor backgroundColor);
    void WriteLine();
    string? ReadLine();

    void ResetColor();
}
