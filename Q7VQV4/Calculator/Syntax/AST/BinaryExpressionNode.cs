namespace Calculator.Syntax.AST;

public class BinaryExpressionNode(ISyntaxNode left, LeafNode @operator, ISyntaxNode right)
    : IExpressionNode
{
    public ICollection<ISyntaxNode> Children { get; } = [left, @operator, right];

    public ISyntaxNode Left { get; } = left;

    public LeafNode Operator { get; } = @operator;

    public ISyntaxNode Right { get; } = right;

    public ConsoleColor DebugColor => ConsoleColor.Cyan;

    public override string ToString() => "BinaryExpr";
}
