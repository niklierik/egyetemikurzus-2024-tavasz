using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals;

[EvaluatorFor(typeof(LeafNode))]
public class LiteralEvaluator : ISubEvaluator
{
    public object? Evaluate(ISyntaxNode arg)
    {
        if (arg is not LeafNode leafNode)
        {
            throw new EvaluatorException($"Cannot process node {arg} as Literal.");
        }
        if (leafNode.Token is NumberLiteralToken literal)
        {
            return literal.Value;
        }

        throw new EvaluatorException($"Unprocessable leaf node: {arg.GetType()} with value.");
    }
}
