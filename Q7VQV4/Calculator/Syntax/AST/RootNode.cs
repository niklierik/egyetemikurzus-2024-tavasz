namespace Calculator.Syntax.AST;

public class RootNode(ICollection<ISyntaxNode> expressions) : ISyntaxNode
{
    public ICollection<ISyntaxNode> Children { get; } = expressions;

    public ConsoleColor DebugColor => ConsoleColor.DarkRed;

    public override string ToString() => "Root";
}
