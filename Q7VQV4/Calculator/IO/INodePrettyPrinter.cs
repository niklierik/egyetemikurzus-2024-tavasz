using Calculator.Syntax.AST;

namespace Calculator.IO;

public interface INodePrettyPrinter
{
    void Print(ISyntaxNode node);
}
