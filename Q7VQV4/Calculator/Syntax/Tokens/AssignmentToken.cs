namespace Calculator.Syntax.Tokens;

[ConstantStringToken("=")]
public sealed record class AssignmentToken : IBinaryOperatorToken
{
    public int BinaryPriority => -1;

    public ConsoleColor DebugColor => ConsoleColor.DarkMagenta;

    public override string ToString() => "=";
}
