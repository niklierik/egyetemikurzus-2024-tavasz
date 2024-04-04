using System.Collections.Immutable;
using Calculator.Evaluators.Exceptions;
using Calculator.Interpreters;
using Calculator.State;
using Calculator.Syntax.AST;
using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals;

[EvaluatorFor(typeof(MethodInvocationNode))]
public class MethodInvocationEvaluator(IInterpreter interpreter, IEvaluator evaluator)
    : ISubEvaluator
{
    private readonly IInterpreter _interpreter = interpreter;
    private readonly IEvaluator _evaluator = evaluator;

    public object? Evaluate(ISyntaxNode arg)
    {
        if (arg is not MethodInvocationNode methodInvocationNode)
        {
            throw new EvaluatorException($"Cannot execute MethodInvocation on node {arg}.");
        }

        var methodName = methodInvocationNode.MethodName;
        if (methodName is not LeafNode leafNode || leafNode.Token is not IdentifierToken id)
        {
            throw new SyntaxException($"Method invocation cannot be applied on node {methodName}.");
        }

        var name = id.Content;
        var method = _interpreter.State.Methods.GetValueOrDefault(name);

        if (method is null)
        {
            throw new MissingNativeObjectException($"Method '{name}' does not exist.");
        }

        object?[] args = methodInvocationNode
            .Args.Select(arg =>
            {
                var evaluator = _evaluator.GetEvaluatorFor(arg);
                if (evaluator is null)
                {
                    throw new EvaluatorException($"Cannot get evaluator for node {evaluator}.");
                }
                return evaluator.Evaluate(arg);
            })
            .ToArray();

        return method.Execute(args);
    }
}
