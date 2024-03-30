namespace Calculator.Syntax.Tokens;

public class DivideToken : IOperandToken
{
    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public int Priority => 1;

    public override string ToString() => "/";
}
