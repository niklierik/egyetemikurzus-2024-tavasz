namespace Calculator.Syntax.Tokens;

public class AddToken : IOperandToken
{
    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public int Priority => 0;

    public override string ToString() => "+";
}
