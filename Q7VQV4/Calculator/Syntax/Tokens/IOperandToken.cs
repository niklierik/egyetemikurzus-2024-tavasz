namespace Calculator.Syntax.Tokens;

public interface IOperandToken : ISyntaxToken
{
    public int Priority { get; }
}
