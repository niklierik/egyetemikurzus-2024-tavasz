namespace Calculator.Syntax.Tokens;

[ConstantStringToken("(")]
public record class OpenBracketToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkRed;

    public override string ToString() => "(";
}
