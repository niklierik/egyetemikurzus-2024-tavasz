namespace Calculator.Syntax.AST;

public sealed record class LeafNode(ISyntaxToken Token) : ISyntaxNode
{
    public ICollection<ISyntaxNode> Children => Enumerable.Empty<ISyntaxNode>().ToList();

    public ConsoleColor DebugColor => Token.DebugColor;

    public override string? ToString() => Token.ToString();
}
