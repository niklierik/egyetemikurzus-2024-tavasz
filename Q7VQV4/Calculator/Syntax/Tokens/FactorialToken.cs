namespace Calculator.Syntax.Tokens;

[ConstantStringToken("!")]
public class FactorialToken : IUnaryOperatorToken
{
    public ConsoleColor DebugColor => ConsoleColor.DarkMagenta;

    public int UnaryPriority => 11;

    public bool PostOperator => true;

    public override string ToString() => "!";
}
