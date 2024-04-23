namespace Calculator.Evaluators.ExpressionEvals.UnaryOps;

[AttributeUsage(AttributeTargets.Class)]
public class UnaryOpAttribute(Type type) : Attribute, IOperatorAttribute
{
    public Type OperatorType { get; } = type;
}
