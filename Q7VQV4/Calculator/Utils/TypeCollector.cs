using System.Collections.Immutable;
using System.Reflection;
using Calculator.Evaluators.ExpressionEvals;
using Calculator.Evaluators.ExpressionEvals.BinaryOps;
using Calculator.Evaluators.ExpressionEvals.UnaryOps;
using Calculator.Syntax;
using Calculator.Syntax.Tokens;

namespace Calculator.Utils;

public class TypeCollector : ITypeCollector
{
    public IReadOnlyList<Type> GetConstantStringTokens(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(type =>
                typeof(ISyntaxToken).IsAssignableFrom(type)
                && !type.IsInterface
                && !type.IsAbstract
                && type.GetCustomAttribute<ConstantStringTokenAttribute>() is not null
            )
            .ToImmutableList();
    }

    public IReadOnlyList<Type> GetSubEvaluators(Assembly assembly)
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

    public IReadOnlyList<Type> GetBinaryOps(Assembly assembly)
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

    public IReadOnlyList<Type> GetUnaryOps(Assembly assembly)
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

    public IReadOnlyList<Type> GetBinaryOpTokens(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(type =>
                typeof(IBinaryOperatorToken).IsAssignableFrom(type)
                && !type.IsInterface
                && !type.IsAbstract
            )
            .ToImmutableList();
    }

    public IReadOnlyList<Type> GetUnaryOpTokens(Assembly assembly)
    {
        return assembly
            .GetTypes()
            .Where(type =>
                typeof(IUnaryOperatorToken).IsAssignableFrom(type)
                && !type.IsInterface
                && !type.IsAbstract
            )
            .ToImmutableList();
    }
}
