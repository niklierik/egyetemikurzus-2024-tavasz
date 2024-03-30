namespace Calculator.Syntax.Tokens;

public class AddToken : IBinaryOperatorToken, IUnaryOperatorToken
{
    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public int BinaryPriority => 0;
    public int UnaryPriority => 10;

    public override string ToString() => "+";
}
