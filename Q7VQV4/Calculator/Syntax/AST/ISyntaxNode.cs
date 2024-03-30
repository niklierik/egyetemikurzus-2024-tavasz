namespace Calculator.Syntax.AST;

public interface ISyntaxNode
{
    public ICollection<ISyntaxNode> Children { get; }

    public ConsoleColor DebugColor { get; }
}
