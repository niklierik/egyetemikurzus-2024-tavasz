namespace Calculator.Evaluators.Exceptions;

public class ConstantOverwriteException : RuntimeException
{
    public ConstantOverwriteException() { }

    public ConstantOverwriteException(string? message)
        : base(message) { }

    public ConstantOverwriteException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
