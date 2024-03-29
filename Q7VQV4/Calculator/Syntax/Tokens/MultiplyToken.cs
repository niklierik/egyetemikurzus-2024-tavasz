namespace Calculator.Syntax.Tokens;

public class MultiplyToken : ISyntaxToken
{
    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public override string ToString() => "*";
}
