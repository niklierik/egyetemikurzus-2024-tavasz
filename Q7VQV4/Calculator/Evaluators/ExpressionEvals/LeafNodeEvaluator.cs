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
        if (leafNode.Token is NumberLiteralToken numberLiteral)
        {
            return numberLiteral.Value;
        }
        if (leafNode.Token is StringLiteralToken stringLiteral)
        {
            return stringLiteral.Value;
        }
        if (leafNode.Token is IdentifierToken identifier)
        {
            return _interpreter.State.GetVariable(identifier.Content);
        }

        throw new EvaluatorException($"Unprocessable leaf node: '{leafNode.Token}'.");
    }
}
