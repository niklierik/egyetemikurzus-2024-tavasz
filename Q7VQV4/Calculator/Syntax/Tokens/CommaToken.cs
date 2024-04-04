using Calculator.Syntax;
using Calculator.Syntax.Tokens;

namespace Calculator.IO.Logging;

[ConstantStringToken(",")]
public sealed record class CommaToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkGray;

    public override string ToString() => ",";
}
