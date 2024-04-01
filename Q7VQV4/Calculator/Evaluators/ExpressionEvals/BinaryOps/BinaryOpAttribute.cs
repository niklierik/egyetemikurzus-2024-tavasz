using Calculator.Syntax.Tokens;

namespace Calculator.Evaluators.ExpressionEvals.BinaryOps;

[AttributeUsage(AttributeTargets.Class)]
public class BinaryOpAttribute(Type type) : Attribute
{
    public Type OperatorType { get; } = type;
}
