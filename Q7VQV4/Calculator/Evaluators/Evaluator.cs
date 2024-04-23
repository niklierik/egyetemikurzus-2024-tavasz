using System.Reflection;
using Calculator.Evaluators.ExpressionEvals;
using Calculator.Evaluators.ExpressionEvals.BinaryOps;
using Calculator.Evaluators.ExpressionEvals.UnaryOps;
using Calculator.Syntax.AST;
using Calculator.Utils;

namespace Calculator.Evaluators;

public class Evaluator : IEvaluator
{
    private readonly IReadOnlyCollection<Type> _subEvaluatorTypes;
    private readonly IReadOnlyCollection<Type> _binaryOperators;
    private readonly IReadOnlyCollection<Type> _unaryOperators;
    private readonly IServiceProvider _serviceProvider;

    public Evaluator(IServiceProvider serviceProvider, ITypeCollector typeCollector)
    {
        _serviceProvider = serviceProvider;

        Assembly assembly = GetType().Assembly;

        _subEvaluatorTypes = typeCollector.GetSubEvaluators(assembly);
        _binaryOperators = typeCollector.GetBinaryOps(assembly);
        _unaryOperators = typeCollector.GetUnaryOps(assembly);
    }

    public async Task<object?> Evaluate(RootNode root)
    {
        object? previous = null;

        foreach (ISyntaxNode node in root.Children)
        {
            previous = await GetEvaluatorFor(node).Evaluate(node);
        }

        return previous;
    }

    public ISubEvaluator GetEvaluatorFor(ISyntaxNode node)
    {
        Type? subEvalType = _subEvaluatorTypes.FirstOrDefault(evalType =>
            IsEvalTypeForNode(node, evalType)
        );

        if (subEvalType is null)
        {
            throw new EvaluatorException($"Missing SubEvaluator for type {node.GetType()}.");
        }

        object? maybeSubEval = _serviceProvider.GetService(subEvalType);

        if (maybeSubEval is not ISubEvaluator subEval)
        {
            throw new EvaluatorException(
                $"Could not provide subevaluator of type {subEvalType} for node {node.GetType()}."
            );
        }

        return subEval;
    }

    private static bool IsEvalTypeForNode(ISyntaxNode node, Type evalType)
    {
        EvaluatorForAttribute? attribute = evalType.GetCustomAttribute<EvaluatorForAttribute>();
        if (attribute is null)
        {
            return false;
        }
        return attribute.Type.IsAssignableFrom(node.GetType());
    }

    public IBinaryOperator GetBinaryOperator(Type tokenType)
    {
        Type? opType = _binaryOperators.FirstOrDefault(
            (binaryOpType) =>
                IsTheOperatorEvaluatorForTheToken<BinaryOpAttribute>(tokenType, binaryOpType)
        );
        if (opType is null)
        {
            throw new EvaluatorException($"There is no Binary Operator for the {tokenType} token.");
        }
        object? instance = _serviceProvider.GetService(opType);
        if (instance is not IBinaryOperator binaryOp)
        {
            throw new EvaluatorException(
                $"Cannot instantiate BinaryOperator {opType} for {tokenType}."
            );
        }

        return binaryOp;
    }

    public IUnaryOperator GetUnaryOperator(Type tokenType)
    {
        Type? opType = _unaryOperators.FirstOrDefault(unaryOpType =>
            IsTheOperatorEvaluatorForTheToken<UnaryOpAttribute>(tokenType, unaryOpType)
        );
        if (opType is null)
        {
            throw new EvaluatorException($"There is no Unary Operator for the token {tokenType}.");
        }

        object? instance = _serviceProvider.GetService(opType);
        if (instance is not IUnaryOperator unaryOp)
        {
            throw new EvaluatorException(
                $"Cannot instantiate UnaryOperator {opType} for {tokenType}."
            );
        }

        return unaryOp;
    }

    private static bool IsTheOperatorEvaluatorForTheToken<T>(Type tokenType, Type opType)
        where T : Attribute, IOperatorAttribute
    {
        T? attribute = opType.GetCustomAttribute<T>();
        if (attribute is null)
        {
            return false;
        }

        return attribute.OperatorType.Equals(tokenType);
    }
}
