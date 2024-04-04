namespace Calculator.Syntax.Tokens;

[ConstantStringToken("from")]
public sealed record class FromToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.Blue;

    public override string ToString() => "from";
}
