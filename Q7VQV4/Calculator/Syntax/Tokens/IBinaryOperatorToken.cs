namespace Calculator.Syntax.Tokens;

public interface IBinaryOperatorToken : IOperatorToken
{
    int BinaryPriority { get; }
}
