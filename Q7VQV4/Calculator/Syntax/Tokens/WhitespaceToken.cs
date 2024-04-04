namespace Calculator.Syntax.Tokens;

public sealed record class WhitespaceToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.Black;

    public override string ToString() => "";
}
