namespace Calculator.Syntax.Tokens;

public interface ILiteralToken<T> : ISyntaxToken, IOperandToken
{
    public T Value { get; }

    public string RawValue { get; }
}
