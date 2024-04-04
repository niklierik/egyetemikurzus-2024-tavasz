namespace Calculator.Syntax.AST;

public sealed record class ImportExpressionNode(
    LeafNode ImportKeyword,
    ICollection<LeafNode> Args,
    LeafNode From,
    LeafNode CSharpClassName
) : ISyntaxNode
{
    public ICollection<ISyntaxNode> Children => [ImportKeyword, .. Args, From, CSharpClassName];

    public ConsoleColor DebugColor => ConsoleColor.Blue;

    public override string ToString() => "Import Statement";
}
