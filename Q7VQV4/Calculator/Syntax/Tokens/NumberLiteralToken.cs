namespace Calculator.Syntax.Tokens;

public class NumberLiteralToken(double value, string rawValue) : ILiteralToken<double>
{

    public double Value { get; } = value;

    public string RawValue { get; } = rawValue;

    public override string ToString() => $"NumberLiteral({Value})";

    public ConsoleColor DebugColor => ConsoleColor.Green;
}
