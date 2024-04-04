namespace Calculator.Syntax.Tokens;

public sealed record class NumberLiteralToken(double Value, string RawValue) : ILiteralToken<double>
{
    public override string ToString() => $"NumberLiteral({Value})";

    public ConsoleColor DebugColor => ConsoleColor.Green;
}
