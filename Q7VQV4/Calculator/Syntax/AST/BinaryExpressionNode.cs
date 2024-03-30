namespace Calculator.Syntax.AST;

public class BinaryExpressionNode(ISyntaxNode left, LeafNode @operator, ISyntaxNode right)
    : IExpressionNode
{
    public ICollection<ISyntaxNode> Children => [left, @operator, right];

    public ConsoleColor DebugColor => ConsoleColor.Cyan;

    public override string ToString() => "BinaryExpr";
}
