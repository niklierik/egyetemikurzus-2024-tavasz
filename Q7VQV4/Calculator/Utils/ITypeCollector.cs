using System.Reflection;

namespace Calculator.Utils;

public interface ITypeCollector
{
    public IReadOnlyList<Type> GetConstantStringTokens(Assembly assembly);
    public IReadOnlyList<Type> GetSubEvaluators(Assembly assembly);
    public IReadOnlyList<Type> GetBinaryOps(Assembly assembly);
    public IReadOnlyList<Type> GetUnaryOps(Assembly assembly);
    public IReadOnlyList<Type> GetUnaryOpTokens(Assembly assembly);
    public IReadOnlyList<Type> GetBinaryOpTokens(Assembly assembly);
}
