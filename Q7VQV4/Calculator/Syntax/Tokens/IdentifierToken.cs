namespace Calculator.Syntax.Tokens;

public class IdentifierToken(string content) : ISyntaxToken
{
    public string Content { get; } = content;

    public ConsoleColor DebugColor => ConsoleColor.Green;

    public override string ToString() => $"IdentifierToken({Content})";
}