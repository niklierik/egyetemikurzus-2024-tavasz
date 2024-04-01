using Calculator.Syntax;

namespace Calculator.IO.Logging;

public record class CommaToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkGray;

    public override string ToString() => ",";
}
