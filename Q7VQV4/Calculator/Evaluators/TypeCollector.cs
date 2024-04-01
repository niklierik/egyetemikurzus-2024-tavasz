using System.Collections.Immutable;
using System.Reflection;
using Calculator.Evaluators.ExpressionEvals;
using Calculator.Evaluators.ExpressionEvals.BinaryOps;
using Calculator.Evaluators.ExpressionEvals.UnaryOps;

namespace Calculator.Evaluators;

public static class TypeCollector
{
    public static IReadOnlyList<Type> GetSubEvaluators(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(type =>
                typeof(ISubEvaluator).IsAssignableFrom(type)
                && !type.IsInterface
                && !type.IsAbstract
                && type.GetCustomAttribute<EvaluatorForAttribute>() is not null
            )
            .ToImmutableList();
    }

    public static IReadOnlyList<Type> GetBinaryOps(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(type =>
                typeof(IBinaryOperator).IsAssignableFrom(type)
                && !type.IsInterface
                && !type.IsAbstract
                && type.GetCustomAttribute<BinaryOpAttribute>() is not null
            )
            .ToImmutableList();
    }

    public static IReadOnlyList<Type> GetUnaryOps(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(type =>
                typeof(IUnaryOperator).IsAssignableFrom(type)
                && !type.IsInterface
                && !type.IsAbstract
                && type.GetCustomAttribute<UnaryOpAttribute>() is not null
            )
            .ToImmutableList();
    }
}
