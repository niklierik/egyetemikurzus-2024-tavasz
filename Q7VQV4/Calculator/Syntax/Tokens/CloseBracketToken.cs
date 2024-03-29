namespace Calculator.Syntax.Tokens;

public class CloseBracketToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkRed;

    public override string ToString() => ")";
}
