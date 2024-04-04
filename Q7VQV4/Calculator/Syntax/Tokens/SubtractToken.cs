namespace Calculator.Syntax.Tokens;

[ConstantStringToken("-")]
public sealed record class SubtractToken : IBinaryOperatorToken, IUnaryOperatorToken
{
    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public int BinaryPriority => 0;
    public int UnaryPriority => 10;

    public override string ToString() => "-";

    public bool PostOperator => false;
}
