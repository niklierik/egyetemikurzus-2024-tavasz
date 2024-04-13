namespace Calculator.Evaluators.ExpressionEvals.UnaryOps;

[AttributeUsage(AttributeTargets.Class)]
public class UnaryOpAttribute(Type type) : Attribute
{
    public Type OperatorType { get; } = type;
}
