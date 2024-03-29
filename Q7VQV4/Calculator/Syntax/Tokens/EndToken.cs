namespace Calculator.Syntax.Tokens;

public record EndToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.Black;

    public override string ToString() => "EOF";

    public bool Successful { get; }

    public EndToken(bool successful)
    {
        Successful = successful;
    }
}
