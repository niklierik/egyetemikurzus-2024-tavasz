namespace Calculator.Syntax.Tokens;

public sealed record class BadToken(string Content) : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.Red;

    public override string ToString() => $"BadToken({Content})";
}
