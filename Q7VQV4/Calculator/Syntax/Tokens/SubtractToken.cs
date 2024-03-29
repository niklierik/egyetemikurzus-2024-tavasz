namespace Calculator.Syntax.Tokens;

public class SubtractToken : ISyntaxToken
{

    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public override string ToString() => "-";
}