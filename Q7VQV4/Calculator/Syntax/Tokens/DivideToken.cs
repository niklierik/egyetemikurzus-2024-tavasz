namespace Calculator.Syntax.Tokens;

public class DivideToken : ISyntaxToken
{

    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public override string ToString() => "/";
}