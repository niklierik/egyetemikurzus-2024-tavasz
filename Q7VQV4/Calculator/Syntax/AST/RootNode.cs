namespace Calculator.Syntax.AST;

public record class RootNode(ICollection<ISyntaxNode> Expressions) : ISyntaxNode
{
    public ICollection<ISyntaxNode> Children { get; } = Expressions;

    public ConsoleColor DebugColor => ConsoleColor.DarkRed;

    public override string ToString() => "Root";
}
