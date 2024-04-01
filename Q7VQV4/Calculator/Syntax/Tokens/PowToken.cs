namespace Calculator.Syntax.Tokens;

[ConstantStringToken("^")]
public record class PowToken : IBinaryOperatorToken
{
    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public int BinaryPriority => 2;

    public override string ToString() => "^";
}
