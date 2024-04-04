namespace Calculator.Syntax.AST;

public sealed record class GrouppedExpressionNode(
    ISyntaxNode OpenToken,
    ISyntaxNode Expression,
    ISyntaxNode CloseToken
) : IExpressionNode
{
    public ICollection<ISyntaxNode> Children { get; } = [OpenToken, Expression, CloseToken];

    public ConsoleColor DebugColor => ConsoleColor.Magenta;

    public override string ToString() => "GroupExpr";
}
