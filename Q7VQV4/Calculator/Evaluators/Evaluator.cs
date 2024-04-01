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

    public object? Evaluate(RootNode root)
    {
        object? previous = null;

        foreach (ISyntaxNode node in root.Children)
        {
            previous = GetEvaluatorFor(node).Evaluate(node);
        }

        return previous;
    }

    public ISubEvaluator GetEvaluatorFor(ISyntaxNode node)
    {
        Type? subEvalType = _subEvaluatorTypes.FirstOrDefault(type =>
        {
            EvaluatorForAttribute? attribute = type.GetCustomAttribute<EvaluatorForAttribute>();
            if (attribute is null)
            {
                return false;
            }
            return attribute.Type.IsAssignableFrom(node.GetType());
        });

        if (subEvalType is null)
        {
            throw new EvaluatorException($"Missing SubEvaluator for type {node.GetType()}.");
        }

        // TODO: di

        object? maybeSubEval = _serviceProvider.GetService(subEvalType);

        if (maybeSubEval is not ISubEvaluator subEval)
        {
            throw new EvaluatorException(
                $"Could not provide subevaluator of type {subEvalType} for node {node.GetType()}."
            );
        }

        return subEval;
    }

    public IBinaryOperator GetBinaryOperator(Type t)
    {
        Type? opType = _binaryOperators.FirstOrDefault(type =>
        {
            BinaryOpAttribute? attribute = type.GetCustomAttribute<BinaryOpAttribute>();
            if (attribute is null)
            {
                // TODO: log
                return false;
            }

            return attribute.OperatorType.Equals(t);
        });
        if (opType is null)
        {
            throw new EvaluatorException($"There is no Binary Operator for the {t} token.");
        }
        object? instance = _serviceProvider.GetService(opType);
        if (instance is not IBinaryOperator binaryOp)
        {
            throw new EvaluatorException($"Cannot instantiate BinaryOperator {opType} for {t}.");
        }

        return binaryOp;
    }

    public IUnaryOperator GetUnaryOperator(Type t)
    {
        Type? opType = _unaryOperators.FirstOrDefault(type =>
        {
            UnaryOpAttribute? attribute = type.GetCustomAttribute<UnaryOpAttribute>();
            if (attribute is null)
            {
                // TODO: log
                return false;
            }

            return attribute.OperatorType.Equals(t);
        });
        if (opType is null)
        {
            throw new EvaluatorException($"There is no Unary Operator for the token {t}.");
        }

        object? instance = _serviceProvider.GetService(opType);
        if (instance is not IUnaryOperator unaryOp)
        {
            throw new EvaluatorException($"Cannot instantiate UnaryOperator {opType} for {t}.");
        }

        return unaryOp;
    }
}
