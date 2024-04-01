namespace Calculator.Evaluators.ExpressionEvals;

[AttributeUsage(AttributeTargets.Class)]
public class EvaluatorForAttribute(Type type) : Attribute
{
    public Type Type { get; } = type;
}
