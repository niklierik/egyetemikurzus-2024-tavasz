using Calculator.Interpreters;
using Calculator.State;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals;

[EvaluatorFor(typeof(LeafNode))]
public class LeafNodeEvaluator(IInterpreter<InterpreterState> interpreter) : ISubEvaluator
{
    private readonly IInterpreter<InterpreterState> _interpreter = interpreter;

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
        if (leafNode.Token is IdentifierToken identifier)
        {
            var variables = _interpreter.State.Variables;
            object? value = variables.GetValueOrDefault(identifier.Content);
            return value;
        }

        throw new EvaluatorException($"Unprocessable leaf node: '{leafNode.Token}'.");
    }
}
