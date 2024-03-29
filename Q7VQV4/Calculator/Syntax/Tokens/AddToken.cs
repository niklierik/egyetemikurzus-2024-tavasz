namespace Calculator.Syntax.Tokens;

public class AddToken : ISyntaxToken
{

    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public override string ToString() => "+";
}