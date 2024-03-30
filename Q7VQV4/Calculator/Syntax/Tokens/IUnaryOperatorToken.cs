namespace Calculator.Syntax.Tokens;

public interface IUnaryOperatorToken : IOperatorToken
{
    int UnaryPriority { get; }
}
