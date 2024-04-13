using Calculator.Interpreters;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals;

[EvaluatorFor(typeof(LeafNode))]
public class LeafNodeEvaluator(IInterpreter interpreter) : ISubEvaluator
{
    private readonly IInterpreter _interpreter = interpreter;

    public Task<object?> Evaluate(ISyntaxNode arg)
    {
        if (arg is not LeafNode leafNode)
        {
            throw new EvaluatorException($"Cannot process node {arg} as Literal.");
        }
        if (leafNode.Token is NumberLiteralToken numberLiteral)
        {
            return Task.FromResult<object?>(numberLiteral.Value);
        }
        if (leafNode.Token is StringLiteralToken stringLiteral)
        {
            return Task.FromResult<object?>(stringLiteral.Value);
        }
        if (leafNode.Token is IdentifierToken identifier)
        {
            return Task.FromResult(_interpreter.State.GetVariable(identifier.Content));
        }

        throw new EvaluatorException($"Unprocessable leaf node: '{leafNode.Token}'.");
    }
}
