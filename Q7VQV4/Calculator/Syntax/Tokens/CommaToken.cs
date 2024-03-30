using Calculator.Syntax;

namespace Calculator.IO.Logging;

public class CommaToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkGray;

    public override string ToString() => ",";
}
