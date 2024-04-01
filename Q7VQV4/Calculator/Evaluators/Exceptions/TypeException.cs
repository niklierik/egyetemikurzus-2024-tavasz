namespace Calculator.Evaluators.Exceptions;

public class TypeException : RuntimeException
{
    public TypeException() { }

    public TypeException(string? message)
        : base(message) { }

    public TypeException(string? message, Exception? innerException)
        : base(message, innerException) { }
}
