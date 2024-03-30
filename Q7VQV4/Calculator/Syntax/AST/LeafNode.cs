namespace Calculator.Syntax.AST;

public class LeafNode(ISyntaxToken literalToken) : ISyntaxNode
{
    public ISyntaxToken Token { get; } = literalToken;

    public ICollection<ISyntaxNode> Children => Enumerable.Empty<ISyntaxNode>().ToList();

    public ConsoleColor DebugColor => Token.DebugColor;

    public override string? ToString() => Token.ToString();
}
