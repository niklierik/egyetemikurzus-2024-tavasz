namespace Calculator.Syntax.Tokens;

public record class IdentifierToken(string Content) : ISyntaxToken, IOperandToken
{
    public ConsoleColor DebugColor => ConsoleColor.Green;

    public override string ToString() => $"IdentifierToken({Content})";
}
