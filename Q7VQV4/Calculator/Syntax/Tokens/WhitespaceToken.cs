namespace Calculator.Syntax.Tokens;

public record class WhitespaceToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.Black;

    public override string ToString() => "";
}
