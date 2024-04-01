namespace Calculator.Syntax.AST;

public record class UnaryExpressionNode(LeafNode Operator, ISyntaxNode Operand) : IExpressionNode
{
    public ICollection<ISyntaxNode> Children { get; } = [Operator, Operand];

    public ConsoleColor DebugColor => ConsoleColor.Cyan;

    public override string ToString() => "UnaryExpr";
}
