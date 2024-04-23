namespace Calculator.Evaluators.ExpressionEvals.BinaryOps;

[AttributeUsage(AttributeTargets.Class)]
public class BinaryOpAttribute(Type type, bool keepLeftTree = false, bool keepRightTree = false)
    : Attribute,
        IOperatorAttribute
{
    public Type OperatorType { get; } = type;
    public bool KeepLeftTree { get; } = keepLeftTree;
    public bool KeepRightTree { get; } = keepRightTree;
}
