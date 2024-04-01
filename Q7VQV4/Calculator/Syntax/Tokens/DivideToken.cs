namespace Calculator.Syntax.Tokens;

[ConstantStringToken("/")]
public record class DivideToken : IBinaryOperatorToken
{
    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public int BinaryPriority => 1;

    public override string ToString() => "/";
}
