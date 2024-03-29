namespace Calculator.Syntax.Tokens;

public class OpenBracketToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkRed;

    public override string ToString() => "(";
}
