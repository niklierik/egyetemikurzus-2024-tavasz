namespace Calculator.Syntax.Tokens;

public interface ILiteralToken<T> : ISyntaxToken
{
    public T Value { get; }

    public string RawValue { get; }
}
