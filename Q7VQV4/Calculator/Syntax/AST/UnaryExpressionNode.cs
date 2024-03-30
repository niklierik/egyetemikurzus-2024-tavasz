namespace Calculator.Syntax.AST;

public class UnaryExpressionNode(LeafNode @operator, ISyntaxNode operand) : IExpressionNode
{
    public ICollection<ISyntaxNode> Children => [@operator, operand];

    public ConsoleColor DebugColor => ConsoleColor.Cyan;

    public override string ToString() => "UnaryExpr";
}
