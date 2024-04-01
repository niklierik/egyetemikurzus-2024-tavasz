namespace Calculator.Syntax.AST;

public class UnaryExpressionNode(LeafNode @operator, ISyntaxNode operand) : IExpressionNode
{
    public LeafNode Operator { get; } = @operator;
    public ISyntaxNode Operand { get; } = operand;

    public ICollection<ISyntaxNode> Children { get; } = [@operator, operand];

    public ConsoleColor DebugColor => ConsoleColor.Cyan;

    public override string ToString() => "UnaryExpr";
}
