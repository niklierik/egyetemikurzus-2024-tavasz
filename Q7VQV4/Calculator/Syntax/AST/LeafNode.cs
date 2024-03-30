namespace Calculator.Syntax.AST;

public class LeafNode(ISyntaxToken literalToken) : ISyntaxNode
{
    public ISyntaxToken LiteralToken { get; } = literalToken;

    public ICollection<ISyntaxNode> Children => Enumerable.Empty<ISyntaxNode>().ToList();

    public ConsoleColor DebugColor => LiteralToken.DebugColor;

    public override string? ToString() => LiteralToken.ToString();
}
