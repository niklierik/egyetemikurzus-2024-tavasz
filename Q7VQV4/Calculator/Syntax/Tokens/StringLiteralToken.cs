using Newtonsoft.Json;

namespace Calculator.Syntax.Tokens;

public sealed record class StringLiteralToken(string Value, string RawValue) : ILiteralToken<string>
{
    public override string ToString() => $"StringLiteral({JsonConvert.SerializeObject(Value)})";

    public ConsoleColor DebugColor => ConsoleColor.Green;
}
