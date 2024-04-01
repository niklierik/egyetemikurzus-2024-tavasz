namespace Calculator.Syntax.Tokens;

public record class MultiplyToken : IBinaryOperatorToken
{
    public ConsoleColor DebugColor => ConsoleColor.Yellow;

    public int BinaryPriority => 1;

    public override string ToString() => "*";
}
