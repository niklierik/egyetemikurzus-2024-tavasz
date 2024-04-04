namespace Calculator.Syntax.AST;

public sealed record class BinaryExpressionNode(
    ISyntaxNode Left,
    LeafNode Operator,
    ISyntaxNode Right
) : IExpressionNode
{
    public ICollection<ISyntaxNode> Children { get; } = [Left, Operator, Right];

    public ConsoleColor DebugColor => ConsoleColor.Cyan;

    public override string ToString() => "BinaryExpr";
}
