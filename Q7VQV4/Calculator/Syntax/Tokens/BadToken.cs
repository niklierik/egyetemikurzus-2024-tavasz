namespace Calculator.Syntax.Tokens;

public class BadToken(string content) : ISyntaxToken
{
    public string Content { get; } = content;

    public ConsoleColor DebugColor => ConsoleColor.Red;

    public override string ToString() => $"BadToken({Content})";
}