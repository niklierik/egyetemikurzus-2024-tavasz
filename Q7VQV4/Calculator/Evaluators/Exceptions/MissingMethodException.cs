namespace Calculator.Evaluators.Exceptions;

public class MissingMethodException : RuntimeException
{
    public MissingMethodException() { }

    public MissingMethodException(string? message)
        : base(message) { }

    public MissingMethodException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
