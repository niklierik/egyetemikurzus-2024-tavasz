namespace Calculator.Syntax.Tokens;

public class WhitespaceToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.Black;

    public override string ToString() => "";
}
