namespace Calculator.Syntax.Tokens;

[ConstantStringToken(")")]
public record class CloseBracketToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkRed;

    public override string ToString() => ")";
}
