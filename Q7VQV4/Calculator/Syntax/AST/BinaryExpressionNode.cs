namespace Calculator.Syntax.AST;

public class BinaryExpressionNode(ISyntaxNode left, LeafNode operand, ISyntaxNode right)
    : IExpressionNode
{
    public ICollection<ISyntaxNode> Children => [left, operand, right];

    public ConsoleColor DebugColor => ConsoleColor.Cyan;

    public override string ToString() => "BinaryExpr";
}
