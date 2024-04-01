namespace Calculator.Evaluators.Exceptions;

public class MissingNativeObjectException : RuntimeException
{
    public MissingNativeObjectException() { }

    public MissingNativeObjectException(string? message)
        : base(message) { }

    public MissingNativeObjectException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
