namespace Calculator.Syntax.AST;

public sealed record class MethodInvocationNode(
    LeafNode MethodName,
    LeafNode OpenArg,
    ICollection<ISyntaxNode> Args,
    LeafNode CloseArg
) : ISyntaxNode
{
    public ICollection<ISyntaxNode> Children => [MethodName, OpenArg, .. Args, CloseArg];

    public ConsoleColor DebugColor => ConsoleColor.Blue;
}
