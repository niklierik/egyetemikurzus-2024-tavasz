namespace Calculator.Syntax.Tokens;

public sealed record class EndToken(bool Successful) : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.Black;

    public override string ToString() => "EOF";
}
