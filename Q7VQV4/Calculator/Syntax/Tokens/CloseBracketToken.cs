namespace Calculator.Syntax.Tokens;

public record class CloseBracketToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkRed;

    public override string ToString() => ")";
}
