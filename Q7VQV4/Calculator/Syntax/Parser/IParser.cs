using Calculator.Syntax.AST;

namespace Calculator.Syntax.Parser;

public interface IParser
{
    RootNode Parse(IReadOnlyList<ISyntaxToken> tokens);
}
