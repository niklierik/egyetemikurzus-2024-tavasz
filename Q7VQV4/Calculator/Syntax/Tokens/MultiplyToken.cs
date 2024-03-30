namespace Calculator.Syntax.Tokens;

public class MultiplyToken : IOperandToken, IBinaryOperandToken
{
    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public int Priority => 1;

    public override string ToString() => "*";
}
