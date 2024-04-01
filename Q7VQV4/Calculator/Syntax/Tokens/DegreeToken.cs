namespace Calculator.Syntax.Tokens;

[ConstantStringToken("°")]
public class DegreeToken : IUnaryOperatorToken
{
    public int UnaryPriority => 11;

    public bool PostOperator => true;

    public ConsoleColor DebugColor => ConsoleColor.DarkMagenta;
}
