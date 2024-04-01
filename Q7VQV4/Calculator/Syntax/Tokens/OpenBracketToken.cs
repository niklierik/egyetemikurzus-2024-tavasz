namespace Calculator.Syntax.Tokens;

public record class OpenBracketToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkRed;

    public override string ToString() => "(";
}
